using System;
using System.Linq;
using System.Collections.Generic;
using CareSource.WC.Core.Configuration.Interfaces;
using CareSource.WC.Core.Configuration.Models;
using CareSource.WC.Services.Eligibility.Adapters.Interfaces;
using CareSource.WC.Services.Eligibility.ConnectedServices.Facets.Eligibility.Get;

namespace CareSource.WC.Services.Eligibility.Adapters
{
    using CareSource.WC.Entities.Eligibility;

    public class FacetsFXIServiceEligibilityAdapter : IEligibilityAdapter<Eligibility>
	{
        private readonly ISettingsAdapter _settingsAdapter;

        public FacetsFXIServiceEligibilityAdapter(ISettingsAdapter settingsAdapter)
        {
            _settingsAdapter = settingsAdapter;
        }

        public IEnumerable<Eligibility> GetEligibility(long? contrivedKey)
		{
            var facetsConfig = _settingsAdapter.GetSection<FacetsConfiguration>("Facets");

            if (contrivedKey == null)
			{
				throw new ArgumentNullException(nameof(contrivedKey));
			}

			WebSvcProcessEligRangeSoapClient webSvcPer =
				new WebSvcProcessEligRangeSoapClient(
					WebSvcProcessEligRangeSoapClient.EndpointConfiguration.WebSvcProcessEligRangeSoap,
					$"{facetsConfig.EndPoint}FaWsvcInpProcessEligRange.asmx");

			Config configPer =
				new Config
				{
					Region = facetsConfig.Region,
					FacetsIdentity = facetsConfig.Identity
				};

			ProcessEligRangeResponse response =
				webSvcPer.ProcessEligRange_MemberKeyAsync(
						null,
						configPer,
						contrivedKey,
						DateTime.Now,
						DateTime.Now,
						string.Empty)
					.Result.ProcessEligRange_MemberKeyResult;

			if (response?.MEPE_COLL == null
				|| !response.MEPE_COLL.Any())
            {
                return null;
            }

			return response.MEPE_COLL
				.Select(MapTo)
                .Where(e => e != null)
				.AsEnumerable();
		}

        private Eligibility MapTo(REC_MEPE eligibility)
        {
            if (eligibility?.MEME_CK == null)
                return null;

            return new Eligibility()
            {
                CategoryDescription = eligibility.CSPD_CAT_DESC,
                ContractId = eligibility.PDPD_ID,
                ContrivedKey = eligibility.MEME_CK,
                EffectiveDate = eligibility.MEPE_EFF_DT,
                PlanName = eligibility.PLDS_DESC,
                PolicyStatus = eligibility.MEPE_ELIG_IND_DESC,
                TermDate = eligibility.MEPE_TERM_DT,
                EligibilityIndicator = eligibility.MEPE_ELIG_IND
            };
        }
	}
}