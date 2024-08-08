// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   WVApplication.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Models.v1
{
	using System.Collections.Generic;
	using CareSource.WC.Services.OnBase.Models.Base;
	using Hyland.Unity.WorkView;

	public class WVApplication : BaseModel
	{
		public IEnumerable<WVClass> Classes { get; set; }

		public IEnumerable<WVFilters> Filters { get; set; }
	}
}