using System;
using System.Text.RegularExpressions;
using CareSource.WC.Core.Extensions;
using CareSource.WC.Entities.Members;
using CareSource.WC.Services.Eligibility.Adapters.Interfaces;
using CareSource.WC.Services.Eligibility.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CareSource.WC.Services.Eligibility.Managers
{
    public class MemberManager : IMemberManager<Member>
    {
        private readonly IMemberAdapter<Member> _memberAdapter;

        public MemberManager(IMemberAdapter<Member> memberAdapter)
        {
            _memberAdapter = memberAdapter;
        }

        public bool ValidateMemberId(string memberId, ModelStateDictionary modelState = null)
        {
            if (modelState == null)
            {
                modelState = new ModelStateDictionary();
            }

            if (memberId.IsNullOrWhiteSpace() || !Regex.IsMatch(memberId, "^[0-9]{11}$"))
            {
                modelState.AddModelError("MemberId", "Member Id must be in the valid format '^[0-9]{11}$'.");
            }

            return modelState.IsValid;
        }

        public Member GetMember(string memberId)
        {
            var memberIdentifiers = ParseMemberId(memberId);

            return _memberAdapter.GetMemberBySubscriberId(
                memberIdentifiers.SubscriberId
                , memberIdentifiers.SubscriberSuffix);
        }

        private (string SubscriberId, string SubscriberSuffix) ParseMemberId(string memberId)
        {
            if (!ValidateMemberId(memberId))
            {
                throw new ArgumentException($"MemberId '{memberId}' is invalid.");
            }

            var subscriberId = memberId.Substring(0, 9);
            var subscriberSuffix = memberId.Substring(9, 2);

            return (subscriberId, subscriberSuffix);
        }
    }
}
