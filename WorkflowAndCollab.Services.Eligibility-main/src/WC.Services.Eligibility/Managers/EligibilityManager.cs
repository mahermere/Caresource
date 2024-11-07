using System;
using System.Collections.Generic;
using CareSource.WC.Entities.Members;
using CareSource.WC.Services.Eligibility.Managers.Interfaces;

namespace CareSource.WC.Services.Eligibility.Managers
{
    using CareSource.WC.Entities.Eligibility;
    using CareSource.WC.Services.Eligibility.Adapters.Interfaces;

    public class EligibilityManager : IEligibilityManager<Eligibility>
	{
        private readonly IMemberManager<Member> _memberManager;
        private readonly IEligibilityAdapter<Eligibility> _eligibilityAdapter;

        public EligibilityManager(IMemberManager<Member> memberManager
            , IEligibilityAdapter<Eligibility> eligibilityAdapter)
        {
            _memberManager = memberManager;
            _eligibilityAdapter = eligibilityAdapter;
        }

        public IEnumerable<Eligibility> GetEligibilities(string memberId)
        {
            var member = _memberManager.GetMember(memberId);

            if (member?.ContrivedKey == null)
            {
                throw new ArgumentException($"Failed to retrieve member for MemberId '{memberId}'.");
            }

            return _eligibilityAdapter.GetEligibility(member.ContrivedKey);
        }
	}
}