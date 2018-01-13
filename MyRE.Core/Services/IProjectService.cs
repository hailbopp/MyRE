using System.Collections.Generic;
using System.Threading.Tasks;
using MyRE.Core.Models.Domain;

namespace MyRE.Core.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetUserProjectsAsync(string userId);
    }
}
