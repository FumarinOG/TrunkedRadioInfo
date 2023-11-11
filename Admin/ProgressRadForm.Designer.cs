namespace Admin
{
    partial class ProgressRadForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProgressLabel = new Telerik.WinControls.UI.RadLabel();
            this.ActionPanel = new System.Windows.Forms.Panel();
            this.RecordsProcessedDataLabel = new Telerik.WinControls.UI.RadLabel();
            this.CurrentActionDataLabel = new Telerik.WinControls.UI.RadLabel();
            this.FileTypeDataLabel = new Telerik.WinControls.UI.RadLabel();
            this.FileNameDataLabel = new Telerik.WinControls.UI.RadLabel();
            this.ProgressBar = new Telerik.WinControls.UI.RadProgressBar();
            this.RecordsProcessedLabel = new Telerik.WinControls.UI.RadLabel();
            this.CurrentActionLabel = new Telerik.WinControls.UI.RadLabel();
            this.FileTypeLabel = new Telerik.WinControls.UI.RadLabel();
            this.FileNameLabel = new Telerik.WinControls.UI.RadLabel();
            this.CompletedPanel = new System.Windows.Forms.Panel();
            this.OperationCompleteLabel = new Telerik.WinControls.UI.RadLabel();
            this.CloseWindowButton = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressLabel)).BeginInit();
            this.ActionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RecordsProcessedDataLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentActionDataLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileTypeDataLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileNameDataLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RecordsProcessedLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentActionLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileTypeLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileNameLabel)).BeginInit();
            this.CompletedPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationCompleteLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseWindowButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = false;
            this.ProgressLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold);
            this.ProgressLabel.Location = new System.Drawing.Point(12, 12);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(734, 27);
            this.ProgressLabel.TabIndex = 1;
            this.ProgressLabel.Text = "Progress Information";
            this.ProgressLabel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProgressLabel.ThemeName = "Fluent";
            // 
            // ActionPanel
            // 
            this.ActionPanel.Controls.Add(this.RecordsProcessedDataLabel);
            this.ActionPanel.Controls.Add(this.CurrentActionDataLabel);
            this.ActionPanel.Controls.Add(this.FileTypeDataLabel);
            this.ActionPanel.Controls.Add(this.FileNameDataLabel);
            this.ActionPanel.Controls.Add(this.ProgressBar);
            this.ActionPanel.Controls.Add(this.RecordsProcessedLabel);
            this.ActionPanel.Controls.Add(this.CurrentActionLabel);
            this.ActionPanel.Controls.Add(this.FileTypeLabel);
            this.ActionPanel.Controls.Add(this.FileNameLabel);
            this.ActionPanel.Location = new System.Drawing.Point(13, 46);
            this.ActionPanel.Name = "ActionPanel";
            this.ActionPanel.Size = new System.Drawing.Size(733, 155);
            this.ActionPanel.TabIndex = 2;
            // 
            // RecordsProcessedDataLabel
            // 
            this.RecordsProcessedDataLabel.AutoSize = false;
            this.RecordsProcessedDataLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F);
            this.RecordsProcessedDataLabel.Location = new System.Drawing.Point(174, 90);
            this.RecordsProcessedDataLabel.Name = "RecordsProcessedDataLabel";
            this.RecordsProcessedDataLabel.Size = new System.Drawing.Size(556, 23);
            this.RecordsProcessedDataLabel.TabIndex = 3;
            this.RecordsProcessedDataLabel.ThemeName = "Fluent";
            // 
            // CurrentActionDataLabel
            // 
            this.CurrentActionDataLabel.AutoSize = false;
            this.CurrentActionDataLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F);
            this.CurrentActionDataLabel.Location = new System.Drawing.Point(174, 61);
            this.CurrentActionDataLabel.Name = "CurrentActionDataLabel";
            this.CurrentActionDataLabel.Size = new System.Drawing.Size(556, 23);
            this.CurrentActionDataLabel.TabIndex = 6;
            this.CurrentActionDataLabel.ThemeName = "Fluent";
            // 
            // FileTypeDataLabel
            // 
            this.FileTypeDataLabel.AutoSize = false;
            this.FileTypeDataLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F);
            this.FileTypeDataLabel.Location = new System.Drawing.Point(174, 32);
            this.FileTypeDataLabel.Name = "FileTypeDataLabel";
            this.FileTypeDataLabel.Size = new System.Drawing.Size(556, 23);
            this.FileTypeDataLabel.TabIndex = 3;
            this.FileTypeDataLabel.ThemeName = "Fluent";
            // 
            // FileNameDataLabel
            // 
            this.FileNameDataLabel.AutoSize = false;
            this.FileNameDataLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F);
            this.FileNameDataLabel.Location = new System.Drawing.Point(174, 3);
            this.FileNameDataLabel.Name = "FileNameDataLabel";
            this.FileNameDataLabel.Size = new System.Drawing.Size(556, 23);
            this.FileNameDataLabel.TabIndex = 2;
            this.FileNameDataLabel.ThemeName = "Fluent";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(4, 120);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.ShowProgressIndicators = true;
            this.ProgressBar.Size = new System.Drawing.Size(726, 24);
            this.ProgressBar.TabIndex = 5;
            this.ProgressBar.Text = "0 %";
            this.ProgressBar.ThemeName = "Fluent";
            // 
            // RecordsProcessedLabel
            // 
            this.RecordsProcessedLabel.AutoSize = false;
            this.RecordsProcessedLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F);
            this.RecordsProcessedLabel.Location = new System.Drawing.Point(3, 90);
            this.RecordsProcessedLabel.Name = "RecordsProcessedLabel";
            this.RecordsProcessedLabel.Size = new System.Drawing.Size(165, 23);
            this.RecordsProcessedLabel.TabIndex = 4;
            this.RecordsProcessedLabel.Text = "Records Processed";
            this.RecordsProcessedLabel.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.RecordsProcessedLabel.ThemeName = "Fluent";
            // 
            // CurrentActionLabel
            // 
            this.CurrentActionLabel.AutoSize = false;
            this.CurrentActionLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F);
            this.CurrentActionLabel.Location = new System.Drawing.Point(3, 61);
            this.CurrentActionLabel.Name = "CurrentActionLabel";
            this.CurrentActionLabel.Size = new System.Drawing.Size(165, 23);
            this.CurrentActionLabel.TabIndex = 3;
            this.CurrentActionLabel.Text = "Current Action";
            this.CurrentActionLabel.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.CurrentActionLabel.ThemeName = "Fluent";
            // 
            // FileTypeLabel
            // 
            this.FileTypeLabel.AutoSize = false;
            this.FileTypeLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F);
            this.FileTypeLabel.Location = new System.Drawing.Point(3, 32);
            this.FileTypeLabel.Name = "FileTypeLabel";
            this.FileTypeLabel.Size = new System.Drawing.Size(165, 23);
            this.FileTypeLabel.TabIndex = 2;
            this.FileTypeLabel.Text = "File Type";
            this.FileTypeLabel.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.FileTypeLabel.ThemeName = "Fluent";
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.AutoSize = false;
            this.FileNameLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F);
            this.FileNameLabel.Location = new System.Drawing.Point(3, 3);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(165, 23);
            this.FileNameLabel.TabIndex = 1;
            this.FileNameLabel.Text = "File Name";
            this.FileNameLabel.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.FileNameLabel.ThemeName = "Fluent";
            // 
            // CompletedPanel
            // 
            this.CompletedPanel.Controls.Add(this.OperationCompleteLabel);
            this.CompletedPanel.Controls.Add(this.CloseWindowButton);
            this.CompletedPanel.Location = new System.Drawing.Point(12, 45);
            this.CompletedPanel.Name = "CompletedPanel";
            this.CompletedPanel.Size = new System.Drawing.Size(733, 155);
            this.CompletedPanel.TabIndex = 7;
            // 
            // OperationCompleteLabel
            // 
            this.OperationCompleteLabel.AutoSize = false;
            this.OperationCompleteLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.OperationCompleteLabel.Location = new System.Drawing.Point(7, 17);
            this.OperationCompleteLabel.Name = "OperationCompleteLabel";
            this.OperationCompleteLabel.Size = new System.Drawing.Size(721, 39);
            this.OperationCompleteLabel.TabIndex = 4;
            this.OperationCompleteLabel.Text = "Operation Complete!";
            this.OperationCompleteLabel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.OperationCompleteLabel.ThemeName = "Fluent";
            // 
            // CloseWindowButton
            // 
            this.CloseWindowButton.Location = new System.Drawing.Point(298, 79);
            this.CloseWindowButton.Name = "CloseWindowButton";
            this.CloseWindowButton.Size = new System.Drawing.Size(139, 36);
            this.CloseWindowButton.TabIndex = 0;
            this.CloseWindowButton.Text = "Close Window";
            this.CloseWindowButton.ThemeName = "Fluent";
            this.CloseWindowButton.Click += new System.EventHandler(this.CloseWindowButton_Click);
            // 
            // ProgressRadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 215);
            this.ControlBox = false;
            this.Controls.Add(this.ActionPanel);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.CompletedPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ProgressRadForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Progress Update";
            this.ThemeName = "Fluent";
            ((System.ComponentModel.ISupportInitialize)(this.ProgressLabel)).EndInit();
            this.ActionPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RecordsProcessedDataLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentActionDataLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileTypeDataLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileNameDataLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RecordsProcessedLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentActionLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileTypeLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileNameLabel)).EndInit();
            this.CompletedPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OperationCompleteLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseWindowButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel ProgressLabel;
        private System.Windows.Forms.Panel ActionPanel;
        private Telerik.WinControls.UI.RadLabel RecordsProcessedDataLabel;
        private Telerik.WinControls.UI.RadLabel CurrentActionDataLabel;
        private Telerik.WinControls.UI.RadLabel FileTypeDataLabel;
        private Telerik.WinControls.UI.RadLabel FileNameDataLabel;
        private Telerik.WinControls.UI.RadProgressBar ProgressBar;
        private Telerik.WinControls.UI.RadLabel RecordsProcessedLabel;
        private Telerik.WinControls.UI.RadLabel CurrentActionLabel;
        private Telerik.WinControls.UI.RadLabel FileTypeLabel;
        private Telerik.WinControls.UI.RadLabel FileNameLabel;
        private System.Windows.Forms.Panel CompletedPanel;
        private Telerik.WinControls.UI.RadButton CloseWindowButton;
        private Telerik.WinControls.UI.RadLabel OperationCompleteLabel;
    }
}
