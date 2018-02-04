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
    }
}