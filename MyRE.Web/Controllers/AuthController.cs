using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRE.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyRE.Core.Models.Data;
using MyRE.Core.Models.Web;
using MyRE.Core.Services;
using Newtonsoft.Json;

namespace MyRE.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public AuthController(
            IAuthService authService, IUserService userService,
            ILogger<AuthController> logger, UserManager<ApplicationUser> userManager1, SignInManager<ApplicationUser> signInManager1)
        {
            _authService = authService;
            _userService = userService;
            _logger = logger;
            _userManager = userManager1;
            _signInManager = signInManager1;
        }

        [HttpGet("Initialize/{encodedData}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> InitializeSmartThings(string encodedData)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/login?loginError='You must log in or register before you can use the web console. Log in, then authenticate again via the SmartApp.'");
            }

            byte[] data = Convert.FromBase64String(encodedData);
            var blob = JsonConvert.DeserializeObject<SmartThingsAuthBlob>(Encoding.UTF8.GetString(data));

            try
            {
                var result = await _authService.CreateInstanceAsync(blob.AccountId, blob.InstanceName,
                    _userService.GetAuthenticatedUserFromContextAsync(HttpContext).Result.UserId,
                    blob.AppId, blob.ApiServerBaseUrl, blob.AccessToken);

                return Redirect("/");
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ErrorResponse(e.Message));
            }


            //var response = await client.GetAsync($"{baseUrl}api/smartapps/installations/{installationId}/test");

            //var response = await client.GetAsync(blob.Url + "api/smartapps/endpoints");
            //var content = await response.Content.ReadAsStringAsync();

            //return Ok(blob);
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var emailLower = request.Email.ToLowerInvariant();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                _logger.LogInformation($"Log in failed for email ${request.Email}");
                return NotFound();
            }

            var signedInResult = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);
            if (signedInResult.Succeeded)
            {
                _logger.LogInformation($"Log in succeeded for email ${request.Email}");
                return Ok();
            }
            else
            {
                _logger.LogInformation($"Log in failed for email ${request.Email}");
                return NotFound();
            }
        }

        [HttpPost("Logout")]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(void), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var newUser = new ApplicationUser()
            {
                Email = request.Email,
                UserName = request.Email
            };

            var createResult = await _userManager.CreateAsync(newUser, request.Password);

            if (!createResult.Succeeded)
            {
                var message = createResult.Errors.First().Description;

                return BadRequest(new ErrorResponse(message));
            }
            return Created();
        }
    }
}