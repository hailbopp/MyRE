using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using MyRE.Core.Models.Data;
using MyRE.Core.Repositories;
using MyRE.Web.Extensions;

namespace MyRE.Web.Authorization
{
    public class ProjectAuthorizationHandler: AuthorizationHandler<OperationAuthorizationRequirement, Project>
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectAuthorizationHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Project resource)
        {
            var owner = await _projectRepository.GetOwnerAsync(resource.ProjectId);

            if (Operations.AllNames.Contains(requirement.Name))
            {
                if (owner.Id == context.User.GetUserId())
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}