using DatabaseService;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Helpers;

namespace Web.Controllers
{
    public class DatabaseController : BaseController
    {
        private IDatabaseService _databaseService;

        public DatabaseController(IDatabaseService databaseService) => _databaseService = databaseService;

        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.Title = "Data Stats";

                return View(await _databaseService.GetDatabaseStatsAsync());
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }
    }
}