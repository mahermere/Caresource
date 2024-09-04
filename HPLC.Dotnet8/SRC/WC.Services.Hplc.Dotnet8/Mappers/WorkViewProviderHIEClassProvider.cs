// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewProviderClassProvider.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Dotnet8.Mappers
{
    using Hyland.Unity.WorkView;
    using WC.Services.Hplc.Dotnet8.Mappers.Interfaces;
    using WC.Services.Hplc.Dotnet8.Models;

    public class WorkViewProviderHieClassProvider
        : BaseWorkViewObjectModelMapper<Provider>,
            IHieModelMapper<WorkViewObject, Provider>
    {

        public WorkViewProviderHieClassProvider() { }

        public override Provider GetMappedModel(WorkViewObject original)
        {
            Provider provider = base.GetMappedModel(original);

            provider.FirstName = GetStringValue(Constants.Provider.FirstName);
            provider.MiddleName = GetStringValue(Constants.Provider.MiddleName);
            provider.LastName = GetStringValue(Constants.Provider.LastName);
            provider.ProviderGroupWebsite = GetStringValue(Constants.Provider.ProviderGroupWebsite);
            provider.GroupNpi = GetStringValue(Constants.Provider.GroupNpi);
            provider.Type = GetStringValue(Constants.Provider.Type);
            provider.Status = GetStringValue(Constants.Provider.Status);
            provider.ActionType = GetStringValue(Constants.Provider.Action);
            provider.BoardCertified = GetStringValue(Constants.Provider.BoardCertified);
            provider.CaqhNumber = GetStringValue(Constants.Provider.CaqhNumber);
            provider.Comments = GetTextValue(Constants.Provider.Comments);
            provider.DeaNumber = GetStringValue(Constants.Provider.DeaNumber);
            provider.Degree = GetStringValue(Constants.Provider.Degree);
            provider.Dob = GetDateValue(Constants.Provider.DateOfBirth);
            provider.GroupId = GetStringValue(Constants.Provider.GroupId);
            provider.HospitalAffiliations = GetStringValue(Constants.Provider.HospitalAffiliations);
            provider.IsPrimaryCareProvider = GetBooleanValue(Constants.Provider.PrimaryCareProvider);
            provider.LicenseNumber = GetStringValue(Constants.Provider.LicenseNumber);
            provider.MedicaidId = GetStringValue(Constants.Provider.MedicaidId);
            provider.MedicareId = GetStringValue(Constants.Provider.MedicareId);
            provider.Npi = GetStringValue(Constants.Provider.Npi);
            provider.RaceEthnicity = GetStringValue(Constants.Provider.RaceEthnicity);
            provider.SecondarySpecialty = GetStringValue(Constants.Provider.SecondarySpecialty);
            provider.Specialty = GetStringValue(Constants.Provider.Specialty);
            provider.Ssn = GetStringValue(Constants.Provider.Ssn);
            provider.TelemedicineServicesProvided =
                GetBooleanValue(Constants.Provider.TeleMedicineServicesProvided);
            provider.Tin = GetStringValue(Constants.Provider.Tin);
            provider.LicenseState = GetStringValue(Constants.Provider.LicenseState);
            provider.Gender = GetStringValue(Constants.Provider.Gender);
            provider.SecondaryTaxonomyNumber =
                GetStringValue(Constants.Provider.SecondaryTaxonomyNumber);
            provider.DeaNumber = GetStringValue(Constants.Provider.DeaNumber);
            provider.CsrNumber = GetStringValue(Constants.Provider.CsrNumber);
            provider.NpSpecialtySupported = GetBooleanValue(Constants.Provider.NpSpecialtySupported);
            provider.HospitalBasedPhysician =
                GetBooleanValue(Constants.Provider.HospitalBasedPhysician);
            provider.Hospitalist = GetBooleanValue(Constants.Provider.Hospitalist);
            provider.Email = GetStringValue(Constants.Provider.Email);
            provider.ScopeOfPracticeSpecialtyObgynAndPpmPcp =
                GetStringValue(Constants.Provider.ScopeOfPracticeSpecialtyObgynAndPpmPcp);
            provider.LocationHasNpPaPracticing =
                GetBooleanValue(Constants.Provider.LocationHasNpPaPracticing);
            provider.GroupTaxonomy = GetStringValue(Constants.Provider.GroupTaxonomy);
            provider.WeekendHours = GetBooleanValue(Constants.Provider.WeekendHours);
            provider.EveningHours = GetBooleanValue(Constants.Provider.EveningHours);
            provider.ServeCsHcn = GetBooleanValue(Constants.Provider.ServeCsHcn);
            provider.Language = GetStringValue(Constants.Provider.Language);
            provider.LanguageOther = GetStringValue(Constants.Provider.LanguageOther);
            provider.BeenExcludedFromMedicareMedicaid =
                GetBooleanValue(Constants.Provider.BeenExcludedFromMedicareMedicaid);
            provider.ExclusionReason = GetStringValue(Constants.Provider.ExclusionReason);
            provider.RenderingProviders = GetBooleanValue(Constants.Provider.RenderingProviders);
            provider.LocumTenens = GetBooleanValue(Constants.Provider.LocumTenens);

            return provider;
        }

        public override WorkViewObject GetMappedModel(Provider original)
        {
            WorkViewObject wvo = base.GetMappedModel(original);

            return wvo;
        }

        public override WorkViewObject GetMappedModel(Object original)
        {
            WorkViewObject wvo = base.GetMappedModel(original);

            return wvo;
        }
    }
}