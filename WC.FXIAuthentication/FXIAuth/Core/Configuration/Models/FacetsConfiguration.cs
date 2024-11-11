// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    FacetsConfiguration.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Configuration.Models;

/// <summary>
///    Represents the data used to define a the facets configuration
/// </summary>
public class FacetsConfiguration
{
	/// <summary>
	///    Gets or sets the end point of the facets configuration class.
	/// </summary>
	public string Password { get; set; }


	/// <summary>
	///    Gets or sets the end point of the facets configuration class.
	/// </summary>
	public string UserName { get; set; }

	/// <summary>
	///    Gets or sets the end point of the facets configuration class.
	/// </summary>
	public string EndPoint { get; set; }

	/// <summary>
	///    Gets or sets the identity of the facets configuration class.
	/// </summary>
	public string FacetsIdentity { get; set; }

	/// <summary>
	///    Gets or sets the region of the facets configuration class.
	/// </summary>
	public string Region { get; set; }

	/// <summary>
	///    Gets or sets the cache timeout of the facets configuration class
	/// </summary>
	public long CacheTimeout { get; set; }

	/// <summary>
	///    Gets or sets the signon Method of the facets configuration class
	/// </summary>
	public object SignonMethod { get; set; }
}