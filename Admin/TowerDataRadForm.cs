using Admin.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemInfoService;
using Telerik.WinControls.UI;
using TowerFrequencyService;
using TowerRadioService;
using TowerTalkgroupService;
using static Admin.Helpers.Factory;
using ITowerService = TowerService.ITowerService;

namespace Admin
{
    public partial class TowerDataRadForm : RadForm
    {
        private const string PAGE_FREQUENCIES = "FrequenciesViewPage";
        private const string PAGE_TALKGROUPS = "TalkgroupsViewPage";
        private const string PAGE_RADIOS = "RadiosViewPage";

        private const string COL_TALKGROUP_ID = "TalkgroupID";

        private const string FREQUENCY_TYPE_CURRENT = "Current";
        private const string FREQUENCY_TYPE_NOT_CURRENT = "Not Current";
        private const string FREQUENCY_TYPE_ALL = "All";

        private readonly IResolver _resolver;
        private readonly SystemViewModel _systemInfo;
        private readonly ITowerService _towerService;
        private readonly ITowerFrequencyService _towerFrequencyService;
        private readonly ITowerTalkgroupService _towerTalkgroupService;
        private readonly ITowerRadioService _towerRadioService;
        private readonly int _towerNumber;

        private readonly ICollection<PageLoadStatus> _pageLoadStatus = new List<PageLoadStatus>();

        private enum FrequencyTypes
        {
            [Description("Current")]
            Current,
            [Description("Not Current")]
            NotCurrent,
            [Description("All")]
            All
        }

        public TowerDataRadForm(IResolver resolver, SystemViewModel systemInfo, ITowerService towerService, ITowerFrequencyService towerFrequencyService,
            ITowerTalkgroupService towerTalkgroupService, ITowerRadioService towerRadioService, int towerNumber)
        {
            InitializeComponent();
            _resolver = resolver;
            _systemInfo = systemInfo;
            _towerService = towerService;
            _towerFrequencyService = towerFrequencyService;
            _towerTalkgroupService = towerTalkgroupService;
            _towerRadioService = towerRadioService;
            _towerNumber = towerNumber;

            BuildPageActions();
        }

        private void BuildPageActions()
        {
            _pageLoadStatus.Add(CreatePageLoadStatus(PAGE_FREQUENCIES, FillFrequencyListAsync));
            _pageLoadStatus.Add(CreatePageLoadStatus(PAGE_TALKGROUPS, FillTalkgroupListAsync));
            _pageLoadStatus.Add(CreatePageLoadStatus(PAGE_RADIOS, FillRadioListAsync));
        }

        private async Task FillDataCountsAsync()
        {
            var currentCursor = Cursor.Current;

            Cursor.Current = Cursors.WaitCursor;
            await FillFrequencyCountAsync(GetFrequencyTypes());
            TalkgroupsViewPage.Text = $@"Talkgroups ({await _towerTalkgroupService.GetTalkgroupsForTowerCountAsync(_systemInfo.ID, _towerNumber):#,##0})";
            RadiosViewPage.Text = $@"Radios ({await _towerRadioService.GetRadiosForTowerCountAsync(_systemInfo.ID, _towerNumber):#,##0})";
            Cursor.Current = currentCursor;
        }

        private async Task FillFrequencyCountAsync(FrequencyTypes frequencyType)
        {
            FrequenciesViewPage.Text = frequencyType switch
            {
                FrequencyTypes.Current => $@"Frequencies ({await _towerFrequencyService.GetFrequenciesForTowerCountAsync(_systemInfo.ID, _towerNumber):#,##0})",
                FrequencyTypes.NotCurrent => $@"Frequencies ({await _towerFrequencyService.GetFrequenciesForTowerNotCurrentCountAsync(_systemInfo.ID,
                    _towerNumber):#,##0})",
                FrequencyTypes.All => $@"Frequencies ({await _towerFrequencyService.GetFrequenciesForTowerAllCountAsync(_systemInfo.ID,
                    _towerNumber):#,##0})",
                _ => throw new Exception("Invalid frequency type"),
            };
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
            var towerInfo = await _towerService.GetDetailAsync(_systemInfo.ID, _towerNumber);

            TowerNumberDataLabel.Text = towerInfo.TowerNumber.ToString();
            TowerDescriptionDataLabel.Text = towerInfo.TowerName;
        }

        private async Task FillFrequencyListAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = FrequenciesRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;

            FrequenciesRadGridView.DataSource = (GetFrequencyTypes()) switch
            {
                FrequencyTypes.Current => await _towerFrequencyService.GetFrequenciesForTowerAsync(_systemInfo.ID, _towerNumber),
                FrequencyTypes.NotCurrent => await _towerFrequencyService.GetFrequenciesForTowerNotCurrentAsync(_systemInfo.ID, _towerNumber),
                FrequencyTypes.All => await _towerFrequencyService.GetFrequenciesForTowerAllAsync(_systemInfo.ID, _towerNumber),
                _ => throw new Exception("Invalid frequency type"),
            };

            await FillFrequencyCountAsync(GetFrequencyTypes());
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private FrequencyTypes GetFrequencyTypes()
        {
            var selected = FrequenciesRadPanel.Controls.OfType<RadRadioButton>().FirstOrDefault(rb => rb.IsChecked);

            if (selected == null)
            {
                throw new Exception("Missing frequency type");
            }

            return selected.Text switch
            {
                FREQUENCY_TYPE_CURRENT => FrequencyTypes.Current,
                FREQUENCY_TYPE_NOT_CURRENT => FrequencyTypes.NotCurrent,
                FREQUENCY_TYPE_ALL => FrequencyTypes.All,
                _ => throw new Exception("Invalid frequency type"),
            };
        }

        private async Task FillTalkgroupListAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = TalkgroupsRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            TalkgroupsRadGridView.DataSource = await _towerTalkgroupService.GetTalkgroupsForTowerAsync(_systemInfo.ID, _towerNumber);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async Task FillRadioListAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = RadiosRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            RadiosRadGridView.DataSource = await _towerRadioService.GetRadiosForTowerAsync(_systemInfo.ID, _towerNumber);
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private void TalkgroupsRadGridView_DoubleClick(object sender, EventArgs e)
        {
            var grid = (RadGridView)sender;

            if (grid.CurrentRow.GetType() != typeof(GridViewDataRowInfo))
            {
                return;
            }

            ViewTalkgroup((int)grid.CurrentRow.Cells[COL_TALKGROUP_ID].Value);
        }

        private void ViewTalkgroup(int talkgroupID)
        {
            using (var talkgroupData = _resolver.GetTalkgroupDataForm(_systemInfo, talkgroupID))
            {
                talkgroupData.ShowDialog(this);
            }
        }

        private async void CurrentFrequenciesRadRadioButton_ToggleStateChanged(object sender, StateChangedEventArgs args) => await FillFrequencyListAsync();

        private async void NotCurrentFrequenciesRadRadioButton_ToggleStateChanged(object sender, StateChangedEventArgs args) => await FillFrequencyListAsync();

        private async void AllFrequenciesRadRadioButton_ToggleStateChanged(object sender, StateChangedEventArgs args) => await FillFrequencyListAsync();

        private async void TowerPageView_SelectedPageChanged(object sender, EventArgs e) => await FillDataAsync(((RadPageView)sender).SelectedPage.Name);

        private async void TowerDataRadForm_Load(object sender, EventArgs e)
        {
            await FillDataCountsAsync();
            await DisplayDataAsync();
            await FillDataAsync(PAGE_FREQUENCIES);
        }
    }
}
