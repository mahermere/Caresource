// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   BaseWorkViewObjectModelMapper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Dotnet8.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Hyland.Unity.WorkView;
    using WC.Services.Hplc.Dotnet8.Mappers.Interfaces;
    using WC.Services.Hplc.Dotnet8.Models;
    using Attribute = Models.Attribute;
    using Object = Hyland.Unity.WorkView.Object;

    public abstract class BaseWorkViewObjectModelMapper<TMappedModel2>
        : IModelMapper<WorkViewObject, TMappedModel2>
        where TMappedModel2 : BaseWorkViewEntity, new()
    {
        protected WorkViewObject MainClassObject;
        protected Object MainObject;

        public virtual TMappedModel2 GetMappedModel(WorkViewObject original)
        {
            MainClassObject = original;

            return new TMappedModel2
            {
                Name = original.Name,
                ClassName = original.ClassName,
                ClassId = original.ClassId,
                CreatedBy = original.CreatedBy,
                CreatedDate = original.CreatedDate,
                Id = original.Id,
                RevisionBy = original.RevisionBy,
                RevisionDate = original.RevisionDate
            };
        }

        public virtual WorkViewObject GetMappedModel(TMappedModel2 original)
        {
            IEnumerable<PropertyInfo> properties = original.GetType()
                .GetProperties()
                .Where(
                    pi => pi.GetCustomAttributes(
                                typeof(WorkViewNameAttribute),
                                true)
                            .Length
                        > 0);

            return new WorkViewObject
            {
                Name = original.Name,
                ClassName = original.ClassName,
                ClassId = original.ClassId,
                CreatedBy = original.CreatedBy,
                CreatedDate = original.CreatedDate,
                Id = original.Id,
                RevisionBy = original.RevisionBy,
                RevisionDate = original.RevisionDate,
                Attributes = properties.Select(
                        pi => new Attribute
                        {
                            Name = ((WorkViewNameAttribute)pi.GetCustomAttribute(
                                    typeof(WorkViewNameAttribute)))
                                .Name,
                            Value = pi.GetValue(original)?
                                .ToString()
                        })
                    .ToList()
            };
        }

        public virtual WorkViewObject GetMappedModel(Object original)
        {
            MainObject = original;

            return new WorkViewObject
            {
                Name = MainObject.Name,
                ClassName = MainObject.Class.Name,
                ClassId = MainObject.Class.ID,
                CreatedBy = MainObject.CreatedBy?.DisplayName,
                CreatedDate = MainObject.CreatedDate,
                Id = MainObject.ID,
                RevisionBy = MainObject.RevisionBy?.DisplayName,
                RevisionDate = MainObject.RevisionDate,
                Attributes = MainObject.AttributeValues.Select(
                        av
                            => new Attribute
                            {
                                Name = av.Name,
                                Value = GetAttributeValue(av.Name)
                            })
                    .ToList()
            };
        }

        protected Attribute FindAttribute(
            WorkViewObject wvObject,
            string attributeName)
        {
            if (wvObject?.Attributes == null)
            {
                throw new NullReferenceException(
                    $"{nameof(MainClassObject)} or its Attributes are null.");
            }

            Attribute attribute = wvObject.Attributes.FirstOrDefault(
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

        private Attribute FindAttribute(string attributeName)
            => FindAttribute(
                MainClassObject,
                attributeName);

        protected AttributeValue FindAttributeValue(
            Object wvObject,
            string attributeName)
        {
            if (wvObject?.AttributeValues == null)
            {
                throw new NullReferenceException(
                    $"{nameof(MainClassObject)} or its Attributes are null.");
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

        private AttributeValue FindAttributeValue(string attributeName)
            => FindAttributeValue(
                MainObject,
                attributeName);

        protected string GetAttributeValue(string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.Value.ToString()
                : default;
        }

        protected bool GetBooleanAttributeValue(string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.BooleanValue
                : default;
        }

        protected bool GetBooleanValue(string attributeName)
        {
            Attribute attributeValue = FindAttribute(attributeName);

            return attributeValue.HasValue
                ? attributeValue.GetBooleanValue
                : default;
        }

        protected decimal? GetCurrency(string attributeName)
        {
            Attribute attributeValue = FindAttribute(attributeName);

            return attributeValue.HasValue
                ? attributeValue.GetDecimalValue
                : default;
        }

        protected decimal GetCurrencyAttributeValue(string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.CurrencyValue
                : default;
        }

        protected DateTime GetDateAttributeValue(string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.DateValue
                : default;
        }

        protected DateTime GetDateTimeAttributeValue(string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.DateTimeValue
                : default;
        }

        protected DateTime? GetDateTimeValue(string attributeName)
        {
            Attribute attributeValue = FindAttribute(attributeName);

            return attributeValue.HasValue
                ? attributeValue.GetDateTimeValue
                : default;
        }

        protected DateTime? GetDateValue(string attributeName)
        {
            Attribute attributeValue = FindAttribute(attributeName);

            return attributeValue.HasValue
                ? attributeValue.GetDateTimeValue.Date
                : default;
        }

        protected decimal GetDecimalAttributeValue(string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.DecimalValue
                : default;
        }

        protected decimal? GetDecimalValue(string attributeName)
        {
            Attribute attributeValue = FindAttribute(attributeName);

            return attributeValue.HasValue
                ? attributeValue.GetDecimalValue
                : default;
        }

        protected DependentObjectList GetDependentObjects()
            => MainObject.GetDependentObjects();

        protected long GetDocumentAttributeValue(string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.DocumentValue
                : default;
        }

        protected long? GetDocumentId(string attributeName)
        {
            Attribute attributeValue = FindAttribute(attributeName);

            return attributeValue.HasValue
                ? attributeValue.GetLongValue
                : default;
        }

        protected double GetDoubleAttributeValue(string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.FloatingPointValue
                : default;
        }

        protected double? GetDoubleValue(string attributeName)
        {
            Attribute attributeValue = FindAttribute(attributeName);

            return attributeValue.HasValue
                ? attributeValue.GetDoubleValue
                : default;
        }

        protected string GetEncryptedStringAttributeValue(string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.EncryptedAlphanumericValue
                : default;
        }

        protected string GetFormattedStringAttributeValue(string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.FormattedTextValue
                : default;
        }

        protected long GetLongAttributeValue(string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.IntegerValue
                : default;
        }

        protected long GetLongValue(string attributeName)
        {
            Attribute attributeValue = FindAttribute(attributeName);

            return attributeValue.HasValue
                ? attributeValue.GetLongValue
                : default;
        }

        protected Object GetRelatedObject(string dottedString)
            => MainObject.GetRelatedObject(dottedString);

        protected long? GetRelationship(string attributeName)
        {
            Attribute attributeValue = FindAttribute(attributeName);

            return attributeValue.HasValue
                ? attributeValue.GetLongValue
                : default;
        }

        protected long GetRelationshipAttributeValue(string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.RelationshipValue
                : default;
        }

        protected string GetStringAttributeValue(
            string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.AlphanumericValue.Trim()
                : default;
        }

        protected string GetStringValue(
            string attributeName)
        {
            Attribute attributeValue = FindAttribute(attributeName);

            return attributeValue.HasValue
                ? attributeValue.GetStringValue
                : default;
        }

        protected string GetTextAttributeValue(string attributeName)
        {
            AttributeValue attributeValue = FindAttributeValue(attributeName);

            return attributeValue.HasValue
                ? attributeValue.TextValue.Trim()
                : default;
        }

        protected string GetTextValue(string attributeName)
        {
            Attribute attributeValue = FindAttribute(attributeName);

            return attributeValue.HasValue
                ? attributeValue.GetStringValue
                : default;
        }

        public WorkViewObject GetMappedModel(object original)
        {
            throw new NotImplementedException();
        }
    }
}