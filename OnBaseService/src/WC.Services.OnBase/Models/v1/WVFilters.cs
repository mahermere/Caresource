// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   WVFilters.cs
// </copyright>
// ------------------------------------------------------------------------------------------------using System;

namespace CareSource.WC.Services.OnBase.Models.v1
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using CareSource.WC.Services.OnBase.Models.Base;
	using Hyland.Unity.WorkView;

	public class WVFilters : BaseModel
	{
		public WVFilters(Filter f)
		{
			Id = f.ID;
			Name = f.Name;
		}
	}
}