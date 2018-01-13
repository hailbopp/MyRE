namespace MyRE.SmartApp.Api.Client
{
    public class MyreSmartAppApiClientFactory: IMyreSmartAppApiClientFactory<MyreSmartAppApiClient>
    {
        public MyreSmartAppApiClient Create(string baseUri, string instanceId, string accessToken)
        {
            return new MyreSmartAppApiClient(baseUri, instanceId, accessToken);
        }
    }
}