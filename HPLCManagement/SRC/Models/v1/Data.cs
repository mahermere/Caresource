// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   Data.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Models.v1
{
	using System.Collections.Generic;

	public class Data : BaseWorkViewEntity
	{
		public Dictionary<string, string> Properties { get; set; }
			= new Dictionary<string, string>();

		public IEnumerable<Data> Related { get; set; }
			= new List<Data>();
	}
}