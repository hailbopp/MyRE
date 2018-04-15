using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MyRE.Core.Models.Data;
using MyRE.Core.Models.Domain;
using MyRE.SmartApp.Api.Client;
using MyRE.SmartApp.Api.Client.Models;
using Optional.Unsafe;
using Project = MyRE.Core.Models.Data.Project;

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

        private async Task<IMyreSmartAppApiClient> CreateInstanceClientAsync(Guid instanceId)
        {
            var instance = await _user.GetAppInstanceByIdAsync(instanceId);
            return CreateInstanceClient(instance);
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

        private async Task<IMyreSmartAppApiClient> CreateProjectClientAsync(Project project)
        {
            if (project.ParentInstance == null && Guid.Empty != project.ParentInstanceId)
            {
                return await CreateInstanceClientAsync(project.ParentInstanceId);
            }
            else
            {
                return CreateInstanceClient(project.ParentInstance);
            }
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

        public async Task<ChildSmartApp> UpsertProjectAsync(Project project)
        {
            var client = await CreateProjectClientAsync(project);

            var existingProjectResponse = await client.GetProjectAsync(project.ProjectId.ToString());

            ApiResponse<ChildSmartApp> upsertResult;

            if (existingProjectResponse.Error.HasValue &&
                existingProjectResponse.Error.ValueOrFailure().StatusCode == HttpStatusCode.NotFound)
            {
                // The project does not exist in its instance. We need to create it.
                var createRequest = new CreateChildAppRequest()
                {
                    ProjectId = project.ProjectId.ToString(),
                    Name = project.Name,
                    Description = project.Description,
                    Source = project.Source.Source
                };

                upsertResult = await client.CreateProjectAsync(createRequest);
            }
            else
            {
                // We need to update the existing project. First, we need to stop its execution. Then we can update it and resume.
                //var stopResult = await client.HaltProjectAsync(project.ProjectId.ToString());

                var updateRequest = new UpdateChildAppRequest()
                {
                    Name = project.Name,
                    Description = project.Description,
                    Source = project.Source.Source
                };

                upsertResult = await client.UpdateProjectAsync(project.ProjectId.ToString(), updateRequest);
            }

            if (upsertResult.Error.HasValue)
            {
                throw new Exception(upsertResult.Error.ValueOrFailure().Message);
            }

            // TODO: Don't resume if the user has explicitly halted execution.
            //var resumeResult = await client.ResumeProjectAsync(project.ProjectId.ToString());

            return upsertResult.Data;
        }

        public async Task<object> ExecuteProjectAsync(Project project)
        {
            var client = await CreateProjectClientAsync(project);
            var result = await client.ExecuteProject(project.ProjectId.ToString());
            return result;
        }

        public async Task HaltProjectAsync(Project project)
        {
            var client = await CreateProjectClientAsync(project);
            var result = await client.HaltProjectAsync(project.ProjectId.ToString());
        }

        public async Task ResumeProjectAsync(Project project)
        {
            var client = await CreateProjectClientAsync(project);
            var result = await client.ResumeProjectAsync(project.ProjectId.ToString());
        }
        
        public async Task DeleteProjectAsync(Project project)
        {
            var client = await CreateProjectClientAsync(project);
            var result = client.DeleteProjectAsync(project.ProjectId.ToString());
        }
    }
}