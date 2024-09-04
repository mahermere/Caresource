// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewTinClassTin.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using WC.Services.Hplc.Dotnet8.Models;

namespace WC.Services.Hplc.Dotnet8.Mappers
{

    public class WorkViewTinClassTin
        : BaseWorkViewObjectModelMapper<Tin>
    {
        public override Tin GetMappedModel(WorkViewObject original)
        {
            Tin tin = base.GetMappedModel(original);

            tin.EntityTin = GetStringValue(Constants.Tin.EntityTin);

            return tin;
        }
    }
}