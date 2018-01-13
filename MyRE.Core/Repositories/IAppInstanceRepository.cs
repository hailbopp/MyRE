using System.Collections.Generic;
using System.Threading.Tasks;
using MyRE.Core.Models.Data;

namespace MyRE.Core.Repositories
{
    public interface IAppInstanceRepository
    {
        Task<AppInstance> CreateAsync(AppInstance entity);
        Task<AppInstance> GetAppInstanceByRemoteIdAsync(string remoteAppId);

        Task<IEnumerable<AppInstance>> GetByUserIdAsync(string userId);
    }
}
