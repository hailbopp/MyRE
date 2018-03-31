using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyRE.SmartApp.Api.Client.Models;

namespace MyRE.SmartApp.Api.Client
{
    public interface IMyreSmartAppApiClient: IDisposable
    {
        Task<ApiResponse<T>> GetAsync<T>(string path);
        Task<ApiResponse<TResponse>> PostAsync<TResponse>(string path);
        Task<ApiResponse<TResponse>> PostAsync<TResponse, TRequest>(string path, TRequest body);
        Task<ApiResponse<TResponse>> PutAsync<TResponse, TRequest>(string path, TRequest body);
        Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string path);

        Task<ApiResponse<InstanceStatus>> GetInstanceStatusAsync();

        Task<ApiResponse<IEnumerable<DeviceInfo>>> ListDevicesAsync();
        Task<ApiResponse<DeviceState>> GetDeviceStatusAsync(string deviceId);

        Task<ApiResponse<ChildSmartApp>> GetProjectAsync(string projectId);
        Task<ApiResponse<ChildSmartApp>> CreateProjectAsync(CreateChildAppRequest request);
        Task<ApiResponse<ResultResponse>> ExecuteProject(string projectId);

        Task<ApiResponse<ChildSmartApp>> UpdateProjectAsync(string projectId, UpdateChildAppRequest request);
        Task<ApiResponse<object>> DeleteProjectAsync(string projectId);
    }
}
