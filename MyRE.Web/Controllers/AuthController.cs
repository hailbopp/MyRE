using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyRE.Web.Data;
using MyRE.Web.Data.Models;
using MyRE.Web.Models.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MyRE.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : BaseController
    {
        private readonly MyREContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AuthController(MyREContext MyREContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AuthController> logger)
        {
            _dbContext = MyREContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        
        [HttpGet("Initialize/{encodedData}")]
        public async Task<IActionResult> InitializeSmartThings(string encodedData)
        {
            if(!HttpContext.User.Identity.IsAuthenticated)
            {   
                return Redirect("/login?loginError='You must log in or register before you can use the web console. Log in, then authenticate again via the SmartApp.'");
            }

            byte[] data = Convert.FromBase64String(encodedData);
            var blob = JsonConvert.DeserializeObject<SmartThingsAuthBlob>(Encoding.UTF8.GetString(data));
            
            
            //var response = await client.GetAsync($"{baseUrl}api/smartapps/installations/{installationId}/test");
            
            //var response = await client.GetAsync(blob.Url + "api/smartapps/endpoints");
            //var content = await response.Content.ReadAsStringAsync();

            return Ok(blob);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var emailLower = request.Email.ToLowerInvariant();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if(user == null)
            {
                _logger.LogInformation($"Log in failed for email ${request.Email}");
                return NotFound();
            }

            var signedInResult = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);
            if(signedInResult.Succeeded)
            {
                _logger.LogInformation($"Log in succeeded for email ${request.Email}");
                return Ok();
            } else
            {
                _logger.LogInformation($"Log in failed for email ${request.Email}");
                return NotFound();
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var newUser = new ApplicationUser()
            {
                Email = request.Email,
                UserName = request.Email
            };

            var createResult = await _userManager.CreateAsync(newUser, request.Password);

            if(!createResult.Succeeded)
            {
                var message = createResult.Errors.First().Description;

                return BadRequest(new ErrorResponse()
                {
                    Message = message
                });
            }
            return Created();
        }
    }
}