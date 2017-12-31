using FreeCoRE.Web.Interfaces;
using FreeCoRE.Web.Models.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCoRE.Web.Services
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
