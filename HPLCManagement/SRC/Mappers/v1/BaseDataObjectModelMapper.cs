// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   BaseWorkViewObjectModelMapper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Mappers.v1
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using HplcManagement.Mappers.v1.Interfaces;
	using HplcManagement.Models.v1;
	using Hyland.Unity.WorkView;
	using Attribute = HplcManagement.Models.v1.Attribute;
	using Object = Hyland.Unity.WorkView.Object;

	public abstract class BaseDataObjectModelMapper
		: IModelMapper<Data, Object>
	{
		protected Data MainClassObject;
		protected Object MainObject;
		protected bool IncludeChildren;

		public virtual Object GetMappedModel(Data original)
			=> throw new NotImplementedException();

		public virtual Data GetMappedModel(Object original, bool includeChildren = true)
		{
			MainObject = original;
			IncludeChildren = includeChildren;

			Data result = new Data
			{
				Name = MainObject.Name,
				ClassName = MainObject.Class.Name,
				ClassId = MainObject.Class.ID,
				CreatedBy = MainObject.CreatedBy?.DisplayName,
				CreatedDate = MainObject.CreatedDate,
				Id = MainObject.ID,
				RevisionBy = MainObject.RevisionBy?.DisplayName,
				RevisionDate = MainObject.RevisionDate,
				Properties = MainObject.AttributeValues
					.Where(a => !a.Name.Equals(Constants.Request.JsonData))
					.Where(a => a.Attribute.AttributeType != AttributeType.Relation)
					.ToDictionary(
						av =>
							av.Name,
						av =>
							GetAttributeValue(av.Name))
			};


			PopulateRelated(result);
			

			return result;
		}

		public abstract void PopulateRelated(Data data);

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

		protected DependentObjectList GetDependentObjects()
			=> MainObject.GetDependentObjects();

		protected Object GetRelatedObject(string dottedString)
			=> MainObject.GetRelatedObject(dottedString);

	}
}