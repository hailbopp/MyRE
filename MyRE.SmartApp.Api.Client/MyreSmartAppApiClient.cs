using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MyRE.SmartApp.Api.Client.Models;
using Newtonsoft.Json;
using Optional;

namespace MyRE.SmartApp.Api.Client
{
    public class MyreSmartAppApiClient : IMyreSmartAppApiClient
    {
        private static class Routes
        {
            public const string InstanceStatus = "status";
            public const string Devices = "devices";
            public static readonly Func<string, string> DeviceId = (deviceId) => $"devices/{deviceId}";
        }

        private const string BASE_ENDPOINT_PATH = "api/smartapps/installations/";

        private readonly HttpClient Client;

        public MyreSmartAppApiClient(string baseUrl, string instanceId, string accessToken)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(new Uri(baseUrl), $"{BASE_ENDPOINT_PATH}{instanceId}/");
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }

        public void Dispose()
        {
            Client.Dispose();
        }

        private async Task<ApiResponse<T>> GetAsync<T>(string path)
        {
            var response = await Client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<T>()
                {
                    //Raw = await response.Content.ReadAsStringAsync(),
                    Data = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())
                };
            }
            return new ApiResponse<T>()
            {
                Raw = await response.Content.ReadAsStringAsync(),
                Error = Option.Some(new ApiError()
                {
                    Message = await response.Content.ReadAsStringAsync(),
                    StatusCode = response.StatusCode,
                })
            };
        }

        public async Task<ApiResponse<InstanceStatus>> GetInstanceStatusAsync() => await GetAsync<InstanceStatus>(Routes.InstanceStatus);

        public async Task<ApiResponse<IEnumerable<DeviceInfo>>> ListDevicesAsync() => await GetAsync<IEnumerable<DeviceInfo>>(Routes.Devices);
        public async Task<ApiResponse<DeviceState>> GetDeviceStatusAsync(string deviceId) => await GetAsync<DeviceState>(Routes.DeviceId(deviceId));
    }
}