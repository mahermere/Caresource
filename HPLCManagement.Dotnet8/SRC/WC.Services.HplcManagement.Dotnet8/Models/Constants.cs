// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   Constants.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.HplcManagement.Dotnet8.Models
{
    using System.Configuration;

    /// <summary>
    ///    Data and functions describing a CareSource.WC.Services.Hplc.Managers.v1.Constants object.
    /// </summary>
    public static class Constants
    {
        private static IServiceProvider? _serviceProvider;
        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        ///    Fields describing a CareSource.WC.Services.Hplc.Location object.
        /// </summary>
        public static class Location
        {
            /// <summary>
            ///    The action type
            /// </summary>
            public const string ActionType = "ActionType";

            /// <summary>
            ///    The address type
            /// </summary>
            public const string AddressType = "AddressType";

            /// <summary>
            ///    The capacity
            /// </summary>
            public const string Capacity = "Capacity";

            /// <summary>
            ///    The city
            /// </summary>
            public const string City = "City";

            /// <summary>
            ///    The name
            /// </summary>
            public const string ClassName = "Location";

            /// <summary>
            ///    The county
            /// </summary>
            public const string County = "County";

            /// <summary>
            ///    The gender restrictions
            /// </summary>
            public const string GenderRestrictions = "GenderRestrictions";

            /// <summary>
            ///    The link to provider
            /// </summary>
            public const string LinkToProvider = "linkToProvider";

            /// <summary>
            ///    The maximum age
            /// </summary>
            public const string MaxAge = "MaxAge";

            /// <summary>
            ///    The minimum age
            /// </summary>
            public const string MinAge = "MinAge";

            /// <summary>
            ///    The notes
            /// </summary>
            public const string Notes = "Notes";

            /// <summary>
            ///    The postal code
            /// </summary>
            public const string PostalCode = "PostalCode";

            /// <summary>
            ///    The primary location
            /// </summary>
            public const string PrimaryLocation = "PrimaryLocation";

            /// <summary>
            ///    The state
            /// </summary>
            public const string State = "State";

            /// <summary>
            ///    The street1
            /// </summary>
            public const string Street1 = "Street1";

            /// <summary>
            ///    The street2
            /// </summary>
            public const string Street2 = "Street2";
        }

        /// <summary>
        ///    Fields describing a CareSource.WC.Services.Hplc.Location.Phone object.
        /// </summary>
        public static class Phone
        {
            /// <summary>
            ///    The name
            /// </summary>
            public const string ClassName = "Phone";

            /// <summary>
            ///    The link to location
            /// </summary>
            public const string LinkToLocation = "linkToProviderLocations";

            /// <summary>
            ///    The number
            /// </summary>
            public const string Number = "PhoneNumber";

            /// <summary>
            ///    The type
            /// </summary>
            public const string Type = "PhoneType";
        }

        public static class Products
        {
            public const string ClassName = "RequestXProduct";
            public const string Code = "ProductCode";
            public const string LinkToRequest = "Request";
            public const string Name = "ProductName";
            public const string Product = "Product";
            public const string RelationshipToState = "RelationshipToState";
        }

        public static class Provider
        {
            /// <summary>
            ///    The action
            /// </summary>
            public const string Action = "ActionType";

            public const string BeenExcludedFromMedicareMedicaid = "BeenExcludedfromMedicareMedicaid";

            /// <summary>
            ///    The board certified
            /// </summary>
            public const string BoardCertified = "BoardCertified";

            /// <summary>
            ///    The caqh number
            /// </summary>
            public const string CaqhNumber = "CAQHNumber";

            /// <summary>
            ///    The WorkView class name
            /// </summary>
            public const string ClassName = "Provider";

            /// <summary>
            ///    The comments
            /// </summary>
            public const string Comments = "Comments";

            public const string CsrNumber = "CSRNumber";

            /// <summary>
            ///    The date of birth
            /// </summary>
            public const string DateOfBirth = "DOB";

            /// <summary>
            ///    The dea number
            /// </summary>
            public const string DeaNumber = "DEANumber";

            /// <summary>
            ///    The degree
            /// </summary>
            public const string Degree = "Degree";

            public const string EveningHours = "EveningHours";

            public const string ExclusionReason = "ExclusionReason";

            /// <summary>
            ///    The provider's gender
            /// </summary>
            public const string Gender = "Gender";

            /// <summary>
            ///    The prprid
            /// </summary>
            public const string GroupId = "PRPRID";

            /// <summary>
            ///    The group npi
            /// </summary>
            public const string GroupNpi = "GroupNPI";

            public const string GroupTaxonomy = "GroupTaxonomy";

            /// <summary>
            ///    The hospital affiliations
            /// </summary>
            public const string HospitalAffiliations = "HospitalAffiliations";

            public const string HospitalBasedPhysician = "HospitalBasedPhysician";

            public const string Hospitalist = "Hospitalist";

            public const string Language = "Language";

            public const string LanguageOther = "LanguageOther";

            /// <summary>
            ///    The license number
            /// </summary>
            public const string LicenseNumber = "LicenseNumber";

            public const string LicenseState = "LicenseState";

            /// <summary>
            ///    The link to request
            /// </summary>
            public const string LinkToRequest = "linkToRequest";

            public const string LocationHasNpPaPracticing = "LocationhasNPPAsPracticing";

            public const string LocumTenens = "LocumTenens";

            /// <summary>
            ///    The medicaid identifier
            /// </summary>
            public const string MedicaidId = "MedicaidID";

            /// <summary>
            ///    The medicare identifier
            /// </summary>
            public const string MedicareId = "MedicareID";

            public const string FirstName = "FirstName";

            public const string MiddleName = "MiddleName";

            public const string LastName = "LastName";

            /// <summary>
            ///    The npi
            /// </summary>
            public const string Npi = "NPI";

            public const string NpSpecialtySupported = "NPSpecialtySupported";

            /// <summary>
            ///    The primary care provider
            /// </summary>
            public const string PrimaryCareProvider = "PCP";

            /// <summary>
            ///    The provider group website
            /// </summary>
            public const string ProviderGroupWebsite = "ProviderGroupWebsite";

            /// <summary>
            ///    The provider race ethnicity
            /// </summary>
            public const string RaceEthnicity = "ProviderRaceEthnicity";

            public const string RenderingProviders = "RenderingProviders";

            public const string ScopeOfPracticeSpecialtyObgynAndPpmPcp
                = "ScopeofPracticeSpecialtyOBGYNandPMPPCP";

            /// <summary>
            ///    The secondary specialty
            /// </summary>
            public const string SecondarySpecialty = "SecondarySpecialty";

            /// <summary>
            ///    The secondary taxonomy number
            /// </summary>
            public const string SecondaryTaxonomyNumber = "SecondaryTaxonomyNumber";

            public const string ServeCsHcn = "ServeCSHCN";

            /// <summary>
            ///    The specialty
            /// </summary>
            public const string Specialty = "Specialty";

            /// <summary>
            ///    The SSN
            /// </summary>
            public const string Ssn = "SSN";

            /// <summary>
            ///    The provider status
            /// </summary>
            public const string Status = "ProviderStatus";

            /// <summary>
            ///    The tele-medicine services provided
            /// </summary>
            public const string TeleMedicineServicesProvided = "TelemedicineServicesProvided";

            /// <summary>
            ///    The tin
            /// </summary>
            public const string Tin = "TIN";

            /// <summary>
            ///    The provider type
            /// </summary>
            public const string Type = "ProviderType";

            public const string WeekendHours = "WeekendHours";

            public const string Email = "Email";
        }

        public static class Request
        {
            /// <summary>
            ///    The application number
            /// </summary>
            public const string ApplicationNumber = "ApplicationNumber";

            /// <summary>
            ///    The caresource received date
            /// </summary>
            public const string CareSourceReceivedDate = "CareSourceReceivedDate";

            /// <summary>
            ///    The change effective date
            /// </summary>
            public const string ChangeEffectiveDate = "ChangeEffectiveDate";

            public const string ClassName = "Request";

            /// <summary>
            ///    The contact email
            /// </summary>
            public const string ContactEmail = "EntityContactEmail";

            /// <summary>
            ///    The contact name
            /// </summary>
            public const string ContactName = "ContactName";

            /// <summary>
            ///    The contact phone
            /// </summary>
            public const string ContactPhone = "ContactPhoneNumber";

            /// <summary>
            ///    The request date
            /// </summary>
            public const string Date = "RequestDate";

            /// <summary>
            ///    The entity name
            /// </summary>
            public const string EntityName = "EntityName";

            /// <summary>
            ///    The link to tin
            /// </summary>
            public const string LinkToTin = "linkToTIN";

            /// <summary>
            ///    The request notes
            /// </summary>
            public const string Notes = "RequestNotes";

            /// <summary>
            ///    The primary state
            /// </summary>
            public const string PrimaryState = "PrimaryState";

            /// <summary>
            ///    The signatory email
            /// </summary>
            public const string SignatoryEmail = "SignatoryEmail";

            /// <summary>
            ///    The signatory name
            /// </summary>
            public const string SignatoryName = "SignatoryName";

            /// <summary>
            ///    The signatory title
            /// </summary>
            public const string SignatoryTitle = "SignatoryTitle";

            /// <summary>
            ///    The request source
            /// </summary>
            public const string Source = "RequestSource";

            /// <summary>
            ///    The request status
            /// </summary>
            public const string Status = "RequestStatus";

            /// <summary>
            ///    The request type
            /// </summary>
            public const string Type = "RequestType";

            /// <summary>
            ///    The request type
            /// </summary>
            public const string HealthPartnerAgreementType = "HealthPartnerAgreementType";

            public const string JsonData = "JsonData";
        }

        public class State
        {
            public const string Abbreviation = "State";
            public const string ClassName = "State";
            public const string Name = "StateName";
        }

        public static class Tin
        {
            public const string ClassName = "TIN";
            public const string EntityTin = "EntityTIN";
            public const string LinkToRequest = "LinkToRequest";
        }

        /// <summary>
        ///    Data and functions describing a
        ///    CareSource.WC.Services.Hplc.Managers.v1.Constants.WorkViewObjects object.
        /// </summary>
        public static class WorkViewObjects
        {
            public static readonly string ApplicationName = _serviceProvider.GetRequiredService<IConfiguration>().GetSection("WorkViewSettings").GetValue<string>("ApplicationName");
          //  ConfigurationManager.AppSettings.Get("Services.WorkView.ApplicationName");

            /// <summary>
            ///    Data and functions describing a
            ///    CareSource.WC.Services.Hplc.Managers.v1.Constants.WorkViewObjects.Filters object.
            /// </summary>
            public sealed class Filters
            {
                /// <summary>
                ///    The provider details
                /// </summary>
                public const string ProviderDetails = "API - Provider Details";

                public const string RequestDetails = "API - Request Details";
            }
        }

        public static class DataSetNames
        {
            public const string ActionType = "ActionType";
            public const string ActionTypeProviderLocations = "Action Type - Provider Locations";
            public const string AddressType = "Address Type";
            public const string ApiProducts = "API - Products";
            public const string PhoneTypeProvider = "Phone Type - Provider";
            public const string ProviderMaintenanceRequestTypes = "Provider Maintenance Request Types";
            public const string ProviderType = "ProviderType";
            public const string State = "State";
            public const string HplcProviderLanguage = "HPLC Provider - Language";
            public const string HealthPartnerAgreementType = "Health Partner Agreement Type";
        }

        public class Boards
        {
            public const string ClassName = "Boards";
        }

        public class Language
        {
            public const string ClassName = "Language";
            public const string LanguageProperty = "Language";
        }
    }
}