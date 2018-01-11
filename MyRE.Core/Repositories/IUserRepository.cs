using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyRE.Core.Models;

namespace MyRE.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string userId);
        Task<User> GetAuthenticatedUserFromContextAsync(HttpContext context);
    }
}
