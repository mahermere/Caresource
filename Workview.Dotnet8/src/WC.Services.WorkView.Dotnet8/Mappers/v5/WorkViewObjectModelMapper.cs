// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   WorkViewObjectModelMapper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Mappers.v5
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	//using CareSource.WC.OnBase.Core.ExtensionMethods;
	using WC.Services.WorkView.Dotnet8.Models.v5;
	using Hyland.Unity;
	using Hyland.Unity.WorkView;
	using Attribute = Hyland.Unity.WorkView.Attribute;
	using Object = Hyland.Unity.WorkView.Object;
    using WC.Services.WorkView.Dotnet8.Extensions;
    using WC.Services.WorkView.Dotnet8.Mappers.v5.Interfaces;

    public class WorkViewObjectModelMapper<TMappedModel2>
		: IModelMapper<WorkViewObject, TMappedModel2>
		where TMappedModel2 : WorkViewBaseObject, new()
	{
		private WorkViewObject _mainClassObject;

		public TMappedModel2 GetMappedModel(WorkViewObject original)
		{
			_mainClassObject = original;

			return new TMappedModel2
			{
				ClassName = original.ClassName,
				Properties = original.Properties,
				Related = original.Related.Select(GetMappedModel)
			};
		}

		public virtual WorkViewObject GetMappedModel(TMappedModel2 original)
		{
			return new WorkViewObject
			{
				ClassName = original.ClassName,
				Properties = original.Properties,
				Related = original.Related.Select(
					c =>
						GetMappedModel((TMappedModel2)c))
			};
		}

		public virtual WorkViewObject GetMappedModel(
			Object original,
			Object parent = null)
		{
			if (original == null)
			{
				return null;
			}

			WorkViewObject wvo = new WorkViewObject
			{
				Name = original.Name,
				ClassName = original.Class.Name,
				CreatedBy = original.CreatedBy?.DisplayName,
				CreatedDate = original.CreatedDate,
				Id = original.ID,
				RevisionBy = original.RevisionBy?.DisplayName,
				RevisionDate = original.RevisionDate,
				Properties = original.AttributeValues.ToDictionary(
					av => av.Name,
					GetAttributeValue
				),
				Related = GetRelated(original, parent)?.ToList()
			};

			return wvo;
		}

		private IEnumerable<WorkViewObject> GetRelated(
			Object original,
			Object parent = null)
		{
			List<Attribute> relations = original.Class.Attributes.Where(
				a => a.AttributeType == AttributeType.Relation).ToList();

			List<WorkViewObject> oneToOneList = new List<WorkViewObject>();

			if (relations.SafeAny())
			{
				oneToOneList.AddRange(
					from attr in relations
					where attr.Name != parent?.Class.Name
					select GetMappedModel(GetRelatedObject(original, attr.Name), original));
			}

			List<WorkViewObject> children = new List<WorkViewObject>();
			DependentObjectList dol = GetDependentObjects(original);

			if (dol.SafeAny())
			{
				children.AddRange(
					from dependentObject in dol
					select
						GetMappedModel(
							dependentObject.Class.GetObjectByID(dependentObject.ID), original));
			}


			List<WorkViewObject> objects = new List<WorkViewObject>();
			objects.AddRange(oneToOneList);
			objects.AddRange(children);
			return objects;
		}

		protected KeyValuePair<string, string> FindAttribute(
			WorkViewObject wvObject,
			string attributeName)
		{
			if (wvObject?.Properties == null)
			{
				throw new NullReferenceException(
					$"{nameof(_mainClassObject)} or its Attributes are null.");
			}

			KeyValuePair<string, string> attribute = wvObject.Properties.FirstOrDefault(
				av => av.Key.Equals(
					attributeName,
					StringComparison.InvariantCultureIgnoreCase));

			if (attribute.Key == null)
			{
				throw new KeyNotFoundException(
					$"No Attribute found for {nameof(attributeName)}:{attributeName}");
			}

			return attribute;
		}

		private KeyValuePair<string, string> FindAttribute(string attributeName)
		{
			return FindAttribute(
				_mainClassObject,
				attributeName);
		}

		protected AttributeValue FindAttributeValue(
			Object wvObject,
			string attributeName)
		{
			if (wvObject?.AttributeValues == null)
			{
				throw new NullReferenceException(
					$"{nameof(_mainClassObject)} or its Attributes are null.");
			}

			AttributeValue attribute = wvObject.AttributeValues.FirstOrDefault(
				av => av.Name.Equals(
					attributeName,
					StringComparison.InvariantCultureIgnoreCase));

			if (attribute == null)
			{
				throw new KeyNotFoundException(
					$"No Attribute found for {nameof(attributeName)}:{attributeName}");
			}

			return attribute;
		}


		protected string GetAttributeValue(AttributeValue attributeValue)
		{
			switch (attributeValue.Attribute.AttributeType)
			{
				case AttributeType.Integer:
					return attributeValue.HasValue
						? attributeValue.IntegerValue.ToString()
						: default;
				case AttributeType.Currency:
					return attributeValue.HasValue
						? attributeValue.CurrencyValue.ToString()
						: default;
				case AttributeType.Float:
					return attributeValue.HasValue
						? attributeValue.FloatingPointValue.ToString()
						: default;
				case AttributeType.Date:
					return attributeValue.HasValue
						? attributeValue.DateValue.ToString("s")
						: default;
					;
				case AttributeType.DateTime:
					return attributeValue.HasValue
						? attributeValue.DateTimeValue.ToString("s")
						: default;
					;
				case AttributeType.Alphanumeric:
					return attributeValue.HasValue
						? attributeValue.AlphanumericValue
						: default;
					;
				case AttributeType.Text:
					return attributeValue.HasValue
						? attributeValue.TextValue
						: default;
					;
				case AttributeType.Relation:
					return attributeValue.HasValue
						? attributeValue.RelationshipValue.ToString()
						: default;
				case AttributeType.Boolean:
					return attributeValue.HasValue
						? attributeValue.BooleanValue.ToString()
						: default;
				case AttributeType.Document:
					return attributeValue.HasValue
						? attributeValue.DocumentValue.ToString()
						: default;
					;
				case AttributeType.FormattedText:
					return attributeValue.HasValue
						? attributeValue.FormattedTextValue
						: default;
					;
				case AttributeType.Decimal:
					return attributeValue.HasValue
						? attributeValue.DecimalValue.ToString()
						: default;
				case AttributeType.EncryptedAlphanumeric:
					return attributeValue.HasValue
						? attributeValue.EncryptedAlphanumericValue
						: default;
					;
				default:
					return attributeValue.HasValue
						? attributeValue.Value.ToString()
						: default;
			}
		}

		protected bool GetBooleanValue(Object wvObject, string attributeName)
		{
			KeyValuePair<string, string> attributeValue = FindAttribute(attributeName);

			return attributeValue.Value.HasValue()
				? attributeValue.Value.GetBooleanValue()
				: default;
		}

		protected decimal? GetCurrency(Object wvObject, string attributeName)
		{
			KeyValuePair<string, string> attributeValue = FindAttribute(attributeName);

			return attributeValue.Value.HasValue()
				? attributeValue.Value.GetDecimalValue()
				: default;
		}

		protected decimal GetCurrencyAttributeValue(Object wvObject, string attributeName)
		{
			AttributeValue attributeValue = FindAttributeValue(wvObject, attributeName);

			return attributeValue.HasValue
				? attributeValue.CurrencyValue
				: default;
		}

		protected DateTime GetDateAttributeValue(Object wvObject, string attributeName)
		{
			AttributeValue attributeValue = FindAttributeValue(wvObject, attributeName);

			return attributeValue.HasValue
				? attributeValue.DateValue
				: default;
		}

		protected DateTime GetDateTimeAttributeValue(Object wvObject, string attributeName)
		{
			AttributeValue attributeValue = FindAttributeValue(wvObject, attributeName);

			return attributeValue.HasValue
				? attributeValue.DateTimeValue
				: default;
		}

		protected DateTime? GetDateTimeValue(string attributeName)
		{
			KeyValuePair<string, string> attributeValue = FindAttribute(attributeName);

			return attributeValue.Value.HasValue()
				? attributeValue.Value.GetDateTimeValue()
				: default;
		}

		protected DateTime? GetDateValue(Object wvObject, string attributeName)
		{
			KeyValuePair<string, string> attributeValue = FindAttribute(attributeName);

			return attributeValue.Value.HasValue()
				? attributeValue.Value.GetDateValue()
				: default;
		}

		protected decimal GetDecimalAttributeValue(Object wvObject, string attributeName)
		{
			AttributeValue attributeValue = FindAttributeValue(wvObject, attributeName);

			return attributeValue.HasValue
				? attributeValue.DecimalValue
				: default;
		}

		protected decimal? GetDecimalValue(Object wvObject, string attributeName)
		{
			KeyValuePair<string, string> attributeValue = FindAttribute(attributeName);

			return attributeValue.Value.HasValue()
				? attributeValue.Value.GetDecimalValue()
				: default;
		}

		protected DependentObjectList GetDependentObjects(Object wvObject)
		{
			try
			{
				return wvObject.GetDependentObjects();
			}
			catch (Exception e)
			{
				//just swallow
				return null;
			}
		}

		protected long GetDocumentAttributeValue(Object wvObject, string attributeName)
		{
			AttributeValue attributeValue = FindAttributeValue(wvObject, attributeName);

			return attributeValue.HasValue
				? attributeValue.DocumentValue
				: default;
		}

		protected long? GetDocumentId(Object wvObject, string attributeName)
		{
			KeyValuePair<string, string> attributeValue = FindAttribute(attributeName);

			return attributeValue.Value.HasValue()
				? attributeValue.Value.GetLongValue()
				: default;
		}

		protected double GetDoubleAttributeValue(Object wvObject, string attributeName)
		{
			AttributeValue attributeValue = FindAttributeValue(wvObject, attributeName);

			return attributeValue.HasValue
				? attributeValue.FloatingPointValue
				: default;
		}

		protected double? GetDoubleValue(Object wvObject, string attributeName)
		{
			KeyValuePair<string, string> attributeValue = FindAttribute(attributeName);

			return attributeValue.Value.HasValue()
				? attributeValue.Value.GetDoubleValue()
				: default;
		}

		protected string GetEncryptedStringAttributeValue(Object wvObject, string attributeName)
		{
			AttributeValue attributeValue = FindAttributeValue(wvObject, attributeName);

			return attributeValue.HasValue
				? attributeValue.EncryptedAlphanumericValue
				: default;
		}

		protected string GetFormattedStringAttributeValue(Object wvObject, string attributeName)
		{
			AttributeValue attributeValue = FindAttributeValue(wvObject, attributeName);

			return attributeValue.HasValue
				? attributeValue.FormattedTextValue
				: default;
		}

		protected long GetLongAttributeValue(Object wvObject, string attributeName)
		{
			AttributeValue attributeValue = FindAttributeValue(wvObject, attributeName);

			return attributeValue.HasValue
				? attributeValue.IntegerValue
				: default;
		}

		protected long GetLongValue(Object wvObject, string attributeName)
		{
			KeyValuePair<string, string> attributeValue = FindAttribute(attributeName);

			return attributeValue.Value.HasValue()
				? attributeValue.Value.GetLongValue()
				: default;
		}

		protected Object GetRelatedObject(Object wvObject, string dottedString)
		{
			try
			{
				return wvObject.GetRelatedObject(dottedString);
			}
			catch (Exception e)
			{
				//  Just return null if object is not found
				return null;
			}
		}

		protected long? GetRelationship(Object wvObject, string attributeName)
		{
			KeyValuePair<string, string> attributeValue = FindAttribute(attributeName);

			return attributeValue.Value.HasValue()
				? attributeValue.Value.GetLongValue()
				: default;
		}

		protected long GetRelationshipAttributeValue(Object wvObject, string attributeName)
		{
			AttributeValue attributeValue = FindAttributeValue(wvObject, attributeName);

			return attributeValue.HasValue
				? attributeValue.RelationshipValue
				: default;
		}

		protected string GetStringAttributeValue(Object wvObject,
			string attributeName)
		{
			AttributeValue attributeValue = FindAttributeValue(wvObject, attributeName);

			return attributeValue.HasValue
				? attributeValue.AlphanumericValue.Trim()
				: default;
		}

		protected string GetStringValue(Object wvObject,
			string attributeName)
		{
			KeyValuePair<string, string> attributeValue = FindAttribute(attributeName);

			return attributeValue.Value.HasValue()
				? attributeValue.Value.GetStringValue()
				: default;
		}

		protected string GetTextAttributeValue(Object wvObject, string attributeName)
		{
			AttributeValue attributeValue = FindAttributeValue(wvObject, attributeName);

			return attributeValue.HasValue
				? attributeValue.TextValue.SafeTrim()
				: default;
		}

		protected string GetTextValue(Object wvObject, string attributeName)
		{
			KeyValuePair<string, string> attributeValue = FindAttribute(attributeName);

			return attributeValue.Value.HasValue()
				? attributeValue.Value.GetStringValue()
				: default;
		}
	}
}