namespace CareSource.WC.OnBase.Core.Services.Interfaces
{
    public interface IRestClient
    {
        TResponse Get<TRequest, TResponse>(string contentType,
            string endPoint,
            string action,
            string userName,
            string password,
            TRequest requestData);

        TResponse Post<TRequest, TResponse>(
            string contentType,
            string endPoint,
            string action,
            string userName,
            string password,
            TRequest requestData);
    }
}
