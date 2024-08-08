// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   Claims.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Models.v1
{
	using ImportProcessor.Models.v1.OnBase;

	public class Claims : BaseWorkViewObject
	{
		public string ClaimNumber { get; set; }

		public string MemberId { get; set; }

		public long? LinkToDocumentData { get; set; }
	}
}