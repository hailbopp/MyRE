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

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public Task<IEnumerable<Project>> GetUserProjectsAsync(string userId) => _projectRepository.GetUserProjectsAsync(userId);
        public Task<Project> GetByIdAsync(Guid projectId)
        {
            return _projectRepository.GetByIdAsync(projectId);
        }

        public Task<Project> CreateAsync(string name, string description, Guid instanceId) =>
            _projectRepository.CreateAsync(name, description, instanceId);

        public async Task DeleteAsync(Guid projectId)
        {
            await _projectRepository.DeleteAsync(projectId);
        }
    }
}