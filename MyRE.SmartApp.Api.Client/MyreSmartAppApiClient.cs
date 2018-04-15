using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            public static readonly Func<string, string> DeviceId = (deviceId) => $"{Devices}/{deviceId}";

            public const string Projects = "projects";
            public static readonly Func<string, string> IndividualProject = (projectId) => $"{Projects}/{projectId}";
            public static readonly Func<string, string> HaltProject = (projectId) => $"{Projects}/{projectId}/stop";
            public static readonly Func<string, string> ResumeProject = (projectId) => $"{Projects}/{projectId}/resume";
            public static readonly Func<string, string> ExecuteProject = (projectId) => $"{Projects}/{projectId}/run";

        }

        private const string BASE_ENDPOINT_PATH = "api/smartapps/installations/";

        private readonly HttpClient Client;

        public MyreSmartAppApiClient(string baseUrl, string instanceId, string accessToken)
        {
            Client = new HttpClient {BaseAddress = new Uri(new Uri(baseUrl), $"{BASE_ENDPOINT_PATH}{instanceId}/")};
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }

        public void Dispose()
        {
            Client.Dispose();
        }

        private async Task<ApiResponse<T>> HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<T>()
                {
                    //Raw = await response.Content.ReadAsStringAsync(),
                    Data = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()),
                    Headers = response.Headers
                };
            }
            return new ApiResponse<T>()
            {
                Raw = await response.Content.ReadAsStringAsync(),
                Headers = response.Headers,
                Error = Option.Some(new ApiError()
                {
                    Message = await response.Content.ReadAsStringAsync(),
                    StatusCode = response.StatusCode,
                })
            };
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string path)
        {
            var response = await Client.GetAsync(path);
            return await HandleResponse<T>(response);
        }

        public async Task<ApiResponse<TResponse>> PostAsync<TResponse, TRequest>(string path, TRequest body)
        {
            var response = await Client.PostAsync(path, new StringContent(JsonConvert.SerializeObject(body)));
            return await HandleResponse<TResponse>(response);
        }

        public Task<ApiResponse<TResponse>> PostAsync<TResponse>(string path)
        {
            return PostAsync<TResponse, object>(path, null);
        }

        public async Task<ApiResponse<TResponse>> PutAsync<TResponse, TRequest>(string path, TRequest body)
        {
            var response = await Client.PutAsync(path, new StringContent(JsonConvert.SerializeObject(body)));
            return await HandleResponse<TResponse>(response);
        }

        public async Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string path)
        {
            var response = await Client.DeleteAsync(path);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<ApiResponse<InstanceStatus>> GetInstanceStatusAsync() => await GetAsync<InstanceStatus>(Routes.InstanceStatus);
        public async Task<ApiResponse<IEnumerable<DeviceInfo>>> ListDevicesAsync() => await GetAsync<IEnumerable<DeviceInfo>>(Routes.Devices);
        public async Task<ApiResponse<DeviceState>> GetDeviceStatusAsync(string deviceId) => await GetAsync<DeviceState>(Routes.DeviceId(deviceId));

        public async Task<ApiResponse<ChildSmartApp>> GetProjectAsync(string projectId) => await GetAsync<ChildSmartApp>(Routes.IndividualProject(projectId));
        public async Task<ApiResponse<ChildSmartApp>> CreateProjectAsync(CreateChildAppRequest request) => 
            await PostAsync<ChildSmartApp, CreateChildAppRequest>(Routes.Projects, request);

        public async Task<ApiResponse<ResultResponse<string>>> HaltProjectAsync(string projectId) => await PostAsync<ResultResponse<string>>(Routes.HaltProject(projectId));

        public async Task<ApiResponse<ResultResponse<string>>> ResumeProjectAsync(string projectId) => await PostAsync<ResultResponse<string>>(Routes.ResumeProject(projectId));

        public async Task<ApiResponse<ResultResponse<object>>> ExecuteProject(string projectId) => await PostAsync<ResultResponse<object>>(Routes.ExecuteProject(projectId));

        public async Task<ApiResponse<ChildSmartApp>> UpdateProjectAsync(string projectId, UpdateChildAppRequest request) =>
            await PutAsync<ChildSmartApp, UpdateChildAppRequest>(Routes.IndividualProject(projectId), request);

        public Task<ApiResponse<object>> DeleteProjectAsync(string projectId) => DeleteAsync<object>(Routes.IndividualProject(projectId));
    }
}