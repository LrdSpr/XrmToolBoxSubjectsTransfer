namespace TransferCaseSubjects
{
    partial class TransferCaseSubjects
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.saveHierarchyDialog = new System.Windows.Forms.SaveFileDialog();
            this.openHierarchyDialog = new System.Windows.Forms.OpenFileDialog();
            this.subjectsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addSubjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSubjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subjectsTreeView = new System.Windows.Forms.TreeView();
            this.editSubjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.subjectsContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton1,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1633, 39);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(286, 36);
            this.toolStripButton2.Text = "Load Subjects From CRM";
            this.toolStripButton2.Click += new System.EventHandler(this.loadHierarchy);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(268, 36);
            this.toolStripButton3.Text = "Load Subjects from File";
            this.toolStripButton3.Click += new System.EventHandler(this.loadHierarchyFromFile);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(241, 36);
            this.toolStripButton1.Text = "Save Subjects To File";
            this.toolStripButton1.Click += new System.EventHandler(this.saveHierarchy);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(277, 36);
            this.toolStripButton4.Text = "Update Subjects In CRM";
            this.toolStripButton4.Click += new System.EventHandler(this.updateInCrm);
            // 
            // subjectsContextMenu
            // 
            this.subjectsContextMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.subjectsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSubjectToolStripMenuItem,
            this.removeSubjectToolStripMenuItem,
            this.editSubjectToolStripMenuItem});
            this.subjectsContextMenu.Name = "subjectsContextMenu";
            this.subjectsContextMenu.Size = new System.Drawing.Size(301, 156);
            // 
            // addSubjectToolStripMenuItem
            // 
            this.addSubjectToolStripMenuItem.Name = "addSubjectToolStripMenuItem";
            this.addSubjectToolStripMenuItem.Size = new System.Drawing.Size(300, 36);
            this.addSubjectToolStripMenuItem.Text = "Add Subject";
            this.addSubjectToolStripMenuItem.Click += new System.EventHandler(this.addSubjectToolStripMenuItem_Click);
            // 
            // removeSubjectToolStripMenuItem
            // 
            this.removeSubjectToolStripMenuItem.Name = "removeSubjectToolStripMenuItem";
            this.removeSubjectToolStripMenuItem.Size = new System.Drawing.Size(300, 36);
            this.removeSubjectToolStripMenuItem.Text = "Remove Subject";
            this.removeSubjectToolStripMenuItem.Click += new System.EventHandler(this.removeSubjectToolStripMenuItem_Click);
            // 
            // subjectsTreeView
            // 
            this.subjectsTreeView.ContextMenuStrip = this.subjectsContextMenu;
            this.subjectsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subjectsTreeView.Location = new System.Drawing.Point(0, 39);
            this.subjectsTreeView.Name = "subjectsTreeView";
            this.subjectsTreeView.Size = new System.Drawing.Size(1633, 918);
            this.subjectsTreeView.TabIndex = 3;
            // 
            // editSubjectToolStripMenuItem
            // 
            this.editSubjectToolStripMenuItem.Name = "editSubjectToolStripMenuItem";
            this.editSubjectToolStripMenuItem.Size = new System.Drawing.Size(300, 36);
            this.editSubjectToolStripMenuItem.Text = "Edit Subject";
            this.editSubjectToolStripMenuItem.Click += new System.EventHandler(this.editSubjectToolStripMenuItem_Click);
            // 
            // TransferCaseSubjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.subjectsTreeView);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "TransferCaseSubjects";
            this.Size = new System.Drawing.Size(1633, 957);
            this.Load += new System.EventHandler(this.TransferCaseSubjects_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.subjectsContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.SaveFileDialog saveHierarchyDialog;
        private System.Windows.Forms.OpenFileDialog openHierarchyDialog;
        private System.Windows.Forms.ContextMenuStrip subjectsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addSubjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSubjectToolStripMenuItem;
        private System.Windows.Forms.TreeView subjectsTreeView;
        private System.Windows.Forms.ToolStripMenuItem editSubjectToolStripMenuItem;
    }
}
