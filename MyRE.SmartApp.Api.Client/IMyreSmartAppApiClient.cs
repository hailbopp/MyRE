using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyRE.SmartApp.Api.Client.Models;

namespace MyRE.SmartApp.Api.Client
{
    public interface IMyreSmartAppApiClient: IDisposable
    {
        Task<ApiResponse<InstanceStatus>> GetInstanceStatusAsync();
        Task<ApiResponse<IEnumerable<DeviceInfo>>> ListDevicesAsync();
        Task<ApiResponse<DeviceState>> GetDeviceStatusAsync(string deviceId);
        Task<ApiResponse<ChildSmartApp>> CreateProjectAsync(CreateChildAppRequest request);
        Task<ApiResponse<ResultResponse>> TestProjectSourceCode(string source);
    }
}
