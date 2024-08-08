// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   WorkviewObjectAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Adapters.v1
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using CareSource.WC.Entities.WorkView;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.WorkView.Mappers;
	using Hyland.Unity;
	using Hyland.Unity.WorkView;
	using Application = Hyland.Unity.Application;
	using Attribute = Hyland.Unity.WorkView.Attribute;
	using Object = Hyland.Unity.WorkView.Object;

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
		/// <param name="workviewObject">The workview object.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">
		///    Could not find WorkView Application Name '{workviewObject.ApplicationName}'.
		///    or
		///    Could not find WorkView Class Name '{workviewObject.ClassName}'.
		///    or
		///    Could not find attribute '{attr.Name}' for class '{workviewObject.ClassName}' in
		///    application '{workviewObject.ApplicationName}'.
		/// </exception>
		/// <exception cref="System.Exception">
		///    Current running service account does not have access to create WorkView Class
		///    '{wvClass.Name}' object.
		/// </exception>
		public WorkviewObject CreateNewObject(
			WorkviewObject workviewObject)
		{
			Hyland.Unity.WorkView.Application wvApplication =
				LoadWvApplication(workviewObject.ApplicationName);

			Class wvClass = LoadWvClass(
				workviewObject.ClassName,
				wvApplication);

			Dictionary<string, Attribute> wvAttributes =
				wvClass.Attributes.ToDictionary(
					a => a.Name,
					a => a);

			Object createdWVObject = wvClass.CreateObject(true);

			if (createdWVObject == null)
			{
				throw new Exception(
					"Current running service account does not have access to create" +
					$" WorkView Class '{wvClass.Name}' object.");
			}

			AttributeValueModifier attrModifier = createdWVObject.CreateAttributeValueModifier();

			foreach (WorkviewAttribute attr in workviewObject.Attributes)
			{
				if (!wvAttributes.TryGetValue(
					attr.Name,
					out Attribute wvAttr))
				{
					throw new ArgumentException(
						$"Could not find attribute '{attr.Name}' " +
						$"for class '{workviewObject.ClassName}' in application " +
						$"'{workviewObject.ApplicationName}'.");
				}

				SetAttribute(
					attrModifier,
					wvAttr,
					attr.Value);
			}

			try
			{
				attrModifier.ApplyChanges();
			}
			catch (UnityAPIException ex)
			{
				if (ex.Message.Contains("is already in use"))
				{
					throw new ArgumentException(
						$"{ex.Message} " +
						$"Cannot store object for class '{workviewObject.ClassName}'" +
						$" in application '{workviewObject.ApplicationName}'.");
				}

				throw new Exception($"{ex.Message} {ex.StackTrace}");
			}

			return _modelMapper.GetMappedModel(
				new Tuple<Hyland.Unity.WorkView.Application, Object>(
					wvApplication,
					createdWVObject));
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
				throw new Exception($"No {workviewObject.ClassName} found for id {workviewObject.Id}");
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

			Class wvClass = LoadWvClass(
				workviewObject.ClassName,
				application);

			DynamicFilterQuery filterQuery = wvClass.CreateDynamicFilterQuery();

			if (workviewObject.Attributes != null)
			{
				foreach (WorkviewAttribute attribute in workviewObject.Attributes)
				{
					filterQuery.AddConstraint(
						attribute.Name,
						Operator.Equal,
						attribute.Value);
				}
			}

			FilterQueryResultItemList items = filterQuery.Execute(100);

			return
				items.Select(
					item
						=> GetWorkviewObject(
							new WorkviewObject
							{
								ApplicationName = workviewObject.ApplicationName,
								ClassName = workviewObject.ClassName,
								Id = item.ObjectID
							}));
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