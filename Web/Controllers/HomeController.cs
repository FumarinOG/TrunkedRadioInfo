using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemInfoService;
using Web.Helpers;
using static Web.Helpers.Factory;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISystemInfoService _systemInfoService;

        public HomeController(ISystemInfoService systemInfoService) => _systemInfoService = systemInfoService;

        public ActionResult Index()
        {
            try
            {
                ViewBag.Title = "Home";

                return View();
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public async Task<ActionResult> GetSystems([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.SYSTEM);
                var filterData = filterDataHelper.ConvertRequest(request);

                return Json((await _systemInfoService.GetListAsync(filterData)).ToDataSourceResult(request));
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }
    }
}