using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRE.Core.Services;

namespace MyRE.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Devices")]
    [Authorize]
    public class DevicesController: BaseController
    {
        private readonly ISmartAppService _smartApp;
        private readonly IUserService _user;

        public DevicesController(ISmartAppService smartApp, IUserService user)
        {
            _smartApp = smartApp;
            _user = user;
        }
        
        [HttpGet("")]
        public async Task<IActionResult> ListDevices([FromQuery] Guid? instanceId = null)
        {
            var user = await _user.GetAuthenticatedUserFromContextAsync(HttpContext);

            if (instanceId == null) return Ok(await _smartApp.ListUserDevicesAsync(user));

            var instances = await _user.GetUserInstancesAsync(user.UserId);
            var requestedInstance = instances.FirstOrDefault(i => i.AppInstanceId == instanceId);

            if (requestedInstance == null)
            {
                return Unauthorized();
            }

            return Ok(await _smartApp.ListInstanceDevicesAsync(requestedInstance));
        }
    }
}
