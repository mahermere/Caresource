// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewTinClassTin.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Mappers.v2
{
	using WC.Services.Hplc.Models.v2;

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