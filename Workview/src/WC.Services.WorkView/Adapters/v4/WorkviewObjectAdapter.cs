// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkviewObjectAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Adapters.v4

{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Data.SqlClient;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Runtime.Caching;
	using System.Threading.Tasks;
	using CareSource.WC.Entities.Workview.v2;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.WorkView.Mappers.v4;
	using CareSource.WC.Services.WorkView.Models.v4;
	using Dapper;
	using Hyland.Unity.WorkView;
	using Application = Hyland.Unity.Application;
	using Attribute = Hyland.Unity.WorkView.Attribute;
	using Object = Hyland.Unity.WorkView.Object;
	using Microsoft.Extensions.Logging;

	public class WorkViewObjectAdapter : IWorkViewObjectAdapter<WorkviewObject>
	{
		private int _timeout => Convert.ToInt32(
			ConfigurationManager.AppSettings.Get("OnBase.Connection.Timeout"));

		private readonly IApplicationConnectionAdapter<Application>
			_applicationConnectionAdapter;

		private readonly
			IModelMapper<WorkviewObject, Tuple<Hyland.Unity.WorkView.Application, Object>>
			_modelMapper;

		/// <summary>Initializes a new instance of the <see cref="WorkViewObjectAdapter" /> class.</summary>
		/// <param name="modelMapper">
		///    The model mapper.
		/// </param>
		/// <param name="applicationConnectionAdapter">
		///    The application connection adapter.
		/// </param>
		public WorkViewObjectAdapter(
			IModelMapper<WorkviewObject, Tuple<Hyland.Unity.WorkView.Application, Object>>
				modelMapper, 
			IApplicationConnectionAdapter<Application> applicationConnectionAdapter)
		{
			_modelMapper = modelMapper;
			_applicationConnectionAdapter = applicationConnectionAdapter;
		}

		/// <summary>Creates the new object.</summary>
		/// <param name="workviewObjects"></param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">
		///    Could not find WorkView Application Name '{WorkviewObject.ApplicationName}'.
		///    or
		///    Could not find WorkView Class Name '{WorkviewObject.ClassName}'.
		///    or
		///    Could not find attribute '{attr.Name}' for class '{WorkviewObject.ClassName}' in
		///    application '{WorkviewObject.ApplicationName}'.
		/// </exception>
		/// <exception cref="System.Exception">
		///    Current running service account does not have access to create WorkView Class
		///    '{wvClass.Name}' object.
		/// </exception>
		public async Task<IEnumerable<WorkviewObject>> CreateNewObjects(
			IEnumerable<WorkviewObject> workviewObjects)
		{
			Hyland.Unity.WorkView.Application wvApplication = null;
			Class wvClass = null;

			IEnumerable<Task<Object>> wvObjects =
				workviewObjects.OrderBy(e => e.ApplicationName)
					.Select(
						async wvo =>
						{
							if (wvApplication == null ||
							    !wvo.ApplicationName.Equals(wvApplication.Name))
							{
								wvApplication = LoadWvApplication(wvo.ApplicationName);
							}

							if (wvClass == null ||
							    !wvo.ClassName.Equals(wvClass.Name))
							{
								wvClass = LoadWvClass(
									wvo.ClassName,
									wvApplication);
							}

							Object createdWvObject = wvClass.CreateObject(true);

							if (createdWvObject == null)
							{
								throw new Exception(
									"Current running service account does not have access to create" +
									$" WorkView Class '{wvClass.Name}' object.");
							}

							AttributeValueModifier attrModifier =
								createdWvObject.CreateAttributeValueModifier();

							foreach (WorkviewAttribute attr in wvo.Attributes)
							{
								Attribute wvAttr =
									wvClass.Attributes.FirstOrDefault(a => a.Name == attr.Name);

								if (wvAttr == null)
								{
									throw new ArgumentException(
										$"Could not find attribute '{attr.Name}' " +
										$"for class '{wvClass.Name}' in application " +
										$"'{wvApplication.Name}'.");
								}

								SetAttribute(
									attrModifier,
									wvAttr,
									attr.Value);
							}

							attrModifier.ApplyChanges();

							return createdWvObject;
						});

			Object[] objects = await Task.WhenAll(wvObjects);

			
			return objects.Select(o => new WorkviewObject
			{
				ApplicationId = wvApplication.ID,
				ApplicationName = wvApplication.Name,
				ClassId = o.Class.ID,
				ClassName = o.Class.Name,
				Id = o.ID,
				Name = o.Name,
				CreatedBy = o.CreatedBy?.RealName,
				CreatedDate = o.CreatedDate,
				RevisionBy = o.RevisionBy?.RealName,
				RevisionDate = o.RevisionDate,
				Attributes = o.AttributeValues
					.Select(
						a => new WorkviewAttribute
						{
							Name = a.Name,
							Value = a.ToString()
						})
					.ToList()
			});
		}

		public WorkviewObject CreateObject(WorkviewObjectPostRequest wvo)
		{
			Hyland.Unity.WorkView.Application wvApplication = LoadWvApplication(wvo.ApplicationName);

			Class wvClass = LoadWvClass(
				wvo.ClassName,
				wvApplication);

			Object createdWvObject = wvClass.CreateObject(true);
			AttributeValueModifier attrModifier =
				createdWvObject.CreateAttributeValueModifier();

			foreach (WorkviewAttributeRequest attr in wvo.Attributes)
			{
				Attribute wvAttr =
					wvClass.Attributes.FirstOrDefault(a => a.Name == attr.Name);

				if (wvAttr == null)
				{
					throw new KeyNotFoundException(
						$"Could not find attribute '{attr.Name}' " +
						$"for class '{wvClass.Name}' in application " +
						$"'{wvApplication.Name}'.");
				}

				SetAttribute(
					attrModifier,
					wvAttr,
					attr.Value);
			}

			attrModifier.ApplyChanges();

			return new WorkviewObject
			{
				Id = createdWvObject.ID,
				ClassName = createdWvObject.Class.Name
			};
		}

		public WorkviewObject UpdateObject(WorkviewObjectPostRequest wvo)
		{
			Hyland.Unity.WorkView.Application wvApplication = LoadWvApplication(wvo.ApplicationName);
			Class wvClass = LoadWvClass(
				wvo.ClassName,
				wvApplication);
			
			Object current  = wvClass.GetObjectByID(wvo.ObjectId);

			if (current == null)
			{
				throw new Exception(
					$"{wvo.ApplicationName} {wvo.ClassName} not found for Id [{wvo.ObjectId}]");
			}

			AttributeValueModifier attrModifier =
				current.CreateAttributeValueModifier();

			foreach (WorkviewAttributeRequest attr in wvo.Attributes)
			{
				Attribute wvAttr =
					wvClass.Attributes.FirstOrDefault(a => a.Name == attr.Name);

				if (wvAttr == null)
				{
					throw new ArgumentException(
						$"Could not find attribute '{attr.Name}' " +
						$"for class '{wvClass.Name}' in application " +
						$"'{wvApplication.Name}'.");
				}

				SetAttribute(
					attrModifier,
					wvAttr,
					attr.Value);
			}

			attrModifier.ApplyChanges();

			return new WorkviewObject
			{
				Id = current.ID,
				ClassName = current.Class.Name
			};
		}

		public WorkviewObject GetWorkviewObject(WorkviewObject workviewObject)
		{
			Hyland.Unity.WorkView.Application application =
				LoadWvApplication(workviewObject.ApplicationName);

			Class wvClass = LoadWvClass(
				workviewObject.ClassName,
				application);

			Object wvObject = wvClass.GetObjectByID(workviewObject.Id.Value);

			if (wvObject == null)
			{
				throw new KeyNotFoundException($"No {workviewObject.ClassName} found for id {workviewObject.Id}");
			}

			return _modelMapper.GetMappedModel(
				new Tuple<Hyland.Unity.WorkView.Application, Object>(
					application,
					wvObject));
		}

		public IEnumerable<WorkviewObject> SearchWorkviewObjects(
			WorkviewObject workviewObject)
		{
			Hyland.Unity.WorkView.Application application =
				LoadWvApplication(workviewObject.ApplicationName);

			Class wvClass =
				LoadWvClass(
					workviewObject.ClassName,
					application);

			Filter allFilter = LoadWvClassFilter(
				workviewObject.FilterName,
				wvClass,
				application);

			FilterQuery filterQuery = allFilter.CreateFilterQuery();

			if (workviewObject.Filters != null)
			{
				foreach (WorkviewFilter filter in workviewObject
					.Filters)
				{
					try
					{
						filterQuery.AddConstraint(
							filter.Name,
							Operator.Equal,
							filter.Value);
					}
					catch (NullReferenceException)
					{
						throw new KeyNotFoundException($"Could not find Filter '{filter.Name}'.");
					}
					catch (InvalidOperationException)
					{
						throw new KeyNotFoundException(
							$"Filter '{filter.Name}' is not allowed for '{filterQuery.Name}'.");
					}
					catch (Exception)
					{
						throw new KeyNotFoundException(
							$"Filter '{filter.Name}' is not valid or is unknown.");
					}
				}
			}

			FilterQueryResultItemList items = filterQuery.Execute(10000);

			List<WorkviewObject> listItems =
				new List<WorkviewObject>();

			foreach (FilterQueryResultItem item in items)
			{
				WorkviewObject obj =
					new WorkviewObject
					{
						ApplicationName = application.Name,
						ClassName = wvClass.Name,
						Id = item.ObjectID,
						Filters = workviewObject.Filters,
						Attributes = null,
						ApplicationId = application.ID,
						FilterName = workviewObject.FilterName,
						ClassId = wvClass.ID
					};

				List<WorkviewAttribute> attributes = new List<WorkviewAttribute>();
				if (workviewObject.Attributes != null &&
				    workviewObject.Attributes.Any())
				{
					foreach (WorkviewAttribute attr in workviewObject.Attributes)
					{
						try
						{
							attributes.Add(
								new WorkviewAttribute
								{
									Name = attr.Name,
									Value = item.GetFilterColumnValue(attr.Name)
										.Value.ToString()
								});
						}
						catch
						{
							throw new KeyNotFoundException($"Could not find Attribute '{attr.Name}'.");
						}
					}

					obj.Attributes = attributes;

					listItems.Add(obj);
				}
				else
				{
					foreach (Attribute attr in allFilter.Class.Attributes)
					{
						try
						{
							string value = item.GetFilterColumnValue(attr.Name).AlphanumericValue;

							attributes.Add(new WorkviewAttribute
							{
								Name = attr.Name,
								Value = value
							});

						}
						catch (Exception)
						{
							// Work view filters do not show you all the the columns you
							// have to know exactly what you are looking for
							//we just want to ignore that it is not there and move on
						}
					}

					obj.Attributes = attributes;
					listItems.Add(obj);
				}
			}


			return listItems;
		}

		public IEnumerable<string> GetDataSetValues(DataSetRequest request)
		{
			Hyland.Unity.WorkView.Application application =
				LoadWvApplication(request.ApplicationName);

			Class wvClass =
				LoadWvClass(
					request.ClassName,
					application);

			string newline = Environment.NewLine;

			string sql = $"SELECT{newline}" +
				$"	[Application].rmApplicationId [Application Id],{newline}" +
				$"	[Application].rmApplicationName [Application Name],{newline}" +
				$"	[Class].ClassId [Class Id],{newline}" +
				$"	[Class].ClassName [Class Name],{newline}" +
				$"	[Attribute].AttributeId [Attribute Id],{newline}" +
				$"	[Attribute].AttributeName [Attribute Name],{newline}" +
				$"	[DataSet].DataSetName [DatSet Name],{newline}" +
				$"	[DataSet Values].DataValue [Available Values]{newline}" +
				$"FROM{newline}" +
				$"	rmApplication [Application] WITH(NOLOCK) {newline}" +
				$"	INNER JOIN rmApplicationClasses [Classes] WITH(NOLOCK) {newline}" +
				$"		ON Application.rmApplicationId = Classes.rmApplicationId{newline}" +
				$"	INNER JOIN rmClass [Class] WITH(NOLOCK) {newline}" +
				$"		ON Classes.classId = Class.classId{newline}" +
				$"	INNER JOIN rmClassAttributes [Class Attributes] WITH(NOLOCK) {newline}" +
				$"		ON Class.classId  = [Class Attributes].classId{newline}" +
				$"	INNER JOIN rmAttribute [Attribute] WITH(NOLOCK) {newline}" +
				$"		ON [Class Attributes].attributeId = Attribute.attributeId{newline}" +
				$"	LEFT JOIN [rmDataSet] [DataSet] WITH(NOLOCK) {newline}" +
				$"		ON [Attribute].DataSetId = DataSet.DataSetId{newline}" +
				$"	LEFT JOIN rmDataSetValue [DataSet Values] WITH(NOLOCK) {newline}" +
				$"		ON DataSet.DataSetId = [DataSet Values].DataSetId{newline}" +
				$"WHERE{newline}" +
				$"	[Application].rmApplicationId = {application.ID}{newline}" +
				$"	AND Class.classId = {wvClass.ID}{newline}" +
				$"	AND [DataSet].DataSetName = '{request.DataSetName}'{newline}" +
				$"ORDER BY{newline}" +
				$"	[Attribute Name], [Available Values]";

			List<string> items = new List<string>();

			using (IDbConnection db = new SqlConnection(
				ConfigurationManager.ConnectionStrings["OnBase.ConnectionString"]
					.ConnectionString))
			{
				using (IDataReader dataset = db.ExecuteReader(sql, commandTimeout: _timeout))
				{
					while (dataset.Read())
					{
						items.Add(dataset.GetString(dataset.GetOrdinal("Available Values")).SafeTrim());
					}
				}
			}
			return items;
		}

		private Filter LoadWvClassFilter(
			string filterName,
			Class wvClass,
			Hyland.Unity.WorkView.Application application)
		{
			Filter filter = application.Filters.FirstOrDefault(
				f => f.Name == filterName && f.Class.Name == wvClass.Name);

			foreach (Filter f in application.Filters)
			{
				Debug.WriteLine($"{f.Class.Name}-{f.Name}");
			}

			if (filter == null)
			{
				throw new ArgumentException(
					$"Could not find WorkView Class '{wvClass.Name}' Filter '{filterName}'.");
			}

			return filter;
		}

		private Class LoadWvClass(
			string className,
			Hyland.Unity.WorkView.Application wvApplication)
		{
			Class wvClass = wvApplication
				.Classes.Find(className);

			if (wvClass == null)
			{
				throw new ArgumentException(
					$"Could not find WorkView Class Name '{className}'.");
			}

			return wvClass;
		}

		/// <summary>Loads the wv application.</summary>
		/// <param name="applicationName">Name of the application.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">Could not find WorkView Application Name '{applicationName}'.</exception>
		private Hyland.Unity.WorkView.Application LoadWvApplication(string applicationName)
		{
			Hyland.Unity.WorkView.Application wvApplication = _applicationConnectionAdapter.Application
				.WorkView.Applications.Find(applicationName);

			if (wvApplication == null)
			{
				throw new ArgumentException(
					$"Could not find WorkView Application Name '{applicationName}'.");
			}

			return wvApplication;
		}

		/// <summary>Sets the attribute.</summary>
		/// <param name="attrModifier">The attribute modifier.</param>
		/// <param name="attribute">The attribute.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="System.ArgumentException">
		///    Given value '{value}' is over the max length of '{attribute.DataLength}' for attribute '{attribute.Name}'.
		///    or
		///    Given value '{value}' could not be converted to a boolean for Attribute '{attribute.Name}'.
		///    or
		///    Given value '{value}' could not be converted to a double for Attribute '{attribute.Name}'.
		///    or
		///    Given value '{value}' could not be converted to a DateTime with the format 'MM/dd/yyyy'
		///    for Attribute '{attribute.Name}'.
		///    or
		///    Given value '{value}' could not be converted to a DateTime with the format
		///    'MM/dd/yyyy hh:mm:ss tt' for Attribute '{attribute.Name}'.
		///    or
		///    Given value '{value}' could not be converted to a decimal for Attribute '{attribute.Name}'.
		///    or
		///    Given value '{value}' could not be converted to a long for Attribute '{attribute.Name}'.
		///    or
		///    Given value '{value}' could not be converted to a long for Attribute '{attribute.Name}'.
		/// </exception>
		/// <exception cref="System.Exception">
		///    Could not set attribute value for type
		///    '{attribute.AttributeType.ToString()}'. This has not been implemented.
		/// </exception>
		private void SetAttribute(
			AttributeValueModifier attrModifier,
			Attribute attribute,
			string value)
		{
			switch (attribute.AttributeType)
			{
				case AttributeType.Alphanumeric:
					if (value.Length > attribute.DataLength)
					{
						throw new ArgumentException(
							$"Given value '{value}' is over the max length of " +
							$"'{attribute.DataLength}' for attribute '{attribute.Name}'.");
					}

					attrModifier.SetAttributeValue(
						attribute,
						value);
					break;
				case AttributeType.Text:
					attrModifier.SetAttributeValue(
						attribute,
						value);
					break;
				case AttributeType.Boolean:
					attrModifier.SetAttributeValue(
						attribute,
						value.ToSafeBool() ??
						throw new ArgumentException(
							$"Given value '{value}' could not be " +
							$"converted to a boolean for Attribute '{attribute.Name}'."));
					break;
				case AttributeType.Currency:
					attrModifier.SetAttributeValue(
						attribute,
						value.ToSafeDouble() ??
						throw new ArgumentException(
							$"Given value '{value}' could not be " +
							$"converted to a double for Attribute '{attribute.Name}'."));
					break;
				case AttributeType.Date:
					attrModifier.SetAttributeValue(
						attribute,
						value.ToSafeDateTime() ??
						throw new ArgumentException(
							$"Given value '{value}' could not be " +
							$"converted to a DateTime with the format 'MM/dd/yyyy' for Attribute '{attribute.Name}'."));
					break;
				case AttributeType.DateTime:
					attrModifier.SetAttributeValue(
						attribute,
						value.ToSafeExactDateTime(
							new[]{"MM/dd/yyyy hh:mm:ss tt", "s"},
							new CultureInfo("en-US"),
							DateTimeStyles.None) ??
						throw new ArgumentException(
							$"Given value '{value}' could not be " +
							$"converted to a DateTime with the format 'MM/dd/yyyy hh:mm:ss tt' for Attribute '{attribute.Name}'."));
					break;
				case AttributeType.Decimal:
				case AttributeType.Float:
					attrModifier.SetAttributeValue(
						attribute,
						value.ToSafeDecimal() ??
						throw new ArgumentException(
							$"Given value '{value}' could not be " +
							$"converted to a decimal for Attribute '{attribute.Name}'."));
					break;
				case AttributeType.Integer:
					attrModifier.SetAttributeValue(
						attribute,
						value.ToSafeLong() ??
						throw new ArgumentException(
							$"Given value '{value}' could not be " +
							$"converted to a long for Attribute '{attribute.Name}'."));
					break;
				case AttributeType.Relation:
					attrModifier.SetAttributeValue(
						attribute,
						value.ToSafeLong() ??
						throw new ArgumentException(
							$"Given value '{value}' could not be " +
							$"converted to a long for Attribute '{attribute.Name}'."));
					break;
				default:
					throw new Exception(
						"Could not set attribute value for type " +
						$"'{attribute.AttributeType.ToString()}'. This has not been implemented.");
			}
		}

		private IEnumerable<Class> GetWorkViewClasses(
			string appName)
		{
			Hyland.Unity.WorkView.Application wvApplication = GetWvApplication();

			return wvApplication.Classes.ToList();

			Hyland.Unity.WorkView.Application GetWvApplication()
			{
				Hyland.Unity.WorkView.Application app =
					_applicationConnectionAdapter
						.Application
						.WorkView
						.Applications.FirstOrDefault(a => a.Name == appName);

				if (app == null)
				{
					throw new ArgumentException($"No workView Application found with name [{appName}]");
				}

				return app;
			}
		}
	}
}