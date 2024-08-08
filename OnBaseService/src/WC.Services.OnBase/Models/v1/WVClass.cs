// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   WVClass.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Models.v1
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using CareSource.WC.Services.OnBase.Models.Base;
	using Hyland.Unity.WorkView;

	public class WVClass : BaseModel
	{
		public WVClass(Class c)
		{
			Id = c.ID;
			Name = c.Name;
			Attributes = c.Attributes.Select(a => new WVAttribute(a));
		}

		public IEnumerable<WVAttribute> Attributes
		{
			get;
			set;
		}
	}
}