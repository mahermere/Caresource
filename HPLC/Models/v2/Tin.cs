// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   Tin.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models.v2
{
	public class Tin: BaseWorkViewEntity
	{
		public Tin()
		{
			ClassName = Constants.Tin.ClassName;
		}
		[WorkViewName(Constants.Tin.EntityTin)]
		public string EntityTin { get; set; }

	}
}