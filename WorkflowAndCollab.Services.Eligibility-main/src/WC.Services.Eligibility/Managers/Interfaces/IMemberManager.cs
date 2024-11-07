using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CareSource.WC.Services.Eligibility.Managers.Interfaces
{
    public interface IMemberManager<out TMemberModel>
    {
        bool ValidateMemberId(string memberId, ModelStateDictionary modelState = null);

        TMemberModel GetMember(string memberId);
    }
}
