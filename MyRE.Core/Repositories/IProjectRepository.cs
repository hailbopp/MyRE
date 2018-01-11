using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyRE.Core.Models.Domain;

namespace MyRE.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetUserProjectsAsync(string userId);
    }
}
