using Admin.Helpers;
using PatchService;
using ProcessedFileService;
using RadioService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemInfoService;
using TalkgroupService;
using Telerik.WinControls.UI;
using TowerFrequencyService;
using TowerNeighborService;
using TowerService;

namespace Admin
{
    public partial class SystemInfoRadForm : RadForm
    {
        public SystemViewModel SystemInfo { get; set; }

        private const string COL_TALKGROUP_ID = "TalkgroupID";
        private const string COL_RADIO_ID = "RadioID";
        private const string COL_TOWER_NUMBER = "TowerNumber";
        private const string COL_FROM_TALKGROUP_ID = "FromTalkgroupID";
        private const string COL_TO_TALKGROUP_ID = "ToTalkgroupID";

        private const string PAGE_TALKGROUPS = "TalkgroupsViewPage";
        private const string PAGE_RADIOS = "RadiosViewPage";
        private const string PAGE_TOWERS = "TowersViewPage";
        private const string PAGE_PATCHES = "PatchesViewPage";
        private const string PAGE_PROCESSED_FILES = "ProcessedFilesViewPage";

        private readonly IResolver _resolver;
        private readonly ITalkgroupService _talkgroupService;
        private readonly IRadioService _radioService;
        private readonly ITowerService _towerService;
        private readonly ITowerFrequencyService _towerFrequencyService;
        private readonly ITowerNeighborService _towerNeighborService;
        private readonly IPatchService _patchService;
        private readonly IProcessedFileService _processedFileService;

        private readonly ICollection<PageLoadStatus> _pageLoadStatus = new List<PageLoadStatus>();

        public SystemInfoRadForm(IResolver resolver, ITalkgroupService talkgroupService, IRadioService radioService, ITowerService towerService,
            ITowerFrequencyService towerFrequencyService, ITowerNeighborService towerNeighborService, IPatchService patchService,
            IProcessedFileService processedFileService)
        {
            InitializeComponent();
            _resolver = resolver;
            _talkgroupService = talkgroupService;
            _radioService = radioService;
            _towerService = towerService;
            _towerFrequencyService = towerFrequencyService;
            _towerNeighborService = towerNeighborService;
            _patchService = patchService;
            _processedFileService = processedFileService;
            BuildTabActions();
        }

        private async void SystemInfoRadForm_Load(object sender, EventArgs e)
        {
            SystemIDDataLabel.Text = SystemInfo.SystemID;
            NameDataLabel.Text = SystemInfo.Name;

            await FillDataCountsAsync();
            await FillDataAsync(PAGE_TALKGROUPS);
        }

        private void BuildTabActions()
        {
            _pageLoadStatus.Add(new PageLoadStatus(PAGE_TALKGROUPS, LoadTalkgroupsAsync));
            _pageLoadStatus.Add(new PageLoadStatus(PAGE_RADIOS, LoadRadiosAsync));
            _pageLoadStatus.Add(new PageLoadStatus(PAGE_TOWERS, LoadTowersAsync));
            _pageLoadStatus.Add(new PageLoadStatus(PAGE_PATCHES, LoadPatchesAsync));
            _pageLoadStatus.Add(new PageLoadStatus(PAGE_PROCESSED_FILES, LoadProcessedFilesAsync));
        }

        private async Task FillDataCountsAsync()
        {
            var currentCursor = Cursor.Current;

            Cursor.Current = Cursors.WaitCursor;
            TalkgroupsViewPage.Text = $@"Talkgroups ({await _talkgroupService.GetCountForSystemAsync(SystemInfo.ID):#,##0})";
            RadiosViewPage.Text = $@"Radios ({await _radioService.GetCountForSystemAsync(SystemInfo.ID):#,##0})";
            TowersViewPage.Text = $@"Towers ({await _towerService.GetCountForSystemAsync(SystemInfo.ID):#,##0})";
            PatchesViewPage.Text = $@"Patches ({await _patchService.GetCountForSystemAsync(SystemInfo.ID, string.Empty):#,##0})";
            ProcessedFilesViewPage.Text = $@"Processed Files ({await _processedFileService.GetCountForSystemAsync(SystemInfo.ID, string.Empty):#,##0})";
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

        private void ResetLoadStatuses()
        {
            foreach (var tabLoadStatus in _pageLoadStatus)
            {
                tabLoadStatus.IsLoaded = false;
            }
        }

        private async Task LoadTalkgroupsAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = TalkgroupsRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            TalkgroupsRadGridView.DataSource = await _talkgroupService.GetDetailForSystemAsync(SystemInfo.ID);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async Task LoadRadiosAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = RadiosRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            RadiosRadGridView.DataSource = await _radioService.GetDetailForSystemAsync(SystemInfo.ID);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async Task LoadTowersAsync()
        {
            var currentCursor = Cursor.Current;

            Cursor.Current = Cursors.WaitCursor;
            TowersRadGridView.DataSource = await _towerService.GetForSystemAsync(SystemInfo.ID);
            Cursor.Current = currentCursor;
        }

        private async Task LoadPatchesAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = PatchesRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            PatchesRadGridView.DataSource = await _patchService.GetSummaryForSystemAsync(SystemInfo.ID);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async Task LoadProcessedFilesAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = ProcessedFilesRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            ProcessedFilesRadGridView.DataSource = await _processedFileService.GetForSystemAsync(SystemInfo.ID);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async void UploadFilesButton_Click(object sender, EventArgs e)
        {
            using (var uploadFiles = _resolver.GetUploadFilesForm(SystemInfo))
            {
                uploadFiles.ShowDialog(this);
                await FillDataCountsAsync();
                ResetLoadStatuses();
                await FillDataAsync(PAGE_TALKGROUPS);
            }
        }

        private async void SystemInfoPageView_SelectedPageChanged(object sender, EventArgs e) => await FillDataAsync(((RadPageView)sender).SelectedPage.Name);

        private void TalkgroupsRadGridView_DoubleClick(object sender, EventArgs e) => ViewData((RadGridView)sender, ViewTalkgroup, COL_TALKGROUP_ID);

        private void ViewTalkgroup(int talkgroupID)
        {
            using (var talkgroupData = _resolver.GetTalkgroupDataForm(SystemInfo, talkgroupID))
            {
                talkgroupData.ShowDialog(this);
            }
        }

        private void RadiosRadGridView_DoubleClick(object sender, EventArgs e)
        {
            var grid = (RadGridView)sender;

            if (grid.CurrentRow.GetType() != typeof(GridViewDataRowInfo))
            {
                return;
            }

            ViewData((RadGridView)sender, ViewRadio, COL_RADIO_ID);
        }

        private void ViewRadio(int radioID)
        {
            using (var radioData = _resolver.GetRadioDataForm(SystemInfo, radioID))
            {
                radioData.ShowDialog(this);
            }
        }

        private async void TowersRadGridView_SelectionChanged(object sender, EventArgs e)
        {
            var towerNumber = (int)((RadGridView)sender).SelectedRows[0].Cells[COL_TOWER_NUMBER].Value;

            await LoadTowerFrequenciesAsync(towerNumber);
            await LoadTowerNeighborsAsync(towerNumber);
        }

        private async Task LoadTowerFrequenciesAsync(int towerNumber)
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = TowerFrequenciesRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            TowerFrequenciesRadGridView.DataSource = await _towerFrequencyService.GetFrequenciesForTowerAsync(SystemInfo.ID, towerNumber);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async Task LoadTowerNeighborsAsync(int towerNumber)
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = TowerNeighborsRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            TowerNeighborsRadGridView.DataSource = await _towerNeighborService.GetForTowerAsync(SystemInfo.ID, towerNumber);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private void TowersRadGridView_DoubleClick(object sender, EventArgs e) => ViewData((RadGridView)sender, ViewTower, COL_TOWER_NUMBER);

        private void ViewTower(int towerNumber)
        {
            using (var towerData = _resolver.GetTowerDataForm(SystemInfo, towerNumber))
            {
                towerData.ShowDialog(this);
            }
        }

        private void PatchesRadGridView_DoubleClick(object sender, EventArgs e)
        {
            var grid = (RadGridView)sender;

            if (grid.CurrentRow.GetType() != typeof(GridViewDataRowInfo))
            {
                return;
            }

            ViewPatch((int)grid.CurrentRow.Cells[COL_FROM_TALKGROUP_ID].Value, (int)grid.CurrentRow.Cells[COL_TO_TALKGROUP_ID].Value);
        }

        private void ViewPatch(int fromTalkgroupID, int toTalkgroupID)
        {
            using (var patchData = _resolver.GetPatchDataForm(SystemInfo.ID, fromTalkgroupID, toTalkgroupID))
            {
                patchData.ShowDialog(this);
            }
        }

        private static void ViewData(RadGridView grid, Action<int> viewData, string column)
        {
            if (grid.CurrentRow.GetType() != typeof(GridViewDataRowInfo))
            {
                return;
            }

            viewData((int)grid.CurrentRow.Cells[column].Value);
        }
    }
}
