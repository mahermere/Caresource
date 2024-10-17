// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   CreateAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Adapters.v5
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Threading.Tasks;
	//using CareSource.WC.OnBase.Core.Connection.Interfaces;
	//using CareSource.WC.OnBase.Core.ExtensionMethods;
	using WC.Services.WorkView.Dotnet8.Mappers.v5;
	using WC.Services.WorkView.Dotnet8.Models.v5;
	using Hyland.Unity.WorkView;
	using Application = Hyland.Unity.Application;
	using Attribute = Hyland.Unity.WorkView.Attribute;
	using ConfigurationManager = System.Configuration.ConfigurationManager;
	using Object = Hyland.Unity.WorkView.Object;
	using Microsoft.Extensions.Logging;
    using WC.Services.WorkView.Dotnet8.Mappers.v5.Interfaces;
    using WC.Services.WorkView.Dotnet8.Extensions;
    using WC.Services.WorkView.Dotnet8.Repository;

    public class CreateAdapter : BaseAdapter, ICreateAdapter
	{
		/// <param name="modelMapper">
		///    The model mapper.
		/// </param>
		/// <param name="repo">
		///    Repository object
		/// </param>
		/// <param name="dataSetAdapter"></param>
		/// <param name="logger"></param>
		public CreateAdapter(
			IModelMapper<WorkViewObject, WorkViewBaseObject> modelMapper,
			IRepository repo,
			IDataSetAdapter dataSetAdapter,
			log4net.ILog logger) :
		base(
			modelMapper,
			repo,
			dataSetAdapter,
			logger)
		{}

		public async Task<IEnumerable<WorkViewObject>> AsyncCreateObjects(
			IEnumerable<WorkViewBaseObject> workviewObjects)
		{
			IEnumerable<Task<WorkViewObject>> createdWvTasks = workviewObjects.Select(
					async wvo => await AsyncCreateWorkViewObject(wvo));

			return await Task.WhenAll(createdWvTasks);
		}

		private async Task<WorkViewObject> AsyncCreateWorkViewObject(
			WorkViewBaseObject wvo,
			Object parent = null)
		{
			Class wvClass = GetWvClass(wvo.ClassName);

			Object createdWvObject = wvClass.CreateObject(true);
			AttributeValueModifier attrModifier =
				createdWvObject.CreateAttributeValueModifier();

			if (parent != null)
			{
				Attribute wvAttr =
					parent.Class.Attributes.FirstOrDefault(a => a.Name.Equals(
						wvClass.Name,
						StringComparison.InvariantCultureIgnoreCase));

				SetAttribute(
					attrModifier,
					wvAttr,
					parent.ID.ToString());
			}

			foreach (KeyValuePair<string, string> attr in wvo.Properties)
			{
				Attribute wvAttr =
					wvClass.Attributes.FirstOrDefault(a => a.Name.Equals(
						attr.Key,
						StringComparison.InvariantCultureIgnoreCase));

				SetAttribute(
					attrModifier,
					wvAttr,
					attr.Value);
			}

			attrModifier.ApplyChanges();

			WorkViewObject mappedItem = ModelMapper.GetMappedModel(createdWvObject);

			await AsyncCreateRelated(wvo,
				parent,
				createdWvObject,
				mappedItem);

			return mappedItem;
		}

		private async Task AsyncCreateRelated(
			WorkViewBaseObject wvo,
			Object parent,
			Object createdWvObject,
			WorkViewObject mappedItem)
		{
			if (wvo.Related.SafeAny())
			{
				IEnumerable<Task<Task<WorkViewObject>>> relatedItems = wvo.Related?.Select(
					async related => AsyncCreateWorkViewObject(
						related,
						createdWvObject));
				mappedItem.Related = await Task.WhenAll(await Task.WhenAll(relatedItems));

				if (parent != null)
				{
					AttributeValueModifier parentModifier = parent.CreateAttributeValueModifier();

					foreach (WorkViewObject o in mappedItem.Related)
					{
						Attribute attr = parent.Class.Attributes.Find(o.ClassName);
						SetAttribute(
							parentModifier,
							attr,
							o.Id.ToString());
					}

					parentModifier.ApplyChanges();
				}
			}
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
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}

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
							new[] { "MM/dd/yyyy hh:mm:ss tt", "s" },
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

		private void ValidateRequestData(
			IEnumerable<Class> classes,
			WorkViewBaseObject wvObject,
			Dictionary<string, string[]> errors,
			string parentName = null)
		{
			Class wvClass = classes.FirstOrDefault(c => c.Name.Equals(wvObject.ClassName));

			if (wvClass == null)
			{
				errors.Add(
					$"{wvObject.ClassName}",
					new[] { $"Class not found: {wvObject.ClassName}" });

				return;
			}

			if (parentName != null)
			{
				if(wvClass.Attributes.All(a => a.Name != parentName))
				{
					errors.Add(
						$"{wvObject.ClassName}.{parentName}",
						new[] { $"Parent property not found: {parentName}" });
				}
			}

			foreach (KeyValuePair<string, string> attribute in wvObject.Properties)
			{
				Attribute attr = wvClass.Attributes
					.FirstOrDefault(att =>
						att.Name.Equals(attribute.Key, StringComparison.InvariantCultureIgnoreCase));

				if (attr == null)
				{
					errors.Add(
						$"{wvObject.ClassName}.{attribute.Key}",
						new[] { $"Property not found: {attribute.Key}" });
				}
				else
				{
					(bool IsValid, string Message) isValid = AttributeIsValid(
						wvClass,
						attr,
						attribute.Value);

					if (!isValid.IsValid)
					{
						errors.Add(
							$"{wvObject.ClassName}.{attribute.Key}",
							new[] { isValid.Message });
					}
				}
			}

			if (wvObject.Related.SafeAny())
			{
				foreach (WorkViewBaseObject related in wvObject.Related)
				{
					ValidateRequestData(
						classes,
						related,
						errors);
				}
			}
		}

		public (bool , Dictionary<string, string[]>) ValidateRequest(
			string workviewApplicationName,
			CreateRequest request)
		{
			Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

			SetWorkViewApplication(workviewApplicationName);

			IEnumerable<Class> classes = GetWorkViewClasses();

			foreach (WorkViewBaseObject wvObject in request.Data)
			{
				ValidateRequestData(
					classes,
					wvObject,
					errors);
			}

			return (!errors.Any(), errors);
		}

		public IEnumerable<WorkViewObject> CreateObjects(
			IEnumerable<WorkViewBaseObject> workviewObjects)
		{
			IEnumerable<WorkViewObject> createdWv =
				workviewObjects.Select(CreateWorkViewObject);

			return createdWv;
		}

		private WorkViewObject CreateWorkViewObject(WorkViewBaseObject wvo)
		{
			Object createdWvObject = CreateWvObject(wvo);
			
			if (wvo.Related.SafeAny())
			{
				foreach (WorkViewBaseObject related in wvo.Related)
				{
					CreateRelated(related, createdWvObject);
				}
				
			}

			WorkViewObject mappedItem = ModelMapper.GetMappedModel(createdWvObject);

			return mappedItem;
		}

		private Object CreateWvObject(WorkViewBaseObject wvo)
		{
			Class wvClass = GetWvClass(wvo.ClassName);

			Object createdWvObject = wvClass.CreateObject(true);

			AttributeValueModifier attrModifier =
				createdWvObject.CreateAttributeValueModifier();

			foreach (KeyValuePair<string, string> attr in wvo.Properties)
			{
				Attribute wvAttr =
					wvClass.Attributes.FirstOrDefault(a => a.Name.Equals(
						attr.Key,
						StringComparison.InvariantCultureIgnoreCase));

				SetAttribute(
					attrModifier,
					wvAttr,
					attr.Value);
			}

			attrModifier.ApplyChanges();

			return createdWvObject;
		}

		private Object CreateRelated(WorkViewBaseObject wvo, Object parent)
		{
			Class wvClass = GetWvClass(wvo.ClassName);
			if (wvClass == null)
			{
				return null;
			}

			Object createdWvObject = wvClass.CreateObject(true);

			if (wvClass.Attributes.SafeAny(a => a.Name.Equals(
					parent.Class.Name,
					StringComparison.CurrentCultureIgnoreCase)))
			{
				wvo.Properties.Add(parent.Class.Name, parent.ID.ToString());
			}
			else
			{
				Attribute wvAttr =
					parent.Class.Attributes.FirstOrDefault(a => a.Name.Equals(
						wvo.ClassName,
						StringComparison.InvariantCultureIgnoreCase));

				AttributeValueModifier parentAttrModifier =
					parent.CreateAttributeValueModifier();

				SetAttribute(
					parentAttrModifier,
					wvAttr,
					createdWvObject.ID.ToString());

				parentAttrModifier.ApplyChanges();
			}

			AttributeValueModifier attrModifier =
				createdWvObject.CreateAttributeValueModifier();

			foreach (KeyValuePair<string, string> attr in wvo.Properties)
			{
				Attribute wvAttr =
					wvClass.Attributes.FirstOrDefault(a => a.Name.Equals(
						attr.Key,
						StringComparison.InvariantCultureIgnoreCase));

				SetAttribute(
					attrModifier,
					wvAttr,
					attr.Value);
			}

			attrModifier.ApplyChanges();

			if (wvo.Related.SafeAny())
			{
				foreach (WorkViewBaseObject related in wvo.Related)
				{
					CreateRelated(related, createdWvObject);
				}
			}

			return createdWvObject;
		}
	}
}