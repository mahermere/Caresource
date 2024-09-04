// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewPhoneClassPhone.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using WC.Services.Hplc.Dotnet8.Models;

namespace WC.Services.Hplc.Dotnet8.Mappers
{
    public class WorkViewPhoneClassPhone
        : BaseWorkViewObjectModelMapper<Phone>
    {
        public override Phone GetMappedModel(WorkViewObject original)
        {
            Phone phone = base.GetMappedModel(original);

            phone.Type = GetStringValue(Constants.Phone.Type);
            phone.Number = GetStringValue(Constants.Phone.Number);

            return String.IsNullOrWhiteSpace(phone.Number)
                ? null
                : phone;
        }
    }
}