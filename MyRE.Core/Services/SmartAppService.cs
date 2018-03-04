using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyRE.Core.Models.Data;
using MyRE.Core.Models.Domain;
using MyRE.SmartApp.Api.Client;
using MyRE.SmartApp.Api.Client.Models;

namespace MyRE.Core.Services
{
    public class SmartAppService : ISmartAppService
    {
        private readonly IMyreSmartAppApiClientFactory _smartappClientFactory;
        private readonly IUserService _user;

        public SmartAppService(IMyreSmartAppApiClientFactory smartappClientFactory, IUserService user)
        {
            _smartappClientFactory = smartappClientFactory;
            _user = user;
        }

        private IMyreSmartAppApiClient CreateInstanceClient(AppInstance instance)
        {
            var hasEmptyPrereqs = new[]
            {
                instance?.InstanceServerBaseUri,
                instance?.RemoteAppId,
                instance?.AccessToken,
            }.Any(string.IsNullOrWhiteSpace);

            if (instance == null || hasEmptyPrereqs)
            {
                throw new ArgumentException("Provided instance is not valid.", nameof(instance));
            }

            return _smartappClientFactory.Create(instance.InstanceServerBaseUri, instance.RemoteAppId, instance.AccessToken);
        }

        public async Task<IEnumerable<DeviceInfo>> ListInstanceDevicesAsync(AppInstance instance) => (await CreateInstanceClient(instance).ListDevicesAsync()).Data;
        public async Task<IEnumerable<DeviceInfo>> ListUserDevicesAsync(User user)
        {
            var instances = await _user.GetUserInstancesAsync(user.UserId);
            return (await Task.WhenAll(
                    instances
                        .Select(async i => await ListInstanceDevicesAsync(i))))
                .SelectMany(r => r);
        }

        public async Task<DeviceState> GetDeviceState(AppInstance instance, Guid remoteDeviceId)
        {
            var result = await CreateInstanceClient(instance).GetDeviceStatusAsync(remoteDeviceId.ToString());

            if (result.Error.HasValue)
            {
                return null;
            }
            else
            {
                return result.Data;
            }
        }

        public async Task<DeviceState> GetDeviceState(User user, Guid remoteDeviceId)
        {
            var instances = await _user.GetUserInstancesAsync(user.UserId);

            var result = await Task.WhenAll(
                instances
                    .Select(async i => await GetDeviceState(i, remoteDeviceId)));

            return result.FirstOrDefault(s => s != null);
        }

        public async Task<ResultResponse> TestSourceAsync(AppInstance instance, string source)
        {
            var client = CreateInstanceClient(instance);
            var result = await client.TestProjectSourceCode(source);

            if (result.Error.HasValue)
            {
                return null;
            }

            return result.Data;
        }
    }
}