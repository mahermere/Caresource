// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.OnBase.Core
//   ApixConfiguration.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Configuration
{
	/// <summary>
	///    Represents the data used to define a the apix configuration
	/// </summary>
	public class ApixConfiguration
	{
		/// <summary>
		///    Gets or sets the password of the apix configuration class.
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		///    Gets or sets the name of the user.
		/// </summary>
		public string UserName { get; set; }
	}
}