namespace Admin
{
    partial class PatchDataRadForm
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
            this.ToTalkgroupDataLabel = new Telerik.WinControls.UI.RadLabel();
            this.ToTalkgroupLabel = new Telerik.WinControls.UI.RadLabel();
            this.FromTalkgroupDataLabel = new Telerik.WinControls.UI.RadLabel();
            this.FromTalkgroupLabel = new Telerik.WinControls.UI.RadLabel();
            this.PatchesRadGridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.ToTalkgroupDataLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToTalkgroupLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromTalkgroupDataLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromTalkgroupLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PatchesRadGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PatchesRadGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // ToTalkgroupDataLabel
            // 
            this.ToTalkgroupDataLabel.AutoSize = false;
            this.ToTalkgroupDataLabel.Location = new System.Drawing.Point(121, 43);
            this.ToTalkgroupDataLabel.Name = "ToTalkgroupDataLabel";
            this.ToTalkgroupDataLabel.Size = new System.Drawing.Size(438, 25);
            this.ToTalkgroupDataLabel.TabIndex = 11;
            this.ToTalkgroupDataLabel.ThemeName = "Fluent";
            // 
            // ToTalkgroupLabel
            // 
            this.ToTalkgroupLabel.AutoSize = false;
            this.ToTalkgroupLabel.Location = new System.Drawing.Point(12, 43);
            this.ToTalkgroupLabel.Name = "ToTalkgroupLabel";
            this.ToTalkgroupLabel.Size = new System.Drawing.Size(103, 25);
            this.ToTalkgroupLabel.TabIndex = 10;
            this.ToTalkgroupLabel.Text = "To Talkgroup";
            this.ToTalkgroupLabel.ThemeName = "Fluent";
            // 
            // FromTalkgroupDataLabel
            // 
            this.FromTalkgroupDataLabel.AutoSize = false;
            this.FromTalkgroupDataLabel.Location = new System.Drawing.Point(121, 12);
            this.FromTalkgroupDataLabel.Name = "FromTalkgroupDataLabel";
            this.FromTalkgroupDataLabel.Size = new System.Drawing.Size(438, 25);
            this.FromTalkgroupDataLabel.TabIndex = 9;
            this.FromTalkgroupDataLabel.ThemeName = "Fluent";
            // 
            // FromTalkgroupLabel
            // 
            this.FromTalkgroupLabel.AutoSize = false;
            this.FromTalkgroupLabel.Location = new System.Drawing.Point(12, 12);
            this.FromTalkgroupLabel.Name = "FromTalkgroupLabel";
            this.FromTalkgroupLabel.Size = new System.Drawing.Size(103, 25);
            this.FromTalkgroupLabel.TabIndex = 8;
            this.FromTalkgroupLabel.Text = "From Talkgroup";
            this.FromTalkgroupLabel.ThemeName = "Fluent";
            // 
            // PatchesRadGridView
            // 
            this.PatchesRadGridView.Location = new System.Drawing.Point(12, 74);
            // 
            // 
            // 
            this.PatchesRadGridView.MasterTemplate.AllowAddNewRow = false;
            gridViewTextBoxColumn1.DataType = typeof(int);
            gridViewTextBoxColumn1.FieldName = "TowerNumber";
            gridViewTextBoxColumn1.HeaderText = "Tower #";
            gridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn1.Name = "TowerNumber";
            gridViewTextBoxColumn1.Width = 75;
            gridViewTextBoxColumn2.FieldName = "Description";
            gridViewTextBoxColumn2.HeaderText = "Description";
            gridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn2.Name = "Description";
            gridViewTextBoxColumn2.Width = 225;
            gridViewTextBoxColumn3.DataType = typeof(System.DateTime);
            gridViewTextBoxColumn3.FieldName = "Date";
            gridViewTextBoxColumn3.FormatString = "{0:MM-dd-yyyy HH:mm}";
            gridViewTextBoxColumn3.HeaderText = "Date";
            gridViewTextBoxColumn3.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn3.Name = "Date";
            gridViewTextBoxColumn3.Width = 100;
            gridViewTextBoxColumn4.DataType = typeof(int);
            gridViewTextBoxColumn4.FieldName = "HitCount";
            gridViewTextBoxColumn4.FormatString = "{0:#,##0}";
            gridViewTextBoxColumn4.HeaderText = "Hit Count";
            gridViewTextBoxColumn4.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn4.Name = "HitCount";
            gridViewTextBoxColumn4.Width = 100;
            this.PatchesRadGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.PatchesRadGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.PatchesRadGridView.Name = "PatchesRadGridView";
            this.PatchesRadGridView.ReadOnly = true;
            this.PatchesRadGridView.ShowGroupPanel = false;
            this.PatchesRadGridView.Size = new System.Drawing.Size(574, 477);
            this.PatchesRadGridView.TabIndex = 12;
            this.PatchesRadGridView.ThemeName = "Fluent";
            // 
            // PatchDataRadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 563);
            this.Controls.Add(this.PatchesRadGridView);
            this.Controls.Add(this.ToTalkgroupDataLabel);
            this.Controls.Add(this.ToTalkgroupLabel);
            this.Controls.Add(this.FromTalkgroupDataLabel);
            this.Controls.Add(this.FromTalkgroupLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "PatchDataRadForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Patch Info";
            this.ThemeName = "Fluent";
            this.Load += new System.EventHandler(this.PatchDataRadForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ToTalkgroupDataLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToTalkgroupLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromTalkgroupDataLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromTalkgroupLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PatchesRadGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PatchesRadGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel ToTalkgroupDataLabel;
        private Telerik.WinControls.UI.RadLabel ToTalkgroupLabel;
        private Telerik.WinControls.UI.RadLabel FromTalkgroupDataLabel;
        private Telerik.WinControls.UI.RadLabel FromTalkgroupLabel;
        private Telerik.WinControls.UI.RadGridView PatchesRadGridView;
    }
}
