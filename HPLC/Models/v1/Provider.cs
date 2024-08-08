// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   Provider.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models.v1
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;

	/// <summary>
	///    Data describing a CareSource.WC.Services.Hplc.Models.v1.Provider object.
	/// </summary>
	public class Provider
	{
		/// <summary>
		///    Gets or sets Provider Action Type
		/// </summary>
		public string ActionType { get; set; }

		/// <summary>
		///    Gets or sets the Provider Board Certified
		/// </summary>
		public string BoardCertified { get; set; }

		/// <summary>
		///    Gets or sets the Provider Caqh Number
		/// </summary>
		[JsonProperty("CAQHNumber")]
		public string CaqhNumber { get; set; }

		/// <summary>
		///    Gets or sets the Provider Comments
		/// </summary>
		public string Comments { get; set; }

		/// <summary>
		///    Gets or sets the Provider Dea Number
		/// </summary>
		[JsonProperty("DEANumber")]
		public string DeaNumber { get; set; }

		/// <summary>
		///    Gets or sets the Provider Degree
		/// </summary>
		public string Degree { get; set; }

		/// <summary>
		///    Gets or sets the Provider Dob
		/// </summary>
		[JsonProperty("DOB")]
		public DateTime? Dob { get; set; }

		/// <summary>
		///    Gets or sets the Provider Group Npi
		/// </summary>
		[JsonProperty("GroupNPI")]
		public string GroupNpi { get; set; }

		/// <summary>
		///    Gets or sets the Provider Hospital Affiliation
		/// </summary>
		public string HospitalAffiliations { get; set; }

		/// <summary>
		///    Gets or sets the Provider Identifier
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		///    Gets or sets a value indicating whether this Provider is a
		/// Primary Care Provider.
		/// </summary>
		public bool IsPrimaryCareProvider { get; set; } = false;

		/// <summary>
		///    Gets or sets the Provider License Number
		/// </summary>
		public string LicenseNumber { get; set; }

		/// <summary>
		///    Gets or sets the Provider Locations
		/// </summary>
		public IEnumerable<Location> Locations { get; set; } = new List<Location>();

		/// <summary>
		///    Gets or sets the Provider Medicaid Identifier
		/// </summary>
		[JsonProperty("MedicaidID")]
		public string MedicaidId { get; set; }

		/// <summary>
		///    Gets or sets the Provider Medicare Identifier
		/// </summary>
		[JsonProperty("MedicareID")]
		public string MedicareId { get; set; }

		/// <summary>
		///    Gets or sets the Provider Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///    Gets or sets the Provider Npi
		/// </summary>
		[JsonProperty("NPI")]
		public string Npi { get; set; }

		/// <summary>
		///    Gets or sets the Provider Provider/Group Website
		/// </summary>
		public string ProviderGroupWebsite { get; set; }

		/// <summary>
		///    Gets or sets the Provider Race Ethnicity
		/// </summary>
		//[JsonProperty("ProviderRaceEthnicity")]
		public string RaceEthnicity { get; set; }

		/// <summary>
		///    Gets or sets the Provider Specialty
		/// </summary>
		public string SecondarySpecialty { get; set; }

		/// <summary>
		///    Gets or sets the Provider Specialty
		/// </summary>
		public string Specialty { get; set; }

		/// <summary>
		///    Gets or sets the Provider SSN
		/// </summary>
		[JsonProperty("SSN")]
		public string Ssn { get; set; }

		/// <summary>
		///    Gets or sets the Provider Tin
		/// </summary>
		[JsonProperty("TIN")]
		public string Tin { get; set; }

		/// <summary>
		///    Gets or sets the Provider Type
		/// </summary>
		//[JsonProperty("ProviderType")]
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [telemedicine services provided].
		/// </summary>
		public bool TelemedicineServicesProvided { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether [group identifier].
		/// </summary>
		/// <value>
		///   <c>true</c> if [group identifier]; otherwise, <c>false</c>.
		/// </value>
		public string GroupId { get; set; }


		/// <summary>
		/// Gets or sets the Provider Status
		/// </summary>
		//[JsonProperty("ProviderStatus")]
		public string Status { get; set; }

	}
}