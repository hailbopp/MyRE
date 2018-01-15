using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MyRE.Web.Authorization;

namespace MyRE.Web.Controllers
{
    public abstract class BaseController: Controller
    {
        protected StatusCodeResult StatusResult(int statusCode)
        {
            return new StatusCodeResult(statusCode);
        }

        protected StatusCodeResult Created()
        {
            return StatusResult(201);
        }

        protected StatusCodeResult ServerError(object value) {
            return StatusResult(500);
        }

        protected Uri GetUriOfResource(string path)
        {
            var requestUri = Request.GetUri();

            return new Uri(requestUri, path);
        }

        private async Task<IActionResult> PerformOperationAuthenticatedResource<TResource, TAction>(
            IAuthorizationService authService,
            Func<Task<TResource>> retrieveResource,
            Func<TResource, Task<TAction>> operation,
            OperationAuthorizationRequirement requirement
        ) where TAction: IActionResult
        {
            var entity = await retrieveResource();
            if (entity == null)
            {
                return NotFound();
            }

            var authResult = await authService.AuthorizeAsync(User, entity, requirement);

            if (authResult.Succeeded)
            {
                return await operation(entity);
            }

            if (User.Identity.IsAuthenticated)
                return Forbid();

            return Challenge();
        }

        protected Task<IActionResult> RetrieveAuthenticatedResource<TResource, TAction>(
            IAuthorizationService authService,
            Func<Task<TResource>> retrieveResource,
            Func<TResource, Task<TAction>> returnFunc
            ) where TAction: IActionResult
        {
            return PerformOperationAuthenticatedResource(authService, retrieveResource, returnFunc, Operations.Read);
        }

        protected Task<IActionResult> DeleteAuthenticatedResource<TResource>(IAuthorizationService authService,
            Func<Task<TResource>> retrieveResource,
            Func<TResource, Task> deleteResource
            )
        {
            return PerformOperationAuthenticatedResource(authService, retrieveResource, 
                async entity => {
                    await deleteResource(entity);
                    return NoContent();
                }, Operations.Delete);
        }
    }
}
