using Kendo.Mvc.UI;
using PatchService;
using SearchDataService;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemInfoService;
using TowerFrequencyService;
using TowerNeighborService;
using TowerRadioService;
using TowerService;
using TowerTalkgroupService;
using Web.Helpers;
using static Web.Helpers.Factory;

namespace Web.Controllers
{
    public class TowerController : BaseController
    {
        private const string GRID_LAYOUT = "_GridLayout";

        private readonly ISystemInfoService _systemInfoService;
        private readonly ITowerService _towerService;
        private readonly ITowerFrequencyService _towerFrequencyService;
        private readonly ITowerNeighborService _towerNeighborService;
        private readonly ITowerTalkgroupService _towerTalkgroupService;
        private readonly ITowerRadioService _towerRadioService;
        private readonly IPatchService _patchService;
        private readonly ISearchDataService _searchDataService;

        public TowerController(ISystemInfoService systemInfoService, ITowerService towerService, ITowerFrequencyService towerFrequencyService,
            ITowerNeighborService towerNeighborService, ITowerTalkgroupService towerTalkgroupService, ITowerRadioService towerRadioService,
            IPatchService patchService, ISearchDataService searchDataService)
        {
            _systemInfoService = systemInfoService;
            _towerService = towerService;
            _towerFrequencyService = towerFrequencyService;
            _towerNeighborService = towerNeighborService;
            _towerTalkgroupService = towerTalkgroupService;
            _towerRadioService = towerRadioService;
            _patchService = patchService;
            _searchDataService = searchDataService;
        }

        public async Task<ActionResult> Index(string systemID, int towerNumber)
        {
            try
            {
                ViewBag.Title = "Tower";

                return View(await _towerService.GetDetailAsync(await _systemInfoService.GetAsync(systemID), towerNumber,
                    _searchDataService.GetView(Request.Form[FilterDataHelper.SEARCH_DATA])));
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Neighbors(string systemID, int towerNumber)
        {
            try
            {
                var model = CreateTowerModel(systemID, towerNumber);

                return View(nameof(Neighbors), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Frequencies(string systemID, int towerNumber)
        {
            try
            {
                var model = CreateTowerModel(systemID, towerNumber);

                return View(nameof(Frequencies), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Talkgroups(string systemID, int towerNumber)
        {
            try
            {
                var model = CreateTowerModel(systemID, towerNumber);

                return View(nameof(Talkgroups), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Radios(string systemID, int towerNumber)
        {
            try
            {
                var model = CreateTowerModel(systemID, towerNumber);

                return View(nameof(Radios), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Patches(string systemID, int towerNumber)
        {
            try
            {
                var model = CreateTowerModel(systemID, towerNumber);

                return View(nameof(Patches), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public async Task<ActionResult> GetFrequencies(string systemID, int towerNumber, string frequencyType, string searchData,
            [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TOWER);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (towerFrequencies, recordCount) = await _towerFrequencyService.GetFrequenciesForTowerAsync(systemID, towerNumber, frequencyType, filterData);

                return Json(new
                {
                    Data = towerFrequencies,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetNeighbors(string systemID, int towerNumber, string searchData, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.NEIGHBOR_TOWER);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (towerNeighbors, recordCount) = await _towerNeighborService.GetForTowerAsync(systemID, towerNumber, filterData);

                return Json(new
                {
                    Data = towerNeighbors,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetTalkgroups(string systemID, int towerNumber, string searchData, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TALKGROUP);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (towerTalkgroups, recordCount) = await _towerTalkgroupService.GetTalkgroupsForTowerAsync(systemID, towerNumber, filterData);

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

        public async Task<ActionResult> GetRadios(string systemID, int towerNumber, string searchData, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.RADIO);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (towerRadios, recordCount) = await _towerRadioService.GetRadiosForTowerAsync(systemID, towerNumber, filterData);

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

        public async Task<ActionResult> GetPatches(string systemID, int towerNumber, string searchData, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TALKGROUP);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (patches, recordCount) = await _patchService.GetForSystemTowerAsync(systemID, towerNumber, filterData);

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
    }
}