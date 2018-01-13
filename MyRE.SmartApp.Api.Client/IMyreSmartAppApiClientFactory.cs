namespace MyRE.SmartApp.Api.Client
{
    public interface IMyreSmartAppApiClientFactory
    {
        IMyreSmartAppApiClient Create(string baseUri, string instanceId, string accessToken);
    }
}