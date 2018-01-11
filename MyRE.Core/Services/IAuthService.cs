using System.Threading.Tasks;
using MyRE.Core.Models;
using MyRE.Core.Models.Data;

namespace MyRE.Core.Services
{
    public interface IAuthService
    {
        Task<AppInstance> CreateInstanceAsync(string accountId, string userId, string appId, string serverBaseUri, string accessToken);
    }
}
