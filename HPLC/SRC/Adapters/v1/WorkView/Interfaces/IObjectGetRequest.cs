// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   IObjectGetRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System.Collections.Generic;

	internal interface IObjectGetRequest
	{
		string ApplicationName { get; set; }

		IEnumerable<string> Attributes { get; set; }

		string ClassName { get; set; }

		string FilterName { get; set; }

		IEnumerable<FilterRequest> Filters { get; set; }
		long ObjectId { get; set; }
	}
}