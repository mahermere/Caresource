// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   KeywordUpdateResult.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6
{
	using System.Collections.Generic;

	public class KeywordUpdateResult
	{
		public IEnumerable<long> SuccessfulIds { get; set; }
		public IEnumerable<Failure> Failures { get; set; }
	}
}