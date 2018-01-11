using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyRE.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Projects")]
    public class ProjectsController : Controller
    {
        public ProjectsController()
        {
        }

        [HttpGet("/")]
        public async Task<IActionResult> ListProjectsAsync()
        {

        }
    }
}