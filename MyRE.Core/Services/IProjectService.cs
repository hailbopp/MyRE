using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyRE.Core.Models.Data;

namespace MyRE.Core.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetUserProjectsAsync(string userId);

        Task<Project> GetByIdAsync(Guid projectId);
        Task<Project> CreateAsync(string name, string description, Guid instanceId);
        Task DeleteAsync(Guid projectId);

        Task<Project> UpdateAsync(Project entity);

        Task<ProjectSource> GetSource(Guid projectId);
    }
}
