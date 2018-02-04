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
    }
}