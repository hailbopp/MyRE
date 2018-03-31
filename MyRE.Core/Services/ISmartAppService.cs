using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyRE.Core.Models.Data;
using MyRE.Core.Models.Domain;
using MyRE.SmartApp.Api.Client.Models;

namespace MyRE.Core.Services
{
    public interface ISmartAppService
    {
        Task<IEnumerable<DeviceInfo>> ListInstanceDevicesAsync(AppInstance instance);
        Task<IEnumerable<DeviceInfo>> ListUserDevicesAsync(User user);

        Task<DeviceState> GetDeviceState(AppInstance instance, Guid remoteDeviceId);
        Task<DeviceState> GetDeviceState(User user, Guid remoteDeviceId);

        Task<ApiResponse<ResultResponse>> TestSourceAsync(AppInstance instance, string source);
    }
}