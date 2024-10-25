// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   KeywordUpdate.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6
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