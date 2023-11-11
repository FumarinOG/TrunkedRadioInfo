using Admin.Helpers;
using RadioHistoryService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemInfoService;
using TalkgroupRadioService;
using Telerik.WinControls.UI;
using TowerRadioService;
using static Admin.Helpers.Factory;
using IRadioService = RadioService.IRadioService;

namespace Admin
{
    public partial class RadioDataRadForm : RadForm
    {
        private const string PAGE_TOWERS = "TowersViewPage";
        private const string PAGE_TALKGROUPS = "TalkgroupsViewPage";
        private const string PAGE_HISTORY = "HistoryViewPage";

        private readonly SystemViewModel _systemInfo;
        private readonly IRadioService _radioService;
        private readonly ITowerRadioService _towerRadioService;
        private readonly ITalkgroupRadioService _talkgroupRadioService;
        private readonly IRadioHistoryService _radioHistoryService;
        private readonly int _radioID;

        private readonly ICollection<PageLoadStatus> _pageLoadStatus = new List<PageLoadStatus>();

        public RadioDataRadForm(SystemViewModel systemInfo, IRadioService radioService, ITowerRadioService towerRadioService,
            ITalkgroupRadioService talkgroupRadioService, IRadioHistoryService radioHistoryService, int radioID)
        {
            InitializeComponent();
            _systemInfo = systemInfo;
            _radioService = radioService;
            _towerRadioService = towerRadioService;
            _talkgroupRadioService = talkgroupRadioService;
            _radioHistoryService = radioHistoryService;
            _radioID = radioID;

            BuildPageActions();
        }

        private void BuildPageActions()
        {
            _pageLoadStatus.Add(CreatePageLoadStatus(PAGE_TOWERS, FillTowerListAsync));
            _pageLoadStatus.Add(CreatePageLoadStatus(PAGE_TALKGROUPS, FillTalkgroupListAsync));
            _pageLoadStatus.Add(CreatePageLoadStatus(PAGE_HISTORY, FillHistoryListAsync));
        }

        private async Task FillDataCountsAsync()
        {
            var currentCursor = Cursor.Current;

            Cursor.Current = Cursors.WaitCursor;
            TowersViewPage.Text = $@"Towers ({await _towerRadioService.GetTowersForRadioCountAsync(_systemInfo.ID, _radioID):#,##0})";
            TalkgroupsViewPage.Text = $@"Talkgroups ({await _talkgroupRadioService.GetTalkgroupsForRadioCountAsync(_systemInfo.ID, _radioID):#,##0})";
            HistoryViewPage.Text = $@"History ({await _radioHistoryService.GetForRadioCountAsync(_systemInfo.ID, _radioID):#,##0})";
            Cursor.Current = currentCursor;
        }

        private async Task FillDataAsync(string pageName)
        {
            var tabProcessor = _pageLoadStatus.SingleOrDefault(tls => tls.PageName == pageName && !tls.IsLoaded);

            if (tabProcessor != null)
            {
                await tabProcessor.LoadMethod();
                tabProcessor.IsLoaded = true;
            }
        }

        private async Task DisplayDataAsync()
        {
            var radio = await _radioService.GetDetailAsync(_systemInfo.ID, _radioID);

            RadioIDDataLabel.Text = radio.RadioID.ToString();
            RadioDescriptionDataLabel.Text = radio.RadioName;
        }

        private async Task FillTowerListAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = TowersRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            TowersRadGridView.DataSource = await _towerRadioService.GetTowersForRadioAsync(_systemInfo.ID, _radioID);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async Task FillTalkgroupListAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = TalkgroupsRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            TalkgroupsRadGridView.DataSource = await _talkgroupRadioService.GetTalkgroupsForRadioAsync(_systemInfo.ID, _radioID);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async Task FillHistoryListAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = HistoryRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            HistoryRadGridView.DataSource = await _radioHistoryService.GetForRadioAsync(_systemInfo.ID, _radioID);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async void RadioPageView_SelectedPageChanged(object sender, System.EventArgs e) => await FillDataAsync(((RadPageView)sender).SelectedPage.Name);

        private async void RadioDataRadForm_Load(object sender, System.EventArgs e)
        {
            await FillDataCountsAsync();
            await DisplayDataAsync();
            await FillDataAsync(PAGE_TOWERS);
        }
    }
}
