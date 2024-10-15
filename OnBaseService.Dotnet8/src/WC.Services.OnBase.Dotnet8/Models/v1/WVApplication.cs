// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   WVApplication.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Models.v1
{
	using System.Collections.Generic;
	using Hyland.Unity.WorkView;
    using WC.Services.OnBase.Dotnet8.Models.Base;

    public class WVApplication : BaseModel
	{
		public IEnumerable<WVClass> Classes { get; set; }

		public IEnumerable<WVFilters> Filters { get; set; }
	}
}