using Kendo.Mvc.UI;
using RadioService;
using SearchDataService;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemInfoService;
using TalkgroupService;
using Web.Helpers;
using static Web.Helpers.Factory;

namespace Web.Controllers
{
    public class ToolsController : BaseController
    {
        private readonly ISystemInfoService _systemInfoService;
        private readonly IRadioService _radioService;
        private readonly ITalkgroupService _talkgroupService;
        private readonly ISearchDataService _searchDataService;

        public ToolsController(ISystemInfoService systemInfoService, IRadioService radioService, ITalkgroupService talkgroupService, ISearchDataService searchDataService)
        {
            _systemInfoService = systemInfoService;
            _radioService = radioService;
            _talkgroupService = talkgroupService;
            _searchDataService = searchDataService;
        }

        public ActionResult Index()
        {
            try
            {
                ViewBag.Title = "Tools";

                return View();
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult RadioNames()
        {
            try
            {
                ViewBag.Title = "Radio Names";

                return View();
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult UnknownTalkgroups()
        {
            try
            {
                ViewBag.Title = "Unknown Talkgroups";

                return View();
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public async Task<ActionResult> GetSystemList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                return Json(await _systemInfoService.GetListAsync(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetRadioNames(string systemID, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.RADIO);
                var filterData = filterDataHelper.ConvertRequest(request);
                var (names, recordCount) = await _radioService.GetNamesAsync(systemID, filterData);

                return Json(new
                {
                    Data = names,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetUnknownTalkgroups(string systemID, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.RADIO);
                var filterData = filterDataHelper.ConvertRequest(request);
                var (names, recordCount) = await _talkgroupService.GetViewForSystemUnknownAsync(systemID, filterData);

                return Json(new
                {
                    Data = names,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }
    }
}