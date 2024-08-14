// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   FacetsConfiguration.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Configuration.Models
{
	/// <summary>
	///    Represents the data used to define a the facets configuration
	/// </summary>
	public class FacetsConfiguration
	{
		/// <summary>
		///    Gets or sets the end point of the facets configuration class.
		/// </summary>
		public string EndPoint { get; set; }

		/// <summary>
		///    Gets or sets the identity of the facets configuration class.
		/// </summary>
		public string Identity { get; set; }

		/// <summary>
		///    Gets or sets the region of the facets configuration class.
		/// </summary>
		public string Region { get; set; }
	}
}