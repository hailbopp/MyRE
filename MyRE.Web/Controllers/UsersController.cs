using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyRE.Core.Models;
using MyRE.Core.Services;

namespace MyRE.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : BaseController
    {
        private readonly IUserService _user;

        public UsersController(IUserService userService)
        {
            _user = userService;
        }

        [HttpGet("Me")]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(User), 200)]
        public async Task<IActionResult> GetLoggedInUser()
        {
            var user = await _user.GetAuthenticatedUserFromContextAsync(HttpContext);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}