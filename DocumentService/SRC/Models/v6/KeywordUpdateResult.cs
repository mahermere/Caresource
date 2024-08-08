// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   KeywordUpdateResult.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System.Collections.Generic;

	public class KeywordUpdateResult
	{
		public IEnumerable<long> SuccessfulIds { get; set; }
		public IEnumerable<Failure> Failures { get; set; }
	}
}