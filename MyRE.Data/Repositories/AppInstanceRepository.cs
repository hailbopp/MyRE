using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyRE.Core.Models;
using MyRE.Core.Repositories;

namespace MyRE.Data.Repositories
{
    public class AppInstanceRepository : IAppInstanceRepository
    {
        private readonly MyREContext _myreContext;

        public AppInstanceRepository(MyREContext myreContext)
        {
            _myreContext = myreContext;
        }

        public async Task<AppInstance> CreateAsync(AppInstance entity)
        {
            var result = await _myreContext.AppInstances.AddAsync(entity);
            var saveResult = await _myreContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<AppInstance> GetAppInstanceByRemoteIdAsync(string remoteAppId)
        {
            var result = await _myreContext.AppInstances.FirstOrDefaultAsync(i => i.RemoteAppId == remoteAppId);

            return result;
        }
    }
    
}
