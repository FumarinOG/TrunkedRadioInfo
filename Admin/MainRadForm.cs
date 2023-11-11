using Admin.Helpers;
using ProcessedFileService;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemInfoService;
using Telerik.WinControls.UI;

namespace Admin
{
    public partial class MainRadForm : RadForm
    {
        private const string COL_SYSTEM_ID = "SystemID";

        private readonly IResolver _resolver;
        private readonly ISystemInfoService _systemInfoService;

        public MainRadForm(IResolver resolver, ISystemInfoService systemInfoService)
        {
            InitializeComponent();
            UploadSystemButton.Enabled = false;
            _resolver = resolver;
            _systemInfoService = systemInfoService;
        }

        private async Task LoadSystemListAsync()
        {
            var currentCursor = Cursor.Current;

            GridWaitingBar.AssociatedControl = SystemsRadGridView;
            GridWaitingBar.StartWaiting();
            Cursor.Current = Cursors.WaitCursor;
            SystemsRadGridView.DataSource = await _systemInfoService.GetListAsync();
            SystemsLabel.Text = $"Systems ({SystemsRadGridView.RowCount:#,##0})";
            Cursor.Current = currentCursor;
            GridWaitingBar.StopWaiting();
        }

        private async void SystemsRadGridView_DoubleClick(object sender, EventArgs e)
        {
            var grid = (RadGridView)sender;

            if ((grid.CurrentRow == null) || (grid.CurrentRow.GetType() != typeof(GridViewDataRowInfo)))
            {
                return;
            }

            await ViewSystemAsync(grid.CurrentRow.Cells[COL_SYSTEM_ID].Value.ToString());
        }

        private async Task ViewSystemAsync(string systemID)
        {
            using (var systemInfoForm = _resolver.GetSystemInfoForm())
            {
                systemInfoForm.SystemInfo = await _systemInfoService.GetSystemAsync(systemID);
                systemInfoForm.ShowDialog(this);
                await LoadSystemListAsync();
            }
        }

        private void SelectSystemFileNameButton_Click(object sender, EventArgs e)
        {
            var fileSelect = new OpenFileDialog
            {
                Filter = Files.FileAssociations[FileTypes.System]
            };

            if (fileSelect.ShowDialog(this) == DialogResult.OK)
            {
                SystemFileNameTextBox.Text = fileSelect.FileName;
            }
        }

        private void SystemFileNameTextBox_TextChanged(object sender, EventArgs e) =>
            UploadSystemButton.Enabled = !string.IsNullOrWhiteSpace(((RadTextBox)sender).Text);

        private async void UploadSystemButton_Click(object sender, EventArgs e)
        {
            var currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            if (!string.IsNullOrWhiteSpace(SystemFileNameTextBox.Text))
            {
                var systemParser = _resolver.GetParser(UnityConfig.PARSER_SYSTEM);
                var system = await systemParser.ParseFileAsync(SystemFileNameTextBox.Text, (s, s1, i, i1) => { });
                var systemService = _resolver.GetSystemInfoService();

                await systemService.WriteAsync(system);

                MessageBox.Show(this, @"System Uploaded!");
                await LoadSystemListAsync();
            }

            Cursor.Current = currentCursor;
        }

        private async void MainRadForm_Load(object sender, EventArgs e) =>
            await LoadSystemListAsync();
    }
}
