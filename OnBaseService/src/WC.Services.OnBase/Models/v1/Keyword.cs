// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   Keyword.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Models.v1
{
	using CareSource.WC.Services.OnBase.Models.Base;

	public class Keyword : BaseModel
	{
		public string DataType { get; set; }

		public long MaxLength { get; set; }

		public string Mask { get; set; }

		public bool Hidden { get; set; } = false;

		public bool Required { get; set; } = false;
		public bool ReadOnly { get; internal set; }
	}
}