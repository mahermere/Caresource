// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   Documents.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Models.v1
{
	using WC.Services.ImportProcessor.Dotnet8.Models.v1.OnBase;

	public class Documents : BaseWorkViewObject
	{
		public long? DocumentId { get; set; }

		public string LetterNumber { get; set; }
	}
}