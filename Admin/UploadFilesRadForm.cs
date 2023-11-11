using Admin.Helpers;
using FileService.Interfaces;
using ProcessedFileService;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemInfoService;
using Telerik.WinControls.UI;

namespace Admin
{
    public partial class UploadFilesRadForm : RadForm
    {
        private const string STRING_SPACE = " ";
        private const string COLUMN_FILE_NAME = "FileName";

        private readonly ITowerFileService _towerFileService;
        private readonly IProcessedFileService _processedFileService;

        private readonly IResolver _resolver;
        private readonly SystemViewModel _systemInfo;
        private readonly BindingList<UploadFileModel> _fileList = new();

        public UploadFilesRadForm(IResolver resolver, SystemViewModel systemInfo, ITowerFileService towerFileService, IProcessedFileService processedFileService)
        {
            InitializeComponent();
            _towerFileService = towerFileService;
            _resolver = resolver;
            _systemInfo = systemInfo;
            _processedFileService = processedFileService;
            SelectFilesButton.Enabled = false;
            ProcessFilesButton.Enabled = false;
            ClearListButton.Enabled = false;
            SystemIDDataLabel.Text = systemInfo.SystemID;
            NameDataLabel.Text = systemInfo.Name;
            FilesRadGridView.DataSource = _fileList;
            SetFileTypeList();
        }

        private void SetFileTypeList()
        {
            foreach (var fileTypeName in Files.FileTypeNames)
            {
                FileTypeDropDownList.Items.Add(fileTypeName.Value);
            }
        }

        private void SelectFilesButton_Click(object sender, EventArgs e)
        {
            if (FileTypeDropDownList.SelectedItem == null)
            {
                return;
            }

            using var fileSelect = new OpenFileDialog
            {
                Filter = Files.FileAssociations[FileTypeDropDownList.SelectedItem
                    .ToString()
                    .Replace(STRING_SPACE, string.Empty)
                    .ToEnum<FileTypes>()],
                Multiselect = true
            };

            if (fileSelect.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var fileName in fileSelect.FileNames)
                {
                    var fileInfo = new FileInfo(fileName);

                    _fileList.Add(_processedFileService.CreateUploadFileModel(fileName, FileTypeDropDownList.SelectedItem
                        .ToString()
                        .Replace(STRING_SPACE, string.Empty), fileInfo.LastWriteTime, fileInfo.Length));
                }

                UpdateColumnHeader();
            }
        }

        private async void ProcessFilesButton_Click(object sender, EventArgs e)
        {
            var fileCount = FilesRadGridView.RowCount;
            var count = 1;

            foreach (var row in FilesRadGridView.Rows)
            {
                var uploadFile = (UploadFileModel)row.DataBoundItem;

                FilesRadGridView.TableElement.ScrollToRow(row.Index);

                switch (uploadFile.Type)
                {
                    case FileTypes.Radios:
                        await ProcessRadiosAsync(uploadFile, (count == fileCount));
                        break;

                    case FileTypes.Talkgroups:
                        await ProcessTalkgroupsAsync(uploadFile, (count == fileCount));
                        break;

                    case FileTypes.Tower:
                        await ProcessTowerAsync(uploadFile, (count == fileCount));
                        break;

                    default:
                        await ProcessLogsAsync(uploadFile, (count == fileCount));
                        break;
                }

                count++;
            }
        }

        private async Task ProcessTalkgroupsAsync(UploadFileModel uploadFile, bool lastFile)
        {
            if (await _processedFileService.FileExistsAsync(_systemInfo.ID, uploadFile.FileName, uploadFile.LastModified, uploadFile.Type))
            {
                uploadFile.Status = FileStatus.Skipped;
            }
            else
            {
                uploadFile.Status = FileStatus.Processing;

                using var progressForm = _resolver.GetProgressForm();

                BeginInvoke((Action)(() => progressForm.ShowDialog(this)));
                await progressForm.RunActionAsync(_resolver.GetParser(UnityConfig.PARSER_TALKGROUP, _systemInfo.ID),
                    _resolver.GetTalkgroupFileService(_systemInfo.ID), _systemInfo.SystemID, uploadFile, lastFile);
                uploadFile.Status = FileStatus.Processed;
                await _processedFileService.WriteAsync(_systemInfo.ID, uploadFile);
            }
        }

        private async Task ProcessRadiosAsync(UploadFileModel uploadFile, bool lastFile)
        {
            if (await _processedFileService.FileExistsAsync(_systemInfo.ID, uploadFile.FileName, uploadFile.LastModified, uploadFile.Type))
            {
                uploadFile.Status = FileStatus.Skipped;
            }
            else
            {
                uploadFile.Status = FileStatus.Processing;

                using var progressForm = _resolver.GetProgressForm();

                BeginInvoke((Action)(() => progressForm.ShowDialog(this)));
                await progressForm.RunActionAsync(_resolver.GetParser(UnityConfig.PARSER_RADIO, _systemInfo.ID),
                    _resolver.GetRadioFileService(_systemInfo.ID), _systemInfo.SystemID, uploadFile, lastFile);
                uploadFile.Status = FileStatus.Processed;
                await _processedFileService.WriteAsync(_systemInfo.ID, uploadFile);
            }
        }

        private async Task ProcessTowerAsync(UploadFileModel fileInfo, bool lastFile)
        {
            fileInfo.Status = FileStatus.Processing;

            await ProcessTowerFileAsync(fileInfo, lastFile);
        }

        private async Task ProcessTowerFileAsync(UploadFileModel uploadFile, bool lastFile)
        {
            if (await _processedFileService.FileExistsAsync(_systemInfo.ID, uploadFile.FileName, uploadFile.LastModified, uploadFile.Type))
            {
                uploadFile.Status = FileStatus.Skipped;
            }
            else
            {
                var currentCursor = Cursor.Current;

                Cursor.Current = Cursors.WaitCursor;

                var parser = _resolver.GetParser(uploadFile.Type.ToString(), _systemInfo.ID);

                await ShowProgress(parser, _towerFileService, uploadFile, lastFile);
                Cursor.Current = currentCursor;
            }
        }

        private async Task ProcessLogsAsync(UploadFileModel uploadFile, bool lastFile)
        {
            if (await _processedFileService.FileExistsAsync(_systemInfo.ID, uploadFile.FileName, uploadFile.LastModified, uploadFile.Type))
            {
                uploadFile.Status = FileStatus.Skipped;
            }
            else
            {
                uploadFile.Status = FileStatus.Processing;
                await ProcessFileAsync(uploadFile, lastFile);
            }
        }

        private async Task ProcessFileAsync(UploadFileModel uploadFile, bool lastFile)
        {
            var currentCursor = Cursor.Current;

            Cursor.Current = Cursors.WaitCursor;

            var parser = _resolver.GetParser(uploadFile.Type.ToString(), _systemInfo.ID);
            var service = _resolver.GetFileService(_systemInfo.ID, uploadFile);

            await ShowProgress(parser, service, uploadFile, lastFile);
            Cursor.Current = currentCursor;
        }

        private async Task ShowProgress(IParser parser, IService service, UploadFileModel uploadFile, bool lastFile)
        {
            using var progressForm = _resolver.GetProgressForm();

            BeginInvoke((Action)(() => progressForm.ShowDialog(this)));
            await progressForm.RunActionAsync(parser, service, _systemInfo.SystemID, uploadFile, lastFile);
            await ProcessFileCompletedAsync(uploadFile);
        }

        private async Task ProcessFileCompletedAsync(UploadFileModel uploadFile)
        {
            uploadFile.Status = FileStatus.Processed;
            await _processedFileService.WriteAsync(_systemInfo.ID, uploadFile);
        }

        private void ClearListButton_Click(object sender, EventArgs e)
        {
            _fileList.Clear();
            UpdateColumnHeader();
        }

        private void UpdateColumnHeader() => FilesRadGridView.Columns[COLUMN_FILE_NAME].HeaderText = $"File Name ({FilesRadGridView.RowCount:#,##0})";

        private void FilesRadGridView_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            ProcessFilesButton.Enabled = ((GridViewRowCollection)sender).Count != 0;
            ClearListButton.Enabled = ProcessFilesButton.Enabled;
        }

        private void FileTypeDropDownList_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e) =>
            SelectFilesButton.Enabled = !string.IsNullOrWhiteSpace(((RadDropDownList)sender).SelectedText);
    }
}
