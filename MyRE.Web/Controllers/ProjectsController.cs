using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRE.Core.Models.Domain;
using MyRE.Core.Services;

namespace MyRE.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Projects")]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _project;
        private readonly IUserService _user;

        public ProjectsController(IProjectService project, IUserService user)
        {
            _project = project;
            _user = user;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<Project>), 200)]
        public async Task<IActionResult> ListProjectsAsync()
        {
            var currentUser = await _user.GetAuthenticatedUserFromContextAsync(HttpContext);
            var projects = await _project.GetUserProjectsAsync(currentUser.UserId);

            return Ok(projects);
        }
    }
}