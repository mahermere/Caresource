// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewLocationClassLocation.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Mappers.v2
{
	using System.Collections.Generic;
	using System.Linq;
	using Hyland.Unity.WorkView;
	using WC.Services.Hplc.Models.v2;
	using Attribute = Models.v2.Attribute;

	public class WorkViewLocationClassLocation : BaseWorkViewObjectModelMapper<Location>
	{
		private readonly IModelMapper<WorkViewObject, Phone> _phoneMapper;

		public WorkViewLocationClassLocation(IModelMapper<WorkViewObject, Phone> phoneMapper)
			=> _phoneMapper = phoneMapper;

		public override Location GetMappedModel(WorkViewObject original)
		{
			Location location = base.GetMappedModel(original);

			location.Types = new List<string> { GetStringValue(Constants.Location.AddressType) };
			location.ActionType = GetStringValue(Constants.Location.ActionType);
			location.Notes = GetTextValue(Constants.Location.Notes);
			location.Capacity = (int)GetLongValue(Constants.Location.Capacity);
			location.City = GetStringValue(Constants.Location.City);
			location.County = GetStringValue(Constants.Location.County);
			location.GenderRestrictions = GetStringValue(Constants.Location.GenderRestrictions);
			location.IsPrimary = GetBooleanValue(Constants.Location.PrimaryLocation);
			location.MaxAge = (int)GetLongValue(Constants.Location.MaxAge);
			location.MinAge = (int)GetLongValue(Constants.Location.MinAge);
			location.PostalCode = GetStringValue(Constants.Location.PostalCode);
			location.State = GetStringValue(Constants.Location.State);
			location.Street1 = GetStringValue(Constants.Location.Street1);
			location.Street2 = GetStringValue(Constants.Location.Street2);

			if (original.Related.SafeAny())
			{
				IEnumerable<WorkViewObject> phones = original.Related.Where(
					wvo => wvo.ClassName.Equals(Constants.Phone.ClassName));

				location.Phones = phones.SafeAny()
					? phones
						.Select(wvo => _phoneMapper.GetMappedModel(wvo))
					: new List<Phone>();
			}

			return location;
		}

		public override WorkViewObject GetMappedModel(Location original)
		{
			WorkViewObject wvo = base.GetMappedModel(original);
			
			wvo.Related = original.Phones.Select(p => _phoneMapper.GetMappedModel(p));

			return wvo;
		}

		public override WorkViewObject GetMappedModel(Object original)
		{
			WorkViewObject wvo = base.GetMappedModel(original);

			DependentObjectList dependentObjects = GetDependentObjects();

			if (dependentObjects.SafeAny())
			{
				List<WorkViewObject> listDependentObjects = new List<WorkViewObject>();

				listDependentObjects.AddRange(
					from o in dependentObjects
					where o.Class.Name.Equals(Constants.Phone.ClassName)
					select _phoneMapper.GetMappedModel(o.Class.GetObjectByID(o.ID)));

				wvo.Related = listDependentObjects;
			}

			return wvo;
		}
	}
}