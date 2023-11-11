using PatchService;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalkgroupService;
using Telerik.WinControls.UI;

namespace Admin
{
    public partial class PatchDataRadForm : RadForm
    {
        private readonly IPatchService _patchService;
        private readonly ITalkgroupService _talkgroupService;
        private readonly int _systemID;
        private readonly int _fromTalkgroupID;
        private readonly int _toTalkgroupID;

        public PatchDataRadForm(IPatchService patchService, ITalkgroupService talkgroupService, int systemID, int fromTalkgroupID, int toTalkgroupID)
        {
            InitializeComponent();
            _patchService = patchService;
            _talkgroupService = talkgroupService;
            _systemID = systemID;
            _fromTalkgroupID = fromTalkgroupID;
            _toTalkgroupID = toTalkgroupID;
        }

        private async Task DisplayDataAsync()
        {
            var currentCursor = Cursor.Current;
            var fromTalkgroup = await _talkgroupService.GetDetailAsync(_systemID, _fromTalkgroupID);
            var toTalkgroup = await _talkgroupService.GetDetailAsync(_systemID, _toTalkgroupID);

            Cursor.Current = Cursors.WaitCursor;
            FromTalkgroupDataLabel.Text = $@"{fromTalkgroup.TalkgroupID} ({fromTalkgroup.TalkgroupName})";
            ToTalkgroupDataLabel.Text = $@"{toTalkgroup.TalkgroupID} ({toTalkgroup.TalkgroupName})";
            PatchesRadGridView.DataSource = _patchService.GetForPatchByDateAsync(_systemID, _fromTalkgroupID, _toTalkgroupID);
            Cursor.Current = currentCursor;
        }

        private async void PatchDataRadForm_Load(object sender, System.EventArgs e) => await DisplayDataAsync();
    }
}
