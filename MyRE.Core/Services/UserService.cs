using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyRE.Core.Extensions;
using MyRE.Core.Models;
using MyRE.Core.Models.Domain;
using MyRE.Core.Repositories;

namespace MyRE.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAppInstanceRepository _instanceRepository;

        public UserService(IUserRepository userRepository, IAppInstanceRepository instanceRepository)
        {
            _userRepository = userRepository;
            _instanceRepository = instanceRepository;
        }

        public async Task<User> GetAuthenticatedUserFromContextAsync(HttpContext context) => await _userRepository.GetAuthenticatedUserFromContextAsync(context);

        public async Task<bool> UserCanAccessUserDataAsync(User accessingUser, string userId) => await Task.Run(() => string.Equals(accessingUser.UserId, userId, StringComparison.CurrentCultureIgnoreCase));

        public async Task<User> GetUserAsync(string userId) => await _userRepository.GetUserAsync(userId);

        public async Task<IEnumerable<Instance>> GetUserInstancesAsync(string userId) => (await _instanceRepository.GetByUserIdAsync(userId)).Select(i => i.ToDomainModel());
    }
}
