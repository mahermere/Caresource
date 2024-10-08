// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   Claims.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Models.v1
{
	using WC.Services.ImportProcessor.Dotnet8.Models.v1.OnBase;

	public class Claims : BaseWorkViewObject
	{
		public string ClaimNumber { get; set; }

		public string MemberId { get; set; }

		public long? LinkToDocumentData { get; set; }
	}
}