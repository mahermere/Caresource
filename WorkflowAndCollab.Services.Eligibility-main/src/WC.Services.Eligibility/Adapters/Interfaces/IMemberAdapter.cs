namespace CareSource.WC.Services.Eligibility.Adapters.Interfaces
{
    public interface IMemberAdapter<TMemberModel>
    {
        TMemberModel GetMemberBySubscriberId(string subscriberId, string subscriberSuffix);
    }
}
