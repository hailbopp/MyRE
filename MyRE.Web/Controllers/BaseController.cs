using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRE.Web.Controllers
{
    public class BaseController: Controller
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
    }
}
