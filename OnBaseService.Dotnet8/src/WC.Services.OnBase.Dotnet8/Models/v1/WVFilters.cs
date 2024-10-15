// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   WVFilters.cs
// </copyright>
// ------------------------------------------------------------------------------------------------using System;

namespace WC.Services.OnBase.Dotnet8.Models.v1
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;

	using Hyland.Unity.WorkView;
    using WC.Services.OnBase.Dotnet8.Models.Base;

    public class WVFilters : BaseModel
	{
		public WVFilters(Filter f)
		{
			Id = f.ID;
			Name = f.Name;
		}
	}
}