// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Hplc
//   Provider.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models.v2
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using Newtonsoft.Json;

	/// <summary>
	///    Data describing a CareSource.WC.Services.Hplc.Models.v1.Provider object.
	/// </summary>
	public class Provider : BaseWorkViewEntity
	{
		public Provider()
			=> ClassName = Constants.Provider.ClassName;

		/// <summary>
		///    Gets or sets Provider Action Type
		/// </summary>
		[WorkViewName(Constants.Provider.Action)]
		public string ActionType { get; set; }

		/// <summary>
		///    Gets or sets the Provider Board Certified
		/// </summary>
		[WorkViewName(Constants.Provider.BoardCertified)]
		public string BoardCertified { get; set; }

		/// <summary>
		///    Gets or sets the Provider Caqh Number
		/// </summary>
		[JsonProperty(Constants.Provider.CaqhNumber)]
		[WorkViewName(Constants.Provider.CaqhNumber)]
		public string CaqhNumber { get; set; }

		/// <summary>
		///    Gets or sets the Provider Comments
		/// </summary>
		[WorkViewName(Constants.Provider.Comments)]
		public string Comments { get; set; }

		/// <summary>
		///    Gets or sets the Provider Dea Number
		/// </summary>
		[JsonProperty(Constants.Provider.DeaNumber)]
		[WorkViewName(Constants.Provider.DeaNumber)]
		public string DeaNumber { get; set; }

		/// <summary>
		///    Gets or sets the Provider Degree
		/// </summary>
		[WorkViewName(Constants.Provider.Degree)]
		public string Degree { get; set; }

		/// <summary>
		///    Gets or sets the Provider Dob
		/// </summary>
		[JsonProperty(Constants.Provider.DateOfBirth)]
		[WorkViewName(Constants.Provider.DateOfBirth)]
		public DateTime? Dob { get; set; }

		/// <summary>
		///    Gets or sets the Provider Group Npi
		/// </summary>
		[JsonProperty(Constants.Provider.GroupNpi)]
		[WorkViewName(Constants.Provider.GroupNpi)]
		public string GroupNpi { get; set; }

		/// <summary>
		///    Gets or sets the Provider Hospital Affiliation
		/// </summary>
		[WorkViewName(Constants.Provider.HospitalAffiliations)]
		public string HospitalAffiliations { get; set; }

		/// <summary>
		///    Gets or sets a value indicating whether this Provider is a
		///    Primary Care Provider.
		/// </summary>
		[WorkViewName(Constants.Provider.PrimaryCareProvider)]
		public bool IsPrimaryCareProvider { get; set; } = false;

		/// <summary>
		///    Gets or sets the Provider License Number
		/// </summary>
		[WorkViewName(Constants.Provider.LicenseNumber)]
		public string LicenseNumber { get; set; }

		/// <summary>
		///    Gets or sets the Provider Locations
		/// </summary>
		public IEnumerable<Location> Locations { get; set; } = new List<Location>();

		/// <summary>
		///    Gets or sets the Provider Medicaid Identifier
		/// </summary>
		[JsonProperty(Constants.Provider.MedicaidId)]
		[WorkViewName(Constants.Provider.MedicaidId)]
		public string MedicaidId { get; set; }

		/// <summary>
		///    Gets or sets the Provider Medicare Identifier
		/// </summary>
		[JsonProperty(Constants.Provider.MedicareId)]
		[WorkViewName(Constants.Provider.MedicareId)]
		public string MedicareId { get; set; }

		/// <summary>
		///    Gets or sets the Provider First Name
		/// </summary>
		[Required]
		[WorkViewName(Constants.Provider.FirstName)]
		public string FirstName { get; set; }
		/// <summary>
		///    Gets or sets the Provider Middle Name
		/// </summary>
		[WorkViewName(Constants.Provider.MiddleName)]
		public string MiddleName { get; set; }
		/// <summary>
		///    Gets or sets the Provider Last Name
		/// </summary>
		[Required]
		[WorkViewName(Constants.Provider.LastName)]
		public string LastName { get; set; }

		/// <summary>
		///    Gets or sets the Provider Npi
		/// </summary>
		[JsonProperty(Constants.Provider.Npi)]
		[WorkViewName(Constants.Provider.Npi)]
		public string Npi { get; set; }

		/// <summary>
		///    Gets or sets the Provider Provider/Group Website
		/// </summary>
		[WorkViewName(Constants.Provider.ProviderGroupWebsite)]
		public string ProviderGroupWebsite { get; set; }

		/// <summary>
		///    Gets or sets the Provider Race Ethnicity
		/// </summary>
		[JsonProperty(Constants.Provider.RaceEthnicity)]
		[WorkViewName(Constants.Provider.RaceEthnicity)]
		public string RaceEthnicity { get; set; }

		/// <summary>
		///    Gets or sets the Provider Specialty
		/// </summary>
		[WorkViewName(Constants.Provider.SecondarySpecialty)]
		public string SecondarySpecialty { get; set; }

		/// <summary>
		///    Gets or sets the Provider Specialty
		/// </summary>
		[WorkViewName(Constants.Provider.Specialty)]
		public string Specialty { get; set; }

		/// <summary>
		///    Gets or sets the Provider SSN
		/// </summary>
		[JsonProperty(Constants.Provider.Ssn)]
		[WorkViewName(Constants.Provider.Ssn)]
		public string Ssn { get; set; }

		/// <summary>
		///    Gets or sets the Provider Tin
		/// </summary>
		[JsonProperty(Constants.Provider.Tin)]
		[WorkViewName(Constants.Provider.Tin)]
		public string Tin { get; set; }

		/// <summary>
		///    Gets or sets the Provider Type
		/// </summary>
		[JsonProperty(Constants.Provider.Type)]
		[WorkViewName(Constants.Provider.Type)]
		public string Type { get; set; }

		/// <summary>
		///    Gets or sets a value indicating whether [telemedicine services provided].
		/// </summary>
		[WorkViewName(Constants.Provider.TeleMedicineServicesProvided)]
		public bool TelemedicineServicesProvided { get; set; } = false;

		/// <summary>
		///    Gets or sets a value indicating whether [group identifier].
		/// </summary>
		/// <value>
		///    <c>true</c> if [group identifier]; otherwise, <c>false</c>.
		/// </value>
		[WorkViewName(Constants.Provider.GroupId)]
		public string GroupId { get; set; }

		/// <summary>
		///    Gets or sets the Provider Status
		/// </summary>
		[JsonProperty(Constants.Provider.Status)]
		[WorkViewName(Constants.Provider.Status)]
		public string Status { get; set; }

		[JsonIgnore]
		public long RequestId { get; set; }

		[JsonProperty(Constants.Provider.Gender)]
		[WorkViewName(Constants.Provider.Gender)]
		public string Gender { get; set; }

		[JsonProperty(Constants.Provider.SecondaryTaxonomyNumber)]
		[WorkViewName(Constants.Provider.SecondaryTaxonomyNumber)]
		public string SecondaryTaxonomyNumber { get; set; }

		[JsonProperty(Constants.Provider.CsrNumber)]
		[WorkViewName(Constants.Provider.CsrNumber)]
		public string CsrNumber { get; set; }

		[JsonProperty(Constants.Provider.LicenseState)]
		[WorkViewName(Constants.Provider.LicenseState)]
		public string LicenseState { get; set; }

		[JsonProperty(Constants.Provider.NpSpecialtySupported)]
		[WorkViewName(Constants.Provider.NpSpecialtySupported)]
		public bool? NpSpecialtySupported { get; set; }

		[JsonProperty(Constants.Provider.LocumTenens)]
		[WorkViewName(Constants.Provider.LocumTenens)]
		public bool? LocumTenens { get; set; }

		[JsonProperty(Constants.Provider.HospitalBasedPhysician)]
		[WorkViewName(Constants.Provider.HospitalBasedPhysician)]
		public bool? HospitalBasedPhysician { get; set; }

		[JsonProperty(Constants.Provider.Hospitalist)]
		[WorkViewName(Constants.Provider.Hospitalist)]
		public bool? Hospitalist { get; set; }

		[JsonProperty(Constants.Provider.ScopeOfPracticeSpecialtyObgynAndPpmPcp)]
		[WorkViewName(Constants.Provider.ScopeOfPracticeSpecialtyObgynAndPpmPcp)]
		public string ScopeOfPracticeSpecialtyObgynAndPpmPcp { get; set; }

		[JsonProperty(Constants.Provider.LocationHasNpPaPracticing)]
		[WorkViewName(Constants.Provider.LocationHasNpPaPracticing)]
		public bool? LocationHasNpPaPracticing { get; set; }

		[JsonProperty(Constants.Provider.GroupTaxonomy)]
		[WorkViewName(Constants.Provider.GroupTaxonomy)]
		public string GroupTaxonomy { get; set; }

		[JsonProperty(Constants.Provider.WeekendHours)]
		[WorkViewName(Constants.Provider.WeekendHours)]
		public bool? WeekendHours { get; set; }

		[JsonProperty(Constants.Provider.EveningHours)]
		[WorkViewName(Constants.Provider.EveningHours)]
		public bool? EveningHours { get; set; }

		[JsonProperty(Constants.Provider.ServeCsHcn)]
		[WorkViewName(Constants.Provider.ServeCsHcn)]
		public bool? ServeCsHcn { get; set; }

		[JsonProperty(Constants.Provider.Language)]
		[WorkViewName(Constants.Provider.Language)]
		public string Language { get; set; }

		[JsonProperty(Constants.Provider.LanguageOther)]
		[WorkViewName(Constants.Provider.LanguageOther)]
		public string LanguageOther { get; set; }

		[JsonProperty(Constants.Provider.BeenExcludedFromMedicareMedicaid)]
		[WorkViewName(Constants.Provider.BeenExcludedFromMedicareMedicaid)]
		public bool? BeenExcludedFromMedicareMedicaid { get; set; }

		[JsonProperty(Constants.Provider.ExclusionReason)]
		[WorkViewName(Constants.Provider.ExclusionReason)]
		public string ExclusionReason { get; set; }

		[JsonProperty(Constants.Provider.RenderingProviders)]
		[WorkViewName(Constants.Provider.RenderingProviders)]
		public bool? RenderingProviders { get; set; }


		[JsonProperty(Constants.Provider.Email)]
		[WorkViewName(Constants.Provider.Email)]
		public string Email { get; set; }
	}
}