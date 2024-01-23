using FileService.Interfaces;
using ObjectLibrary.Interfaces;
using ProcessedFileService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Admin
{
    public partial class ProgressRadForm : RadForm
    {
        public ProgressRadForm()
        {
            InitializeComponent();
            ActionPanel.Visible = true;
            CompletedPanel.Visible = false;
        }

        public async Task RunActionAsync(IParser parser, IService service, string systemID, UploadFileModel uploadFile, bool showCompleted) =>
            await Task.Run(async () =>
            {
                UpdateFileInfo(uploadFile.FileName, uploadFile.TypeText);
                var items = await parser.ParseFileAsync(uploadFile.FileName, UpdateProgress);

                uploadFile.RowCount = await ProcessDataAsync(service, items, UpdateProgress, CompletedTasks, showCompleted);
            });

        private void UpdateFileInfo(string fileName, string fileType)
        {
            if (FileNameDataLabel.InvokeRequired || FileTypeDataLabel.InvokeRequired)
            {
                Invoke((MethodInvoker)(() =>
                {
                    UpdateFileInfo(fileName, fileType);
                }));
            }
            else
            {
                FileNameDataLabel.Text = fileName;
                FileTypeDataLabel.Text = fileType;
            }
        }

        private void UpdateProgress(string currentAction, string recordCount, int currentRecord, int recordTotal)
        {
            if (CurrentActionDataLabel.InvokeRequired || RecordsProcessedDataLabel.InvokeRequired || ProgressBar.InvokeRequired)
            {
                Invoke((MethodInvoker)(() =>
                {
                    UpdateProgress(currentAction, recordCount, currentRecord, recordTotal);
                }));
            }
            else
            {
                CurrentActionDataLabel.Text = currentAction;
                RecordsProcessedDataLabel.Text = recordCount;

                if (recordTotal == 0)
                {
                    ProgressBar.Maximum = 500000;
                }
                else
                {
                    ProgressBar.Maximum = recordTotal;
                }

                ProgressBar.Value1 = currentRecord;
            }
        }

        private static async Task<int> ProcessDataAsync(IService service, IEnumerable<IRecord> records, Action<string, string, int, int> updateProgress, Action<bool> completedTasks, bool showCompleted)
        {
            var rowCount = await service.ProcessRecordsAsync(records, updateProgress, completedTasks, showCompleted);

            return rowCount;
        }

        private void CompletedTasks(bool showCompleted)
        {
            if (ActionPanel.InvokeRequired)
            {
                if (!showCompleted)
                {
                    Action closeWindow = CloseWindow;

                    Invoke(closeWindow);
                }
                else
                {
                    Action<bool> callback = CompletedTasks;

                    Invoke(callback, true);
                }
            }
            else
            {
                ActionPanel.Visible = false;
                CompletedPanel.Visible = true;
                Refresh();
            }
        }

        private void CloseWindow() => Close();

        private void CloseWindowButton_Click(object sender, EventArgs e) => CloseWindow();
    }
}
