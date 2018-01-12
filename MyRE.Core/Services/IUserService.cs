﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyRE.Core.Models;
using MyRE.Core.Models.Domain;

namespace MyRE.Core.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string userId);
        Task<User> GetAuthenticatedUserFromContextAsync(HttpContext context);

        Task<bool> UserCanAccessUserDataAsync(User accessingUser, string userId);

        Task<IEnumerable<Instance>> GetUserInstancesAsync(string userId);
    }
}
