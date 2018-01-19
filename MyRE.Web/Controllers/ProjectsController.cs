using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly IDomainModelMappingService<Core.Models.Data.Routine, Routine> _routineMappingService;

        public ProjectsController(IProjectService project, IUserService user, IAuthorizationService authorizationService, 
            IDomainModelMappingService<Core.Models.Data.Routine, Routine> routineMappingService)
        {
            _project = project;
            _user = user;
            _authorizationService = authorizationService;
            _routineMappingService = routineMappingService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<Project>), 200)]
        public async Task<IEnumerable<Project>> ListProjectsAsync()
        {
            var currentUser = await _user.GetAuthenticatedUserFromContextAsync(HttpContext);
            var projects = await _project.GetUserProjectsAsync(currentUser.UserId);

            return projects.Select(p => p.ToDomainModel());
        }

        [HttpGet("{projectId:Guid}")]
        [ProducesResponseType(typeof(Project), 200)]
        public async Task<IActionResult> GetProjectAsync(Guid projectId)
        {
            return await RetrieveAuthenticatedResource(
                _authorizationService, 
                () => _project.GetByIdAsync(projectId),
                project => Task.FromResult(Ok(project.ToDomainModel())));
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(Project), 201)]
        public async Task<IActionResult> CreateNewProject([FromBody]Project newProject)
        {
            var createdProject = await _project.CreateAsync(newProject.Name, newProject.Description, newProject.InstanceId);

            return Created(GetUriOfResource($"/api/Projects/{createdProject.ProjectId}"), createdProject.ToDomainModel());
        }

        [HttpDelete("{projectId:Guid}")]
        [ProducesResponseType(typeof(void), 204)]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            return await DeleteAuthenticatedResource(_authorizationService, () => _project.GetByIdAsync(projectId),
                project => _project.DeleteAsync(projectId));
        }

        [HttpGet("{projectId:Guid}/Routines")]
        public async Task<IActionResult> ListProjectRoutines(Guid projectId)
        {
            return await RetrieveAuthenticatedResource(
                _authorizationService,
                () => _project.GetByIdAsync(projectId),
                async project => Ok((await _project.GetRoutines(projectId)).Select(_routineMappingService.ToDomainModel)));
        }
    }
}