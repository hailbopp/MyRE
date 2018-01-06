using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyRE.Web.Interfaces;
using MyRE.Web.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetLoggedInUser()
        {
            var user = await _user.GetAuthenticatedUserFromContextAsync(this.HttpContext);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}