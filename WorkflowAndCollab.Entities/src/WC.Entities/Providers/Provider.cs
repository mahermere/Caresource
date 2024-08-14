// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Caresource.WC.Entities.WC.Entities
//   Provider.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Providers
{
	using CareSource.WC.Entities.Common;

	/// <summary>
	///    Represents the data used to define a the provider
	/// </summary>
	public class Provider
	{
		/// <summary>
		///    Gets or sets the address of the provider class.
		/// </summary>
		public Address Address { get; set; }

		/// <summary>
		///    Gets or sets the email of the provider class.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		///    Gets or sets the fax of the provider class.
		/// </summary>
		public string Fax { get; set; }

		/// <summary>
		///    Gets or sets the name of the provider class.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///    Gets or sets the network status of the provider class.
		/// </summary>
		public string NetworkStatus { get; set; }

		/// <summary>
		///    Gets or sets the npi of the provider class.
		/// </summary>
		public string Npi { get; set; }

		/// <summary>
		///    Gets or sets the phone of the provider class.
		/// </summary>
		public string Phone { get; set; }

		/// <summary>
		///    Gets or sets the provider identifier of the provider class.
		/// </summary>
		public string ProviderId { get; set; }

		/// <summary>
		///    Gets or sets the specialty of the provider class.
		/// </summary>
		public string Specialty { get; set; }
	}
}