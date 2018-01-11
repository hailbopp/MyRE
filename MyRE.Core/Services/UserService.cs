using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyRE.Core.Models;
using MyRE.Core.Repositories;

namespace MyRE.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetAuthenticatedUserFromContextAsync(HttpContext context)
        {
            return await _userRepository.GetAuthenticatedUserFromContextAsync(context);
        }

        public async Task<User> GetUserAsync(string userId)
        {
            return await _userRepository.GetUserAsync(userId);
        }
    }
}
