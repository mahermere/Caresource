// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   ProblemDetails.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models.Core
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;

	/// <summary>
	/// Data and functions describing a ProblemDetails object.
	/// </summary>
	public class ProblemDetails
	{
		/// <summary>
		/// Gets or sets the Problem Details Detail
		/// </summary>
		[JsonProperty("detail")]
		public string Detail { get; set; }

		/// <summary>
		/// Gets the extensions.
		/// </summary>
		/// <value>
		/// The extensions.
		/// </value>
		[JsonExtensionData]
		public IDictionary<string, object> Extensions { get; } =
			new Dictionary<string, object>(StringComparer.Ordinal);

		/// <summary>
		/// Gets or sets the Problem Details Instance
		/// </summary>
		[JsonProperty("instance")]
		public string Instance { get; set; }

		/// <summary>
		/// Gets or sets the Problem Details Status
		/// </summary>
		[JsonProperty("status")]
		public int? Status { get; set; }

		/// <summary>
		/// Gets or sets the Problem Details Title
		/// </summary>
		[JsonProperty("title")]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the Problem Details Type
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; set; }
	}
}