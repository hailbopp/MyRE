using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyRE.Core.Models;

namespace MyRE.Core.Repositories
{
    public interface IAppInstanceRepository
    {
        Task<AppInstance> CreateAsync(AppInstance entity);
        Task<AppInstance> GetAppInstanceByRemoteIdAsync(string remoteAppId);
    }
}
