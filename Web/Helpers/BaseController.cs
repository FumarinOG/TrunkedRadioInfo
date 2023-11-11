using NLog;
using System;
using System.Web.Mvc;

namespace Web.Helpers
{
    public class BaseController : Controller
    {
        protected static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected ActionResult HandleViewError(Exception exception)
        {
            _logger.Error(exception);

            return View("Error");
        }

        protected ActionResult HandleJSONError(Exception exception)
        {
            _logger.Error(exception);
            return Json(new { Success = false, exception.Message });

        }
    }
}