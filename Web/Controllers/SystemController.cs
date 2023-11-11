using Kendo.Mvc.UI;
using PatchService;
using ProcessedFileService;
using RadioService;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemDetailService;
using TalkgroupService;
using TowerService;
using Web.Helpers;
using static Web.Helpers.Factory;

namespace Web.Controllers
{
    public class SystemController : BaseController
    {
        private const string GRID_LAYOUT = "_GridLayout";

        private readonly ISystemDetailService _systemDetailService;
        private readonly ITalkgroupService _talkgroupService;
        private readonly IRadioService _radioService;
        private readonly ITowerService _towerService;
        private readonly IPatchService _patchService;
        private readonly IProcessedFileService _processedFileService;

        public SystemController(ISystemDetailService systemDetailService, ITalkgroupService talkgroupService, IRadioService radioService,
            ITowerService towerService, IPatchService patchService, IProcessedFileService processedFileService)
        {
            _systemDetailService = systemDetailService;
            _talkgroupService = talkgroupService;
            _radioService = radioService;
            _towerService = towerService;
            _patchService = patchService;
            _processedFileService = processedFileService;
        }

        public async Task<ActionResult> Index(string systemID)
        {
            try
            {
                var systemInfo = await _systemDetailService.GetAsync(systemID);

                ViewBag.Title = "System Info";
                return View(systemInfo);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Talkgroups(string systemID)
        {
            try
            {
                var model = CreateSystemModel(systemID);

                return View(nameof(Talkgroups), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Radios(string systemID)
        {
            try
            {
                var model = CreateSystemModel(systemID);

                return View(nameof(Radios), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Towers(string systemID)
        {
            try
            {
                var model = CreateSystemModel(systemID);

                return View(nameof(Towers), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Patches(string systemID)
        {
            try
            {
                var model = CreateSystemModel(systemID);

                return View(nameof(Patches), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult ProcessedFiles(string systemID)
        {
            try
            {
                var model = CreateSystemModel(systemID);

                return View(nameof(ProcessedFiles), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        [ValidateInput(false)]
        public async Task<ActionResult> GetTalkgroups(string systemID, bool activeOnly, DateTime? dateFrom, DateTime? dateTo,
            [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TALKGROUP);
                var filterData = filterDataHelper.ConvertRequest(request, dateFrom, dateTo);
                var (talkgroups, recordCount) = await _talkgroupService.GetViewForSystemAsync(systemID, activeOnly, filterData);

                return Json(new
                {
                    Data = talkgroups,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        [ValidateInput(false)]
        public async Task<ActionResult> GetRadios(string systemID, bool activeOnly, DateTime? dateFrom, DateTime? dateTo,
            [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.RADIO);
                var filterData = filterDataHelper.ConvertRequest(request, dateFrom, dateTo);
                var (radios, recordCount) = await _radioService.GetViewForSystemAsync(systemID, activeOnly, filterData);

                return Json(new
                {
                    Data = radios,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetTowers(string systemID, bool activeOnly, DateTime? dateFrom, DateTime? dateTo,
            [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TOWER);
                var filterData = filterDataHelper.ConvertRequest(request, dateFrom, dateTo);
                var (towers, recordCount) = await _towerService.GetViewForSystemAsync(systemID, activeOnly, filterData);

                return Json(new
                {
                    Data = towers,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetPatches(string systemID, DateTime? dateFrom, DateTime? dateTo, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TALKGROUP_FROM, FilterDataHelper.TALKGROUP_TO);
                var filterData = filterDataHelper.ConvertRequest(request, dateFrom, dateTo);
                var (patches, recordCount) = await _patchService.GetSummaryForSystemAsync(systemID, filterData);

                return Json(new
                {
                    Data = patches,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetProcessedFiles(string systemID, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.FILE);
                var filterData = filterDataHelper.ConvertRequest(request);
                var (processedFiles, recordCount) = await _processedFileService.GetViewForSystemAsync(systemID, filterData);

                return Json(new
                {
                    Data = processedFiles,
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