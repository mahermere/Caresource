// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   Failure.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace

namespace CareSource.WC.Services.Models
{
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class Failure
	{
		/// <summary>
		///    Gets or sets the Failure Identifier
		/// </summary>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		///    Gets or sets the Failure Messages
		/// </summary>
		[JsonProperty(PropertyName = "messages")]
		public IEnumerable<string> Messages { get; set; }
	}
}