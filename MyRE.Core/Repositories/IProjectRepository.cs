using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyRE.Core.Models.Domain;

namespace MyRE.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetUserProjectsAsync(string userId);
        Task<Project> CreateAsync(string name, string description, Guid instanceId);
    }
}
