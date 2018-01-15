using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRE.Core.Extensions;
using MyRE.Core.Models.Domain;
using MyRE.Core.Services;
using MyRE.Web.Authorization;

namespace MyRE.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Projects")]
    [Authorize]
    public class ProjectsController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IProjectService _project;
        private readonly IUserService _user;

        public ProjectsController(IProjectService project, IUserService user, IAuthorizationService authorizationService)
        {
            _project = project;
            _user = user;
            _authorizationService = authorizationService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<Project>), 200)]
        public async Task<IActionResult> ListProjectsAsync()
        {
            var currentUser = await _user.GetAuthenticatedUserFromContextAsync(HttpContext);
            var projects = await _project.GetUserProjectsAsync(currentUser.UserId);

            return Ok(projects);
        }

        [HttpGet("{projectId:Guid}")]
        public async Task<IActionResult> GetProjectAsync(Guid projectId)
        {
            var project = await _project.GetByIdAsync(projectId);

            if (project == null)
            {
                return NotFound();
            }

            var authResult = await _authorizationService.AuthorizeAsync(User, project, Operations.Read);
            if (authResult.Succeeded)
            {
                return Ok(project);
            }

            return Forbid();
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(Project), 201)]
        public async Task<IActionResult> CreateNewProject([FromBody]Project newProject)
        {
            var createdProject = await _project.CreateAsync(newProject.Name, newProject.Description, newProject.InstanceId);

            return Created(GetUriOfResource($"/api/Projects/{createdProject.ProjectId}"), createdProject.ToDomainModel());
        }
    }
}