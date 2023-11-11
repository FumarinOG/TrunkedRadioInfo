using Kendo.Mvc.UI;
using RadioHistoryService;
using RadioService;
using SearchDataService;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemInfoService;
using TalkgroupRadioService;
using TowerRadioService;
using TowerTalkgroupRadioService;
using Web.Helpers;
using static Web.Helpers.Factory;

namespace Web.Controllers
{
    public class RadioController : BaseController
    {
        private const string GRID_LAYOUT = "_GridLayout";

        private readonly ISystemInfoService _systemInfoService;
        private readonly IRadioService _radioService;
        private readonly ITowerRadioService _towerRadioService;
        private readonly ITowerTalkgroupRadioService _towerTalkgroupRadioService;
        private readonly ITalkgroupRadioService _talkgroupRadioService;
        private readonly IRadioHistoryService _radioHistoryService;
        private readonly ISearchDataService _searchDataService;

        public RadioController(ISystemInfoService systemInfoService, IRadioService radioService, ITowerRadioService towerRadioService,
            ITalkgroupRadioService talkgroupRadioService, ITowerTalkgroupRadioService towerTalkgroupRadioService, IRadioHistoryService radioHistoryService,
            ISearchDataService searchDataService)
        {
            _systemInfoService = systemInfoService;
            _radioService = radioService;
            _towerRadioService = towerRadioService;
            _talkgroupRadioService = talkgroupRadioService;
            _towerTalkgroupRadioService = towerTalkgroupRadioService;
            _radioHistoryService = radioHistoryService;
            _searchDataService = searchDataService;
        }

        public async Task<ActionResult> Index(string systemID, int radioID)
        {
            try
            {
                ViewBag.Title = "Radio";

                return View(await _radioService.GetDetailAsync(await _systemInfoService.GetAsync(systemID), radioID,
                    _searchDataService.GetView(Request.Form[FilterDataHelper.SEARCH_DATA])));
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Talkgroups(string systemID, int radioID)
        {
            try
            {
                var model = CreateRadioModel(systemID, radioID);

                return View(nameof(Talkgroups), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Towers(string systemID, int radioID)
        {
            try
            {
                var model = CreateRadioModel(systemID, radioID);

                return View(nameof(Towers), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult History(string systemID, int radioID)
        {
            try
            {
                var model = CreateRadioModel(systemID, radioID);

                return View(nameof(History), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult TalkgroupsByTower(string systemID, int radioID)
        {
            try
            {
                var model = CreateRadioModel(systemID, radioID);

                return View(nameof(TalkgroupsByTower), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public async Task<ActionResult> GetTowers(string systemID, int radioID, string searchData, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TOWER);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (radioTowers, recordCount) = await _towerRadioService.GetTowersForRadioAsync(systemID, radioID, filterData);

                return Json(new
                {
                    Data = radioTowers,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetTalkgroups(string systemID, int radioID, string searchData, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TALKGROUP);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (radioTalkgroups, recordCount) = await _talkgroupRadioService.GetTalkgroupsForRadioAsync(systemID, radioID, filterData);

                return Json(new
                {
                    Data = radioTalkgroups,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetHistory(string systemID, int radioID, string searchData, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.RADIO);
                var filterData = filterDataHelper.ConvertRequest(request);
                var (radioHistory, recordCount) = await _radioHistoryService.GetForRadioAsync(systemID, radioID, filterData);

                return Json(new
                {
                    Data = radioHistory,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetTowerList(string systemID, int radioID, string searchData, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TOWER);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var results = await _towerRadioService.GetTowerListForRadioAsync(systemID, radioID, filterData);

                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetDateList(string systemID, int radioID, int towerNumber, string searchData, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TOWER);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var results = await _towerRadioService.GetDateListForTowerRadioAsync(systemID, radioID, towerNumber, filterData);

                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetTowerTalkgroups(string systemID, int radioID, int towerNumber, DateTime? date, string searchData,
            [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.RADIO);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (towerTalkgroups, recordCount) = await _towerTalkgroupRadioService.GetTalkgroupsForTowerRadioAsync(systemID, radioID, towerNumber,
                    date, filterData);

                return Json(new
                {
                    Data = towerTalkgroups,
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