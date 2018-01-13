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
        public static class Routes
        {
            public static string InstanceStatus = "status";
            public static string Devices = "devices";
        }

        private const string BASE_ENDPOINT_PATH = "api/smartapps/installations/";

        public readonly HttpClient Client;

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
        
        public async Task<ApiResponse<InstanceStatus>> GetInstanceStatusAsync()
        {
            var response = await Client.GetAsync(Routes.InstanceStatus);

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<InstanceStatus>()
                {
                    Data = JsonConvert.DeserializeObject<InstanceStatus>(await response.Content.ReadAsStringAsync()),
                };
            }

            return new ApiResponse<InstanceStatus>()
            {
                Error = Option.Some(new ApiError()
                {
                    Message = await response.Content.ReadAsStringAsync(),
                    StatusCode = response.StatusCode,
                }),
            };
        }

        public async Task<ApiResponse<IEnumerable<DeviceInfo>>> ListDevicesAsync()
        {
            var response = await Client.GetAsync(Routes.Devices);

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<IEnumerable<DeviceInfo>>()
                {
                    Data = JsonConvert.DeserializeObject<IEnumerable<DeviceInfo>>(await response.Content.ReadAsStringAsync())
                };
            }
            return new ApiResponse<IEnumerable<DeviceInfo>>()
            {
                Error = Option.Some(new ApiError()
                {
                    Message = await response.Content.ReadAsStringAsync(),
                    StatusCode = response.StatusCode,
                })
            };
        }
    }
}