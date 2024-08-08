// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   Failure.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System.Collections.Generic;

	public class Failure
	{
		/// <summary>
		///    Gets or sets the Failure Identifier
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		///    Gets or sets the Failure Messages
		/// </summary>
		public IEnumerable<string> Messages { get; set; }
	}
}