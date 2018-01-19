using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyRE.Core.Models.Data;

namespace MyRE.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetUserProjectsAsync(string userId);
        Task<Project> CreateAsync(string name, string description, Guid instanceId);

        Task<Project> GetByIdAsync(Guid projectId);
        Task<ApplicationUser> GetOwnerAsync(Guid projectId);
        Task DeleteAsync(Guid projectId);

        Task<IEnumerable<Routine>> GetRoutines(Guid projectId);
    }
}
