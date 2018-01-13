namespace MyRE.SmartApp.Api.Client
{
    public interface IMyreSmartAppApiClientFactory<T> where T: IMyreSmartAppApiClient
    {
        T Create(string baseUri, string instanceId, string accessToken);
    }
}