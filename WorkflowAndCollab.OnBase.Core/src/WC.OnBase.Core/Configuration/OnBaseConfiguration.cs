// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.OnBase.Core
//   OnBaseConfiguration.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Configuration
{
	/// <summary>
	///    Represents the data used to define a the OnBase configuration settings
	/// </summary>
	public class OnBaseConfiguration
	{
		/// <summary>
		///    Gets or sets the data source of the on base configuration class.
		/// </summary>
		public string DataSource { get; set; }

		/// <summary>
		///    Gets or sets the domain of the on base configuration class.
		/// </summary>
		public string Domain { get; set; }

		/// <summary>
		///    Gets or sets the password of the on base configuration class.
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		///    Gets or sets the URL of the on base configuration class.
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		///    Gets or sets the name of the user.
		/// </summary>
		public string UserName { get; set; }
	}
}