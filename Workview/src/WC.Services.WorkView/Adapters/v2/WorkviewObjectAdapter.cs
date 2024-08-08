using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CareSource.WC.Entities.Workview.v2;
using CareSource.WC.OnBase.Core.Connection.Interfaces;
using CareSource.WC.OnBase.Core.ExtensionMethods;
using CareSource.WC.Services.WorkView.Mappers.v2;
using Hyland.Unity.WorkView;
using Application = Hyland.Unity.Application;
using Attribute = Hyland.Unity.WorkView.Attribute;
using Object = Hyland.Unity.WorkView.Object;

namespace CareSource.WC.Services.WorkView.Adapters.v2

{
	public class WorkviewObjectAdapter : IWorkviewObjectAdapter<WorkviewObject>
	{
		private readonly IApplicationConnectionAdapter<Application>
			_applicationConnectionAdapter;

		private readonly
			IModelMapper<WorkviewObject, System.Tuple<Hyland.Unity.WorkView.Application, Object>>
			_modelMapper;

		/// <summary>Initializes a new instance of the <see cref="WorkviewObjectAdapter" /> class.</summary>
		/// <param name="modelMapper">
		///    The model mapper.
		/// </param>
		/// <param name="applicationConnectionAdapter">
		///    The application connection adapter.
		/// </param>
		public WorkviewObjectAdapter(
			IModelMapper<WorkviewObject, System.Tuple<Hyland.Unity.WorkView.Application, Object>>
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

			IEnumerable<Task<WorkviewObject>> wvObjects =
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

							return new WorkviewObject
							{
								Id = createdWvObject.ID, ClassName = createdWvObject.Class.Name,
							};
						});

			return await Task.WhenAll(wvObjects);
		}

		public WorkviewObject GetWorkviewObject(WorkviewObject WorkviewObject)
		{
			Hyland.Unity.WorkView.Application application =
				LoadWvApplication(WorkviewObject.ApplicationName);

			Class wvClass = LoadWvClass(
				WorkviewObject.ClassName,
				application);

			Object wvObject = wvClass.GetObjectByID(WorkviewObject.Id.Value);

			if (wvObject == null)
			{
				throw new Exception($"No {WorkviewObject.ClassName} found for id {WorkviewObject.Id}");
			}

			return _modelMapper.GetMappedModel(
				new Tuple<Hyland.Unity.WorkView.Application, Object>(
					application,
					wvObject));
		}

		public IEnumerable<WorkviewObject> SearchWorkviewObjects(
			WorkviewObject WorkviewObject)
		{
			Hyland.Unity.WorkView.Application application =
				LoadWvApplication(WorkviewObject.ApplicationName);

			Class wvClass =
				LoadWvClass(
					WorkviewObject.ClassName,
					application);

			Filter allFilter = LoadWvClassFilter(
				WorkviewObject.FilterName,
				wvClass,
				application);

			FilterQuery filterQuery = allFilter.CreateFilterQuery();

			if (WorkviewObject.Filters != null)
			{
				foreach (WorkviewFilter filter in WorkviewObject
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
						throw new ArgumentException($"Could not find Filter '{filter.Name}'.");
					}
					catch (InvalidOperationException)
					{
						throw new ArgumentException(
							$"Filter '{filter.Name}' is not allowed for '{filterQuery.Name}'.");
					}
					catch (Exception)
					{
						throw new ArgumentException(
							$"Filter '{filter.Name}' is not valid or is unknown.");
					}
				}
			}

			FilterQueryResultItemList items = filterQuery.Execute(10000);

			List<WorkviewObject> listItems =
				new List<WorkviewObject>();

			//List<WorkviewObject> results =
			//	new List<WorkviewObject>();

			foreach (FilterQueryResultItem item in items)
			{
				Object wvObject = wvClass.GetObjectByID(item.ObjectID);

				WorkviewObject obj =
					new WorkviewObject
					{
						ApplicationName = WorkviewObject.ApplicationName,
						ClassName = WorkviewObject.ClassName,
						Id = item.ObjectID,
						Filters = WorkviewObject.Filters,
						Attributes = WorkviewObject.Attributes,
						ApplicationId = application.ID,
						FilterName = WorkviewObject.FilterName,
						ClassId = wvClass.ID,
						CreatedBy = wvObject.CreatedBy?.DisplayName,
						CreatedDate = wvObject.CreatedDate,
						RevisionBy = wvObject.RevisionBy?.DisplayName,
						RevisionDate = wvObject.RevisionDate
					};

				List<WorkviewAttribute> attributes = new List<WorkviewAttribute>();
				if (WorkviewObject.Attributes != null &&
				    WorkviewObject.Attributes.Any())
				{
					foreach (WorkviewAttribute attr in WorkviewObject.Attributes)
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
							throw new ArgumentException($"Could not find Attribute '{attr.Name}'.");
						}
					}

					obj.Attributes = attributes;

					listItems.Add(obj);
				}
			}

			return listItems;
		}


		private Filter LoadWvClassFilter(
			string filterName,
			Class wvClass,
			Hyland.Unity.WorkView.Application application)
		{
			Filter filter = application.Filters.FirstOrDefault(
				f => f.Name == filterName && f.Class.Name == wvClass.Name);


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
		/// <exception cref="System.ArgumentException">
		///    Could not find WorkView Application Name
		///    '{applicationName}'.
		/// </exception>
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
		///    Given value '{value}' is over the max length of '{attribute.DataLength}' for attribute
		///    '{attribute.Name}'.
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
				case AttributeType.Text:
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
						value.ToSafeExactDateTime(
							"MM/dd/yyyy",
							new CultureInfo("en-US"),
							DateTimeStyles.None) ??
						throw new ArgumentException(
							$"Given value '{value}' could not be " +
							$"converted to a DateTime with the format 'MM/dd/yyyy' for Attribute '{attribute.Name}'."));
					break;
				case AttributeType.DateTime:
					attrModifier.SetAttributeValue(
						attribute,
						value.ToSafeExactDateTime(
							"MM/dd/yyyy hh:mm:ss tt",
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
	}
}