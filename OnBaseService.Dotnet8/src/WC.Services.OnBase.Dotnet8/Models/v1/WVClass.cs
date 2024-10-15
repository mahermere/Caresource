// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   WVClass.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Models.v1
{
	using System.Collections.Generic;
	using System.Linq;
	using Hyland.Unity.WorkView;
    using WC.Services.OnBase.Dotnet8.Models.Base;

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