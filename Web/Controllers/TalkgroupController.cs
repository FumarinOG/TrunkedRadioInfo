using Kendo.Mvc.UI;
using PatchService;
using SearchDataService;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemInfoService;
using TalkgroupHistoryService;
using TalkgroupRadioService;
using TalkgroupService;
using TowerTalkgroupRadioService;
using TowerTalkgroupService;
using Web.Helpers;
using static Web.Helpers.Factory;

namespace Web.Controllers
{
    public class TalkgroupController : BaseController
    {
        private const string GRID_LAYOUT = "_GridLayout";

        private readonly ISystemInfoService _systemInfoService;
        private readonly ITalkgroupService _talkgroupService;
        private readonly ITowerTalkgroupService _towerTalkgroupService;
        private readonly ITowerTalkgroupRadioService _towerTalkgroupRadioService;
        private readonly IPatchService _patchService;
        private readonly ITalkgroupRadioService _talkgroupRadioService;
        private readonly ITalkgroupHistoryService _talkgroupHistoryService;
        private readonly ISearchDataService _searchDataService;

        public TalkgroupController(ISystemInfoService systemInfoService, ITalkgroupService talkgroupService, ITowerTalkgroupService towerTalkgroupService,
            ITowerTalkgroupRadioService towerTalkgroupRadioService, ITalkgroupRadioService talkgroupRadioService, IPatchService patchService,
            ITalkgroupHistoryService talkgroupHistoryService, ISearchDataService searchDataService)
        {
            _systemInfoService = systemInfoService;
            _talkgroupService = talkgroupService;
            _towerTalkgroupService = towerTalkgroupService;
            _towerTalkgroupRadioService = towerTalkgroupRadioService;
            _talkgroupRadioService = talkgroupRadioService;
            _patchService = patchService;
            _talkgroupHistoryService = talkgroupHistoryService;
            _searchDataService = searchDataService;
        }

        public async Task<ActionResult> Index(string systemID, int talkgroupID)
        {
            try
            {
                ViewBag.Title = "Talkgroup";

                return View(await _talkgroupService.GetDetailAsync(await _systemInfoService.GetAsync(systemID), talkgroupID,
                    _searchDataService.GetView(Request.Form[FilterDataHelper.SEARCH_DATA])));
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Radios(string systemID, int talkgroupID)
        {
            try
            {
                var model = CreateTalkgroupModel(systemID, talkgroupID);

                return View(nameof(Radios), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Towers(string systemID, int talkgroupID)
        {
            try
            {
                var model = CreateTalkgroupModel(systemID, talkgroupID);

                return View(nameof(Towers), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Patches(string systemID, int talkgroupID)
        {
            try
            {
                var model = CreateTalkgroupModel(systemID, talkgroupID);

                return View(nameof(Patches), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult History(string systemID, int talkgroupID)
        {
            try
            {
                var model = CreateTalkgroupModel(systemID, talkgroupID);

                return View(nameof(History), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult RadiosByTower(string systemID, int talkgroupID)
        {
            try
            {
                var model = CreateTalkgroupModel(systemID, talkgroupID);

                return View(nameof(RadiosByTower), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public async Task<ActionResult> GetRadios(string systemID, int talkgroupID, string searchData, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.RADIO);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (talkgroupRadios, recordCount) = await _talkgroupRadioService.GetRadiosForTalkgroupAsync(systemID, talkgroupID, filterData);

                return Json(new
                {
                    Data = talkgroupRadios,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetTowers(string systemID, int talkgroupID, string searchData, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TOWER);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (talkgroupTowers, recordCount) = await _towerTalkgroupService.GetTowersForTalkgroupViewAsync(systemID, talkgroupID, filterData);

                return Json(new
                {
                    Data = talkgroupTowers,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetPatches(string systemID, int talkgroupID, string searchData, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TALKGROUP_FROM, FilterDataHelper.TALKGROUP_TO);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (patches, recordCount) = await _patchService.GetForSystemTalkgroupAsync(systemID, talkgroupID, filterData);

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

        public async Task<ActionResult> GetHistory(string systemID, int talkgroupID, string searchText, string searchData,
            [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TALKGROUP);
                var filterData = filterDataHelper.ConvertRequest(request);
                var (talkgroupHistory, recordCount) = await _talkgroupHistoryService.GetForTalkgroupAsync(systemID, talkgroupID, filterData);

                return Json(new
                {
                    Data = talkgroupHistory,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetTowerList(string systemID, int talkgroupID, string searchData, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TOWER);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var results = await _towerTalkgroupService.GetTowerListForTalkgroupAsync(systemID, talkgroupID, filterData);

                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetDateList(string systemID, int talkgroupID, int towerNumber, string searchData,
            [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TOWER);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var results = await _towerTalkgroupService.GetDateListForTowerTalkgroupAsync(systemID, talkgroupID, towerNumber, filterData);

                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetTowerRadios(string systemID, int talkgroupID, int towerNumber, DateTime? date, string searchData,
            [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.RADIO);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (towerRadios, recordCount) = await _towerTalkgroupRadioService.GetRadiosForTowerTalkgroupAsync(systemID, talkgroupID, towerNumber,
                    date, filterData);

                return Json(new
                {
                    Data = towerRadios,
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