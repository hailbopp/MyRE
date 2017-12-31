using FreeCoRE.Web.Data;
using FreeCoRE.Web.Data.Models;
using FreeCoRE.Web.Interfaces;
using FreeCoRE.Web.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCoRE.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FreeCoreContext _freecoreContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(FreeCoreContext freeCoreContext, UserManager<ApplicationUser> userManager)
        {
            _freecoreContext = freeCoreContext;
            _userManager = userManager;
        }

        public async Task<User> GetUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user.MapToDomain();
        }
        public async Task<User> GetAuthenticatedUserFromContextAsync(HttpContext context)
        {
            return (await _userManager.GetUserAsync(context.User)).MapToDomain();
        }

    }

    static class Extensions
    {
        public static User MapToDomain(this ApplicationUser self)
        {
            if (self == null) return null;
            return new User()
            {
                UserId = self.Id,
                Email = self.Email,
            };
        }
    }
}
