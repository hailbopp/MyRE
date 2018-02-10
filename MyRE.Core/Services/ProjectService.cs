using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyRE.Core.Models.Data;
using MyRE.Core.Repositories;

namespace MyRE.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectSourceMappingService _projectSourceMapping;

        public ProjectService(IProjectRepository projectRepository, IProjectSourceMappingService projectSourceMapping)
        {
            _projectRepository = projectRepository;
            _projectSourceMapping = projectSourceMapping;
        }

        public Task<IEnumerable<Project>> GetUserProjectsAsync(string userId) => _projectRepository.GetUserProjectsAsync(userId);
        public async Task<Project> GetByIdAsync(Guid projectId)
        {
            var proj = await _projectRepository.GetByIdAsync(projectId);
            return proj;
        }

        public Task<Project> CreateAsync(string name, string description, Guid instanceId) =>
            _projectRepository.CreateAsync(name, description, instanceId);

        public async Task DeleteAsync(Guid projectId)
        {
            await _projectRepository.DeleteAsync(projectId);
        }

        public async Task<Project> UpdateAsync(Project entity)
        {
            return await _projectRepository.UpdateAsync(entity);
        }

        public Task<ProjectSource> GetSource(Guid projectId)
        {
            return _projectRepository.GetSourceById(projectId);
        }
    }
}