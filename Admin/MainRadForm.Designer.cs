namespace Admin
{
    partial class MainRadForm
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.SystemsLabel = new Telerik.WinControls.UI.RadLabel();
            this.SystemsRadGridView = new Telerik.WinControls.UI.RadGridView();
            this.GridWaitingBar = new Telerik.WinControls.UI.RadWaitingBar();
            this.dotsRingWaitingBarIndicatorElement1 = new Telerik.WinControls.UI.DotsRingWaitingBarIndicatorElement();
            this.SystemFileNameLabel = new Telerik.WinControls.UI.RadLabel();
            this.SystemFileNameTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.UploadSystemButton = new Telerik.WinControls.UI.RadButton();
            this.SelectSystemFileNameButton = new Telerik.WinControls.UI.RadButton();
            this.FluentTheme = new Telerik.WinControls.Themes.FluentTheme();
            ((System.ComponentModel.ISupportInitialize)(this.SystemsLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SystemsRadGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SystemsRadGridView.MasterTemplate)).BeginInit();
            this.SystemsRadGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridWaitingBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SystemFileNameLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SystemFileNameTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UploadSystemButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SelectSystemFileNameButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // SystemsLabel
            // 
            this.SystemsLabel.AutoSize = false;
            this.SystemsLabel.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold);
            this.SystemsLabel.Location = new System.Drawing.Point(20, 20);
            this.SystemsLabel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SystemsLabel.Name = "SystemsLabel";
            this.SystemsLabel.Size = new System.Drawing.Size(756, 27);
            this.SystemsLabel.TabIndex = 0;
            this.SystemsLabel.Text = "Systems";
            this.SystemsLabel.ThemeName = "Fluent";
            // 
            // SystemsRadGridView
            // 
            this.SystemsRadGridView.Controls.Add(this.GridWaitingBar);
            this.SystemsRadGridView.Location = new System.Drawing.Point(20, 62);
            this.SystemsRadGridView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            // 
            // 
            // 
            this.SystemsRadGridView.MasterTemplate.AllowAddNewRow = false;
            gridViewTextBoxColumn1.FieldName = "SystemID";
            gridViewTextBoxColumn1.HeaderText = "System ID";
            gridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn1.Name = "SystemID";
            gridViewTextBoxColumn1.Width = 120;
            gridViewTextBoxColumn2.FieldName = "Description";
            gridViewTextBoxColumn2.HeaderText = "Description";
            gridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn2.Name = "Description";
            gridViewTextBoxColumn2.Width = 351;
            gridViewTextBoxColumn3.DataType = typeof(int);
            gridViewTextBoxColumn3.FieldName = "TalkgroupCount";
            gridViewTextBoxColumn3.FormatString = "{0:#,##0}";
            gridViewTextBoxColumn3.HeaderText = "Talkgroups";
            gridViewTextBoxColumn3.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn3.Name = "TalkgroupCount";
            gridViewTextBoxColumn3.Width = 142;
            gridViewTextBoxColumn4.DataType = typeof(int);
            gridViewTextBoxColumn4.FieldName = "RadioCount";
            gridViewTextBoxColumn4.FormatString = "{0:#,##0}";
            gridViewTextBoxColumn4.HeaderText = "Radios";
            gridViewTextBoxColumn4.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn4.Name = "RadioCount";
            gridViewTextBoxColumn4.Width = 142;
            gridViewTextBoxColumn5.DataType = typeof(int);
            gridViewTextBoxColumn5.FieldName = "TowerCount";
            gridViewTextBoxColumn5.FormatString = "{0:#,##0}";
            gridViewTextBoxColumn5.HeaderText = "Towers";
            gridViewTextBoxColumn5.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn5.Name = "TowerCount";
            gridViewTextBoxColumn5.Width = 142;
            gridViewTextBoxColumn6.DataType = typeof(int);
            gridViewTextBoxColumn6.FieldName = "RowCount";
            gridViewTextBoxColumn6.FormatString = "{0:#,##0}";
            gridViewTextBoxColumn6.HeaderText = "Log Rows";
            gridViewTextBoxColumn6.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn6.Name = "RowCount";
            gridViewTextBoxColumn6.Width = 142;
            gridViewTextBoxColumn7.DataType = typeof(System.DateTime);
            gridViewTextBoxColumn7.FieldName = "FirstSeen";
            gridViewTextBoxColumn7.FormatString = "{0:MM-dd-yyyy HH:mm}";
            gridViewTextBoxColumn7.HeaderText = "First Seen";
            gridViewTextBoxColumn7.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn7.Name = "FirstSeen";
            gridViewTextBoxColumn7.Width = 180;
            gridViewTextBoxColumn8.DataType = typeof(System.DateTime);
            gridViewTextBoxColumn8.FieldName = "LastSeen";
            gridViewTextBoxColumn8.FormatString = "{0:MM-dd-yyyy HH:mm}";
            gridViewTextBoxColumn8.HeaderText = "Last Seen";
            gridViewTextBoxColumn8.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn8.Name = "LastSeen";
            gridViewTextBoxColumn8.Width = 180;
            this.SystemsRadGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8});
            this.SystemsRadGridView.MasterTemplate.EnableAlternatingRowColor = true;
            this.SystemsRadGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.SystemsRadGridView.Name = "SystemsRadGridView";
            this.SystemsRadGridView.ReadOnly = true;
            this.SystemsRadGridView.ShowGroupPanel = false;
            this.SystemsRadGridView.Size = new System.Drawing.Size(1470, 690);
            this.SystemsRadGridView.TabIndex = 1;
            this.SystemsRadGridView.ThemeName = "Fluent";
            this.SystemsRadGridView.DoubleClick += new System.EventHandler(this.SystemsRadGridView_DoubleClick);
            // 
            // GridWaitingBar
            // 
            this.GridWaitingBar.BackColor = System.Drawing.Color.White;
            this.GridWaitingBar.Location = new System.Drawing.Point(680, 284);
            this.GridWaitingBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GridWaitingBar.Name = "GridWaitingBar";
            this.GridWaitingBar.Size = new System.Drawing.Size(105, 105);
            this.GridWaitingBar.TabIndex = 5;
            this.GridWaitingBar.ThemeName = "Fluent";
            this.GridWaitingBar.WaitingIndicators.Add(this.dotsRingWaitingBarIndicatorElement1);
            this.GridWaitingBar.WaitingIndicatorSize = new System.Drawing.Size(150, 21);
            this.GridWaitingBar.WaitingSpeed = 50;
            this.GridWaitingBar.WaitingStep = 2;
            this.GridWaitingBar.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.DotsRing;
            // 
            // dotsRingWaitingBarIndicatorElement1
            // 
            this.dotsRingWaitingBarIndicatorElement1.Name = "dotsRingWaitingBarIndicatorElement1";
            // 
            // SystemFileNameLabel
            // 
            this.SystemFileNameLabel.AutoSize = false;
            this.SystemFileNameLabel.Location = new System.Drawing.Point(18, 772);
            this.SystemFileNameLabel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SystemFileNameLabel.Name = "SystemFileNameLabel";
            this.SystemFileNameLabel.Size = new System.Drawing.Size(140, 40);
            this.SystemFileNameLabel.TabIndex = 1;
            this.SystemFileNameLabel.Text = "System Filename";
            this.SystemFileNameLabel.ThemeName = "Fluent";
            // 
            // SystemFileNameTextBox
            // 
            this.SystemFileNameTextBox.Location = new System.Drawing.Point(166, 777);
            this.SystemFileNameTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SystemFileNameTextBox.Name = "SystemFileNameTextBox";
            this.SystemFileNameTextBox.Size = new System.Drawing.Size(1082, 36);
            this.SystemFileNameTextBox.TabIndex = 2;
            this.SystemFileNameTextBox.ThemeName = "Fluent";
            this.SystemFileNameTextBox.TextChanged += new System.EventHandler(this.SystemFileNameTextBox_TextChanged);
            // 
            // UploadSystemButton
            // 
            this.UploadSystemButton.Location = new System.Drawing.Point(1314, 778);
            this.UploadSystemButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.UploadSystemButton.Name = "UploadSystemButton";
            this.UploadSystemButton.Size = new System.Drawing.Size(176, 36);
            this.UploadSystemButton.TabIndex = 3;
            this.UploadSystemButton.Text = "Upload System";
            this.UploadSystemButton.ThemeName = "Fluent";
            this.UploadSystemButton.Click += new System.EventHandler(this.UploadSystemButton_Click);
            // 
            // SelectSystemFileNameButton
            // 
            this.SelectSystemFileNameButton.Location = new System.Drawing.Point(1257, 778);
            this.SelectSystemFileNameButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SelectSystemFileNameButton.Name = "SelectSystemFileNameButton";
            this.SelectSystemFileNameButton.Size = new System.Drawing.Size(48, 36);
            this.SelectSystemFileNameButton.TabIndex = 4;
            this.SelectSystemFileNameButton.Text = "...";
            this.SelectSystemFileNameButton.ThemeName = "Fluent";
            this.SelectSystemFileNameButton.Click += new System.EventHandler(this.SelectSystemFileNameButton_Click);
            // 
            // MainRadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1543, 745);
            this.Controls.Add(this.SelectSystemFileNameButton);
            this.Controls.Add(this.UploadSystemButton);
            this.Controls.Add(this.SystemFileNameTextBox);
            this.Controls.Add(this.SystemFileNameLabel);
            this.Controls.Add(this.SystemsRadGridView);
            this.Controls.Add(this.SystemsLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainRadForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Trunked Radio Info Admin";
            this.ThemeName = "Fluent";
            this.Load += new System.EventHandler(this.MainRadForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SystemsLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SystemsRadGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SystemsRadGridView)).EndInit();
            this.SystemsRadGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridWaitingBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SystemFileNameLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SystemFileNameTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UploadSystemButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SelectSystemFileNameButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadLabel SystemsLabel;
        private Telerik.WinControls.UI.RadGridView SystemsRadGridView;
        private Telerik.WinControls.UI.RadLabel SystemFileNameLabel;
        private Telerik.WinControls.UI.RadTextBox SystemFileNameTextBox;
        private Telerik.WinControls.UI.RadButton UploadSystemButton;
        private Telerik.WinControls.UI.RadButton SelectSystemFileNameButton;
        private Telerik.WinControls.Themes.FluentTheme FluentTheme;
        private Telerik.WinControls.UI.RadWaitingBar GridWaitingBar;
        private Telerik.WinControls.UI.DotsRingWaitingBarIndicatorElement dotsRingWaitingBarIndicatorElement1;
    }
}
