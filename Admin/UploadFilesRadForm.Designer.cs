namespace Admin
{
    partial class UploadFilesRadForm
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.SystemIDDataLabel = new Telerik.WinControls.UI.RadLabel();
            this.NameDataLabel = new Telerik.WinControls.UI.RadLabel();
            this.FileTypeLabel = new Telerik.WinControls.UI.RadLabel();
            this.SystemIDLabel = new Telerik.WinControls.UI.RadLabel();
            this.FilesRadGridView = new Telerik.WinControls.UI.RadGridView();
            this.ClearListButton = new Telerik.WinControls.UI.RadButton();
            this.ProcessFilesButton = new Telerik.WinControls.UI.RadButton();
            this.SelectFilesButton = new Telerik.WinControls.UI.RadButton();
            this.FileTypeDropDownList = new Telerik.WinControls.UI.RadDropDownList();
            this.NameLabel = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.SystemIDDataLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NameDataLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileTypeLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SystemIDLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilesRadGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilesRadGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClearListButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessFilesButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SelectFilesButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileTypeDropDownList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NameLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // SystemIDDataLabel
            // 
            this.SystemIDDataLabel.AutoSize = false;
            this.SystemIDDataLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F);
            this.SystemIDDataLabel.Location = new System.Drawing.Point(92, 12);
            this.SystemIDDataLabel.Name = "SystemIDDataLabel";
            this.SystemIDDataLabel.Size = new System.Drawing.Size(230, 25);
            this.SystemIDDataLabel.TabIndex = 7;
            this.SystemIDDataLabel.ThemeName = "Fluent";
            // 
            // NameDataLabel
            // 
            this.NameDataLabel.AutoSize = false;
            this.NameDataLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F);
            this.NameDataLabel.Location = new System.Drawing.Point(414, 12);
            this.NameDataLabel.Name = "NameDataLabel";
            this.NameDataLabel.Size = new System.Drawing.Size(332, 25);
            this.NameDataLabel.TabIndex = 8;
            this.NameDataLabel.ThemeName = "Fluent";
            // 
            // FileTypeLabel
            // 
            this.FileTypeLabel.AutoSize = false;
            this.FileTypeLabel.Location = new System.Drawing.Point(290, 549);
            this.FileTypeLabel.Name = "FileTypeLabel";
            this.FileTypeLabel.Size = new System.Drawing.Size(62, 25);
            this.FileTypeLabel.TabIndex = 6;
            this.FileTypeLabel.Text = "File Type";
            this.FileTypeLabel.ThemeName = "Fluent";
            // 
            // SystemIDLabel
            // 
            this.SystemIDLabel.AutoSize = false;
            this.SystemIDLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold);
            this.SystemIDLabel.Location = new System.Drawing.Point(7, 12);
            this.SystemIDLabel.Name = "SystemIDLabel";
            this.SystemIDLabel.Size = new System.Drawing.Size(79, 25);
            this.SystemIDLabel.TabIndex = 5;
            this.SystemIDLabel.Text = "System ID";
            this.SystemIDLabel.ThemeName = "Fluent";
            // 
            // FilesRadGridView
            // 
            this.FilesRadGridView.Location = new System.Drawing.Point(7, 44);
            // 
            // 
            // 
            this.FilesRadGridView.MasterTemplate.AllowAddNewRow = false;
            gridViewTextBoxColumn1.FieldName = "FileName";
            gridViewTextBoxColumn1.HeaderText = "File Name";
            gridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn1.Name = "FileName";
            gridViewTextBoxColumn1.Width = 475;
            gridViewTextBoxColumn2.FieldName = "TypeText";
            gridViewTextBoxColumn2.HeaderText = "File Type";
            gridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn2.Name = "FileType";
            gridViewTextBoxColumn2.Width = 94;
            gridViewTextBoxColumn3.DataType = typeof(long);
            gridViewTextBoxColumn3.FieldName = "Size";
            gridViewTextBoxColumn3.FormatString = "{0:#,##0}";
            gridViewTextBoxColumn3.HeaderText = "Size";
            gridViewTextBoxColumn3.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn3.Name = "Size";
            gridViewTextBoxColumn3.Width = 90;
            gridViewTextBoxColumn4.FieldName = "StatusText";
            gridViewTextBoxColumn4.HeaderText = "Status";
            gridViewTextBoxColumn4.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn4.Name = "Status";
            gridViewTextBoxColumn4.Width = 120;
            this.FilesRadGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.FilesRadGridView.MasterTemplate.EnableAlternatingRowColor = true;
            this.FilesRadGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.FilesRadGridView.Name = "FilesRadGridView";
            this.FilesRadGridView.ReadOnly = true;
            this.FilesRadGridView.ShowGroupPanel = false;
            this.FilesRadGridView.Size = new System.Drawing.Size(827, 495);
            this.FilesRadGridView.TabIndex = 9;
            this.FilesRadGridView.ThemeName = "Fluent";
            this.FilesRadGridView.RowsChanged += new Telerik.WinControls.UI.GridViewCollectionChangedEventHandler(this.FilesRadGridView_RowsChanged);
            // 
            // ClearListButton
            // 
            this.ClearListButton.Location = new System.Drawing.Point(13, 551);
            this.ClearListButton.Name = "ClearListButton";
            this.ClearListButton.Size = new System.Drawing.Size(130, 24);
            this.ClearListButton.TabIndex = 10;
            this.ClearListButton.Text = "Clear List";
            this.ClearListButton.ThemeName = "Fluent";
            this.ClearListButton.Click += new System.EventHandler(this.ClearListButton_Click);
            // 
            // ProcessFilesButton
            // 
            this.ProcessFilesButton.Location = new System.Drawing.Point(700, 551);
            this.ProcessFilesButton.Name = "ProcessFilesButton";
            this.ProcessFilesButton.Size = new System.Drawing.Size(130, 24);
            this.ProcessFilesButton.TabIndex = 11;
            this.ProcessFilesButton.Text = "Process Files";
            this.ProcessFilesButton.ThemeName = "Fluent";
            this.ProcessFilesButton.Click += new System.EventHandler(this.ProcessFilesButton_Click);
            // 
            // SelectFilesButton
            // 
            this.SelectFilesButton.Location = new System.Drawing.Point(564, 551);
            this.SelectFilesButton.Name = "SelectFilesButton";
            this.SelectFilesButton.Size = new System.Drawing.Size(130, 24);
            this.SelectFilesButton.TabIndex = 12;
            this.SelectFilesButton.Text = "Select Files";
            this.SelectFilesButton.ThemeName = "Fluent";
            this.SelectFilesButton.Click += new System.EventHandler(this.SelectFilesButton_Click);
            // 
            // FileTypeDropDownList
            // 
            this.FileTypeDropDownList.Location = new System.Drawing.Point(358, 552);
            this.FileTypeDropDownList.Name = "FileTypeDropDownList";
            this.FileTypeDropDownList.Size = new System.Drawing.Size(200, 24);
            this.FileTypeDropDownList.TabIndex = 13;
            this.FileTypeDropDownList.ThemeName = "Fluent";
            this.FileTypeDropDownList.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.FileTypeDropDownList_SelectedIndexChanged);
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = false;
            this.NameLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold);
            this.NameLabel.Location = new System.Drawing.Point(350, 12);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(58, 25);
            this.NameLabel.TabIndex = 6;
            this.NameLabel.Text = "Name";
            this.NameLabel.ThemeName = "Fluent";
            // 
            // UploadFilesRadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 587);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.FileTypeDropDownList);
            this.Controls.Add(this.SelectFilesButton);
            this.Controls.Add(this.ProcessFilesButton);
            this.Controls.Add(this.ClearListButton);
            this.Controls.Add(this.FilesRadGridView);
            this.Controls.Add(this.SystemIDDataLabel);
            this.Controls.Add(this.NameDataLabel);
            this.Controls.Add(this.FileTypeLabel);
            this.Controls.Add(this.SystemIDLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "UploadFilesRadForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Upload Files";
            this.ThemeName = "Fluent";
            ((System.ComponentModel.ISupportInitialize)(this.SystemIDDataLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NameDataLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileTypeLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SystemIDLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilesRadGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilesRadGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClearListButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessFilesButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SelectFilesButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileTypeDropDownList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NameLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel SystemIDDataLabel;
        private Telerik.WinControls.UI.RadLabel NameDataLabel;
        private Telerik.WinControls.UI.RadLabel FileTypeLabel;
        private Telerik.WinControls.UI.RadLabel SystemIDLabel;
        private Telerik.WinControls.UI.RadGridView FilesRadGridView;
        private Telerik.WinControls.UI.RadButton ClearListButton;
        private Telerik.WinControls.UI.RadButton ProcessFilesButton;
        private Telerik.WinControls.UI.RadButton SelectFilesButton;
        private Telerik.WinControls.UI.RadDropDownList FileTypeDropDownList;
        private Telerik.WinControls.UI.RadLabel NameLabel;
    }
}
