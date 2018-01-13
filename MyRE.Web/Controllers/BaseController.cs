using Microsoft.AspNetCore.Mvc;

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
    }
}
