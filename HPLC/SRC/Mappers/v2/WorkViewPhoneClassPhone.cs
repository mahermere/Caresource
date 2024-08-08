// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewPhoneClassPhone.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Mappers.v2
{
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using WC.Services.Hplc.Models.v2;

	public class WorkViewPhoneClassPhone
		: BaseWorkViewObjectModelMapper<Phone>
	{
		public override Phone GetMappedModel(WorkViewObject original)
		{
			Phone phone = base.GetMappedModel(original);

			phone.Type = GetStringValue(Constants.Phone.Type);
			phone.Number = GetStringValue(Constants.Phone.Number);

			return phone.Number.IsNullOrWhiteSpace()
				? null
				: phone;
		}
	}
}