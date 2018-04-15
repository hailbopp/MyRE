namespace MyRE.SmartApp.Api.Client
{
    public class MyreSmartAppApiClientFactory: IMyreSmartAppApiClientFactory
    {
        public IMyreSmartAppApiClient Create(string baseUri, string instanceId, string accessToken) => new MyreSmartAppApiClient(baseUri, instanceId, accessToken);
    }
}