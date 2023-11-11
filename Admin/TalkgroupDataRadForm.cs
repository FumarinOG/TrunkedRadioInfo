using Admin.Helpers;
using PatchService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemInfoService;
using TalkgroupHistoryService;
using TalkgroupRadioService;
using Telerik.WinControls.UI;
using TowerTalkgroupService;
using ITalkgroupService = TalkgroupService.ITalkgroupService;

namespace Admin
{
    public partial class TalkgroupDataRadForm : RadForm
    {
        private const string PAGE_TOWERS = "TowersViewPage";
        private const string PAGE_RADIOS = "RadiosViewPage";
        private const string PAGE_PATCHES = "PatchesViewPage";
        private const string PAGE_HISTORY = "HistoryViewPage";

        private readonly SystemViewModel _systemInfo;
        private readonly ITalkgroupService _talkgroupService;
        private readonly ITowerTalkgroupService _towerTalkgroupService;
        private readonly ITalkgroupRadioService _talkgroupRadioService;
        private readonly IPatchService _patchService;
        private readonly ITalkgroupHistoryService _talkgroupHistoryService;
        private readonly int _talkgroupID;

        private readonly ICollection<PageLoadStatus> _pageLoadStatus = new List<PageLoadStatus>();

        public TalkgroupDataRadForm(SystemViewModel systemInfo, ITalkgroupService talkgroupService, ITowerTalkgroupService towerTalkgroupService,
            ITalkgroupRadioService talkgroupRadioService, IPatchService patchService, ITalkgroupHistoryService talkgroupHistoryService, int talkgroupID)
        {
            InitializeComponent();
            _systemInfo = systemInfo;
            _talkgroupService = talkgroupService;
            _towerTalkgroupService = towerTalkgroupService;
            _talkgroupRadioService = talkgroupRadioService;
            _patchService = patchService;
            _talkgroupHistoryService = talkgroupHistoryService;
            _talkgroupID = talkgroupID;

            BuildTabActions();
        }

        private void BuildTabActions()
        {
            _pageLoadStatus.Add(new PageLoadStatus(PAGE_TOWERS, FillTowerListAsync));
            _pageLoadStatus.Add(new PageLoadStatus(PAGE_RADIOS, FillRadioListAsync));
            _pageLoadStatus.Add(new PageLoadStatus(PAGE_PATCHES, FillPatchListAsync));
            _pageLoadStatus.Add(new PageLoadStatus(PAGE_HISTORY, FillHistoryListAsync));
        }

        private async Task FillDataCountsAsync()
        {
            var currentCursor = Cursor.Current;

            Cursor.Current = Cursors.WaitCursor;
            TowersViewPage.Text = $@"Towers ({await _towerTalkgroupService.GetTowersForTalkgroupCountAsync(_systemInfo.ID, _talkgroupID):#,##0})";
            RadiosViewPage.Text = $@"Radios ({await _talkgroupRadioService.GetRadiosForTalkgroupCountAsync(_systemInfo.ID, _talkgroupID):#,##0})";
            PatchesViewPage.Text = $@"Patches ({await _patchService.GetForSystemTalkgroupCountAsync(_systemInfo.ID, _talkgroupID):#,##0})";
            HistoryViewPage.Text = $@"History ({await _talkgroupHistoryService.GetForTalkgroupCountAsync(_systemInfo.ID, _talkgroupID):#,##0})";
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
            var talkgroup = await _talkgroupService.GetDetailAsync(_systemInfo.ID, _talkgroupID);

            TalkgroupIDDataLabel.Text = talkgroup.TalkgroupID.ToString();
            TalkgroupDescriptionDataLabel.Text = talkgroup.TalkgroupName;
            await FillDataAsync(PAGE_TOWERS);
        }

        private async Task FillTowerListAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = TowersRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            TowersRadGridView.DataSource = await _towerTalkgroupService.GetTowersForTalkgroupAsync(_systemInfo.ID, _talkgroupID);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async Task FillRadioListAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = RadiosRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            RadiosRadGridView.DataSource = await _talkgroupRadioService.GetRadiosForTalkgroupAsync(_systemInfo.ID, _talkgroupID);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async Task FillPatchListAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = PatchesRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            PatchesRadGridView.DataSource = await _patchService.GetForSystemTalkgroupAsync(_systemInfo.ID, _talkgroupID);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async Task FillHistoryListAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = HistoryRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            HistoryRadGridView.DataSource = await _talkgroupHistoryService.GetForTalkgroupAsync(_systemInfo.ID, _talkgroupID);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async void TalkgroupPageView_SelectedPageChanged(object sender, System.EventArgs e) => await FillDataAsync(((RadPageView)sender).SelectedPage.Name);

        private async void TalkgroupDataRadForm_Load(object sender, System.EventArgs e)
        {
            await FillDataCountsAsync();
            await DisplayDataAsync();
        }
    }
}
