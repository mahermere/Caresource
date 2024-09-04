// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   ServiceConfiguration.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Dotnet8.Models.Core
{
	/// <summary>
	/// Data and functions describing a CareSource.WC.Services.Hplc.Models.Configuration object.
	/// </summary>
	/// <remarks>
	///	This class maps to the AppSettings.json file setting for the WorkView service connection
	/// </remarks>
	public class Configuration
	{
		/// <summary>
		/// Gets or sets the Configuration Services
		/// </summary>
		public Services Services { get; set; }
	}

	/// <summary>
	/// Data and functions describing a CareSource.WC.Services.Hplc.Models.Services object.
	/// </summary>
	public class Services
	{
		/// <summary>
		/// Gets or sets the Services Work View
		/// </summary>
		public WorkView WorkView { get; set; }
	}

	/// <summary>
	/// Data and functions describing a CareSource.WC.Services.Hplc.Models.WorkView object.
	/// </summary>
	public class WorkView
	{
		/// <summary>
		/// Gets or sets the Work View Root URL
		/// </summary>
		public string RootUrl { get; set; }

		/// <summary>
		/// Gets or sets the Work View Application Name
		/// </summary>
		public string ApplicationName { get; set; }
	}
}