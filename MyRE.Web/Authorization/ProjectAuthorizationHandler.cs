using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using MyRE.Core.Models.Data;
using MyRE.Core.Repositories;
using MyRE.Core.Services;
using MyRE.Web.Extensions;

namespace MyRE.Web.Authorization
{
    public class ProjectAuthorizationHandler: AuthorizationHandler<OperationAuthorizationRequirement, Project>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserService _user;

        protected ProjectAuthorizationHandler(IProjectRepository projectRepository, IUserService user)
        {
            _projectRepository = projectRepository;
            _user = user;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Project resource)
        {
            var owner = await _projectRepository.GetOwnerAsync(resource.ProjectId);

            if (owner.Id == context.User.GetUserId())
            {
                context.Succeed(requirement);
            }
        }
    }
}