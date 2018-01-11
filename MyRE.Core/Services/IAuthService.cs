using System.Threading.Tasks;
using MyRE.Core.Models;

namespace MyRE.Core.Services
{
    public interface IAuthService
    {
        Task<AppInstance> CreateInstanceAsync(string accountId, string userId, string appId, string serverBaseUri, string accessToken);
    }
}
