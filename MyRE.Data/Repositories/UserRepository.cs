using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyRE.Core.Models;
using MyRE.Core.Repositories;

namespace MyRE.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyREContext _MyREContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(MyREContext MyREContext, UserManager<ApplicationUser> userManager)
        {
            _MyREContext = MyREContext;
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
