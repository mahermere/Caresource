// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   ImportProcessor
//   KeywordUpdate.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Models.v1
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	/// </summary>
	public class KeywordUpdate
	{
		/// <summary>
		///    Gets or sets the KeywordUpdate Document Identifier
		/// </summary>
		[Required]
		public long DocumentId { get; set; }


		/// <summary>
		///    Gets or sets the KeywordUpdate Keywords
		/// </summary>
		public IDictionary<string, string> Keywords { get; set; }
	}
}