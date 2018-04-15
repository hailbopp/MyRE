using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRE.Core.Extensions;
using MyRE.Core.Models.Data;
using MyRE.Core.Models.Web;
using MyRE.Core.Repositories;
using MyRE.Core.Services;
using MyRE.Web.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project = MyRE.Core.Models.Domain.Project;

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
        private readonly IProjectLogRepository _projectLog;

        private readonly IProjectMappingService _projectMappingService;

        public ProjectsController(IProjectService project, IUserService user, IAuthorizationService authorizationService, IProjectMappingService projectMappingService, IProjectLogRepository projectLog)
        {
            _project = project;
            _user = user;
            _authorizationService = authorizationService;
            _projectMappingService = projectMappingService;
            _projectLog = projectLog;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<Project>), 200)]
        public async Task<IEnumerable<Project>> ListProjectsAsync()
        {
            var currentUser = await _user.GetAuthenticatedUserFromContextAsync(HttpContext);
            var projects = await _project.GetUserProjectsAsync(currentUser.UserId);

            return projects.Select(_projectMappingService.ToDomain);
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(Project), 201)]
        public async Task<IActionResult> CreateNewProject([FromBody]Project newProject)
        {
            var createdProject = await _project.CreateAsync(newProject.Name, newProject.Description, newProject.InstanceId);

            return Created(GetUriOfResource($"/api/Projects/{createdProject.ProjectId}"), _projectMappingService.ToDomain(createdProject));
        }

        [HttpGet("{projectId:Guid}")]
        [ProducesResponseType(typeof(Project), 200)]
        public async Task<IActionResult> GetProjectAsync(Guid projectId)
        {
            return await RetrieveAuthenticatedResource(
                _authorizationService, 
                () => _project.GetByIdAsync(projectId),
                project => Task.FromResult(Ok(_projectMappingService.ToDomain(project))));
        }

        [HttpPut("{projectId:Guid}")]
        public async Task<IActionResult> UpdateProjectAsync([FromRoute] Guid projectId, [FromBody] JToken rawBody)
        {
            var body = rawBody.ToObject<Project>();

            var localPersistResult = await _project.UpdateAsync(_projectMappingService.ToData((Project) body));

            return Ok(_projectMappingService.ToDomain(localPersistResult));
        }

        [HttpPost("{projectId:Guid}/execute")]
        public async Task<IActionResult> ExecuteProjectAsync([FromRoute] Guid projectId)
        {
            var proj = await _project.GetByIdAsync(projectId);
            var result = await _project.ExecuteAsync(proj);

            return Ok(result);
        }

        [HttpDelete("{projectId:Guid}")]
        [ProducesResponseType(typeof(void), 204)]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            return await DeleteAuthenticatedResource(_authorizationService, () => _project.GetByIdAsync(projectId),
                project => _project.DeleteAsync(projectId));
        }

        [HttpPost("{projectId:Guid}/logs")]
        [AllowAnonymous]
        public async Task<IActionResult> RecordProjectLog([FromRoute] Guid projectId, [FromBody] Dictionary<string, object> rawBody)
        {
            var pLog = new ProjectLog()
            {
                ProjectId = projectId,
                Timestamp = DateTimeOffset.UtcNow,
                LogData = rawBody,
            };

            var result = await _projectLog.AddAsync(pLog);
            return Ok(result.ValueOr(pLog));
        }
    }
}