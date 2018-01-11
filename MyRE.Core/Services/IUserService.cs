using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyRE.Core.Models;

namespace MyRE.Core.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string userId);
        Task<User> GetAuthenticatedUserFromContextAsync(HttpContext context);
    }
}
