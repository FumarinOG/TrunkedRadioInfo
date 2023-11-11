using Kendo.Mvc.UI;
using SearchDataService;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemInfoService;
using TowerFrequencyRadioService;
using TowerFrequencyService;
using TowerFrequencyTalkgroupService;
using TowerService;
using Web.Helpers;
using static Web.Helpers.Factory;

namespace Web.Controllers
{
    public class FrequencyController : BaseController
    {
        private const string GRID_LAYOUT = "_GridLayout";

        private readonly ITowerFrequencyService _towerFrequencyService;
        private readonly ITowerFrequencyTalkgroupService _towerFrequencyTalkgroupService;
        private readonly ITowerFrequencyRadioService _towerFrequencyRadioService;
        private readonly ISystemInfoService _systemInfoService;
        private readonly ITowerService _towerService;
        private readonly ISearchDataService _searchDataService;

        public FrequencyController(ITowerFrequencyService towerFrequencyService, ITowerFrequencyTalkgroupService towerFrequencyTalkgroupService,
            ITowerFrequencyRadioService towerFrequencyRadioService, ISystemInfoService systemInfoService, ITowerService towerService,
            ISearchDataService searchDataService)
        {
            _towerFrequencyService = towerFrequencyService;
            _towerFrequencyTalkgroupService = towerFrequencyTalkgroupService;
            _towerFrequencyRadioService = towerFrequencyRadioService;
            _systemInfoService = systemInfoService;
            _towerService = towerService;
            _searchDataService = searchDataService;
        }

        public async Task<ActionResult> Index(string systemID, int towerNumber, string frequency)
        {
            try
            {
                var systemInfo = await _systemInfoService.GetAsync(systemID);

                ViewBag.Title = "Frequency";
                return View(await _towerFrequencyService.GetSummaryAsync(systemInfo, await _towerService.GetDetailAsync(systemInfo.ID, towerNumber), frequency,
                    _searchDataService.GetView(Request.Form[FilterDataHelper.SEARCH_DATA])));
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Talkgroups(string systemID, int towerNumber, string frequency)
        {
            try
            {
                var model = CreateTowerFrequencyModel(systemID, towerNumber, frequency);

                return View(nameof(Talkgroups), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public ActionResult Radios(string systemID, int towerNumber, string frequency)
        {
            try
            {
                var model = CreateTowerFrequencyModel(systemID, towerNumber, frequency);

                return View(nameof(Radios), GRID_LAYOUT, model);
            }
            catch (Exception exception)
            {
                return HandleViewError(exception);
            }
        }

        public async Task<ActionResult> GetTalkgroups(string systemID, int towerNumber, string frequency, string searchData,
            [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TALKGROUP);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (towerFrequencyTalkgroups, recordCount) = await _towerFrequencyTalkgroupService.GetTalkgroupsForTowerFrequencyAsync(systemID, towerNumber,
                    frequency, filterData);

                return Json(new
                {
                    Data = towerFrequencyTalkgroups,
                    Total = recordCount
                });
            }
            catch (Exception exception)
            {
                return HandleJSONError(exception);
            }
        }

        public async Task<ActionResult> GetRadios(string systemID, int towerNumber, string frequency, string searchData,
            [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var filterDataHelper = CreateFilterDataHelper(FilterDataHelper.TALKGROUP);
                var filterData = filterDataHelper.ConvertRequest(request, _searchDataService.Create(searchData));
                var (towerFrequencyRadios, recordCount) = await _towerFrequencyRadioService.GetRadiosForTowerFrequencyAsync(systemID, towerNumber,
                    frequency, filterData);

                return Json(new
                {
                    Data = towerFrequencyRadios,
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