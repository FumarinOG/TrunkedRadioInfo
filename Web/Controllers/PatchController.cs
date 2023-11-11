using Kendo.Mvc.UI;
using PatchService;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemInfoService;
using Web.Helpers;

namespace Web.Controllers
{
    public class PatchController : BaseController
    {
        private readonly IPatchService _patchService;
        private readonly ISystemInfoService _systemInfoService;

        public PatchController(IPatchService patchService, ISystemInfoService systemInfoService)
        {
            _patchService = patchService;
            _systemInfoService = systemInfoService;
        }

        public async Task<ActionResult> Index(string systemID, int fromTalkgroupID, int toTalkgroupID)
        {
            try
            {
                ViewBag.Title = "Patch";

                return View(await _patchService.GetSummaryAsync(await _systemInfoService.GetAsync(systemID), fromTalkgroupID, toTalkgroupID));
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public async Task<ActionResult> GetPatches([DataSourceRequest] DataSourceRequest request, string systemID, int fromTalkgroupID, int toTalkgroupID)
        {
            try
            {
                var systemInfo = await _systemInfoService.GetAsync(systemID);

                return Json(new
                {
                    Data = await _patchService.GetForPatchByDateAsync(systemInfo.ID, fromTalkgroupID, toTalkgroupID, request.Page, request.PageSize),
                    Total = await _patchService.GetForPatchByDateCountAsync(systemInfo.ID, fromTalkgroupID, toTalkgroupID)
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }
    }
}