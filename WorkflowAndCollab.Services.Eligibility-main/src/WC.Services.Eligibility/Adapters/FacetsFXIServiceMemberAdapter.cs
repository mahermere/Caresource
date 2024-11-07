using System;
using System.Linq;
using CareSource.WC.Core.Configuration.Interfaces;
using CareSource.WC.Core.Configuration.Models;
using CareSource.WC.Entities.Common;
using CareSource.WC.Entities.Members;
using CareSource.WC.Services.Eligibility.Adapters.Interfaces;
using CareSource.WC.Services.Eligibility.ConnectedServices.Facets.Member.Get;

namespace CareSource.WC.Services.Eligibility.Adapters
{
    public class FacetsFXIServiceMemberAdapter : IMemberAdapter<Member>
    {
        private readonly ISettingsAdapter _settingsAdapter;

        public FacetsFXIServiceMemberAdapter(ISettingsAdapter settingsAdapter)
        {
            _settingsAdapter = settingsAdapter;
        }

        public Member GetMemberBySubscriberId(string subscriberId, string subscriberSuffix)
        {
            var facetsConfig = _settingsAdapter.GetSection<FacetsConfiguration>("Facets");

            WebSvcSearchMember_v3SoapClient webSvcSm
                = new WebSvcSearchMember_v3SoapClient(
                    WebSvcSearchMember_v3SoapClient.EndpointConfiguration.WebSvcSearchMember_v3Soap,
                    $"{facetsConfig.EndPoint}FaWsvcInpSearchMember_v3.asmx");

            Config configSm =
                new Config
                {
                    Region = facetsConfig.Region,
                    FacetsIdentity = facetsConfig.Identity
                };

            REC_MEME member
                = webSvcSm.SearchMember_v3_SubscriberIdSuffixAsync(
                        null,
                        configSm,
                        subscriberId,
                        int.Parse(subscriberSuffix))
                    .Result.SearchMember_v3_SubscriberIdSuffixResult?.MEV0_COLL?.FirstOrDefault();

            return MapTo(member);
        }

        private Member MapTo(REC_MEME member)
        {
            if (member?.MEME_CK == null)
                return null;

            return new Member()
            {
                ContrivedKey = member.MEME_CK,
                DateOfBirth = member.MEME_BIRTH_DT,
                Email = member.SBAD_EMAIL_HOME,
                FirstName = member.MEME_FIRST_NAME,
                Hicn = member.MEME_HICN,
                HomeAddress = new Address
                {
                    Line1 = member.SBAD_ADDR1_HOME,
                    Line2 = member.SBAD_ADDR2_HOME,
                    Line3 = member.SBAD_ADDR3_HOME,
                    City = member.SBAD_CITY_HOME,
                    State = member.SBAD_STATE_HOME,
                    Zip = member.SBAD_ZIP_HOME
                },
                LastName = member.MEME_LAST_NAME,
                MedicaidId = member.MEME_MEDCD_NO,
                MiddleInitial = member.MEME_MID_INIT,
                Phone = member.SBAD_PHONE_HOME,
                SubscriberId = member.SBSB_ID,
                Suffix = member.MEME_SFX
            };
        }
    }
}
