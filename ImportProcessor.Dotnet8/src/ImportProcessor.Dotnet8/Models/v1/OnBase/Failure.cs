// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   Failure.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace

namespace WC.Services.ImportProcessor.Dotnet8.Models.v1.OnBase
{
	using System.Collections.Generic;
	using System.Text.Json.Serialization;

	public class Failure
	{
		/// <summary>
		///    Gets or sets the Failure Identifier
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		///    Gets or sets the Failure Messages
		/// </summary>
		[JsonPropertyName("messages")]
		public IEnumerable<string> Messages { get; set; }
	}
}