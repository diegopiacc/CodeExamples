namespace TMain
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salvaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simulazioneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.luceAmbientaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.giornostandardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simpleOpenGlControl1 = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_newSimulation = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.simulazioneToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(725, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salvaToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // salvaToolStripMenuItem
            // 
            this.salvaToolStripMenuItem.Name = "salvaToolStripMenuItem";
            this.salvaToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.salvaToolStripMenuItem.Text = "Salva Immagine";
            this.salvaToolStripMenuItem.Click += new System.EventHandler(this.salvaToolStripMenuItem_Click);
            // 
            // simulazioneToolStripMenuItem
            // 
            this.simulazioneToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.luceAmbientaleToolStripMenuItem,
            this.resetToolStripMenuItem});
            this.simulazioneToolStripMenuItem.Name = "simulazioneToolStripMenuItem";
            this.simulazioneToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.simulazioneToolStripMenuItem.Text = "Simulazione";
            // 
            // luceAmbientaleToolStripMenuItem
            // 
            this.luceAmbientaleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.giornostandardToolStripMenuItem,
            this.notteToolStripMenuItem});
            this.luceAmbientaleToolStripMenuItem.Name = "luceAmbientaleToolStripMenuItem";
            this.luceAmbientaleToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.luceAmbientaleToolStripMenuItem.Text = "Luce Ambientale";
            // 
            // giornostandardToolStripMenuItem
            // 
            this.giornostandardToolStripMenuItem.Name = "giornostandardToolStripMenuItem";
            this.giornostandardToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.giornostandardToolStripMenuItem.Text = "Giorno (standard)";
            this.giornostandardToolStripMenuItem.Click += new System.EventHandler(this.giornostandardToolStripMenuItem_Click);
            // 
            // notteToolStripMenuItem
            // 
            this.notteToolStripMenuItem.Name = "notteToolStripMenuItem";
            this.notteToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.notteToolStripMenuItem.Text = "Notte";
            this.notteToolStripMenuItem.Click += new System.EventHandler(this.notteToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.indexToolStripMenuItem,
            this.toolStripSeparator1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.indexToolStripMenuItem.Text = "Index";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(123, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // simpleOpenGlControl1
            // 
            this.simpleOpenGlControl1.AccumBits = ((byte)(0));
            this.simpleOpenGlControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleOpenGlControl1.AutoCheckErrors = false;
            this.simpleOpenGlControl1.AutoFinish = false;
            this.simpleOpenGlControl1.AutoMakeCurrent = true;
            this.simpleOpenGlControl1.AutoSize = true;
            this.simpleOpenGlControl1.AutoSwapBuffers = true;
            this.simpleOpenGlControl1.BackColor = System.Drawing.Color.Black;
            this.simpleOpenGlControl1.ColorBits = ((byte)(32));
            this.simpleOpenGlControl1.DepthBits = ((byte)(16));
            this.simpleOpenGlControl1.Location = new System.Drawing.Point(0, 0);
            this.simpleOpenGlControl1.Name = "simpleOpenGlControl1";
            this.simpleOpenGlControl1.Size = new System.Drawing.Size(725, 515);
            this.simpleOpenGlControl1.StencilBits = ((byte)(0));
            this.simpleOpenGlControl1.TabIndex = 0;
            this.simpleOpenGlControl1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.simpleOpenGlControl1_MouseWheel);
            this.simpleOpenGlControl1.Load += new System.EventHandler(this.simpleOpenGlControl1_Load);
            this.simpleOpenGlControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.simpleOpenGlControl1_MouseDown);
            this.simpleOpenGlControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.simpleOpenGlControl1_MouseMove);
            this.simpleOpenGlControl1.Resize += new System.EventHandler(this.simpleOpenGlControl1_Resize);
            this.simpleOpenGlControl1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.simpleOpenGlControl1_KeyPress);
            this.simpleOpenGlControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.simpleOpenGlControl1_MouseUp);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 505);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(725, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_newSimulation,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(725, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButton_newSimulation
            // 
            this.toolStripButton_newSimulation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_newSimulation.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_newSimulation.Image")));
            this.toolStripButton_newSimulation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_newSimulation.Name = "toolStripButton_newSimulation";
            this.toolStripButton_newSimulation.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_newSimulation.Text = "Nuovo";
            this.toolStripButton_newSimulation.Click += new System.EventHandler(this.toolStripButton_newSimulation_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 527);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.simpleOpenGlControl1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private Tao.Platform.Windows.SimpleOpenGlControl simpleOpenGlControl1;
        private System.Windows.Forms.ToolStripMenuItem simulazioneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem luceAmbientaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem giornostandardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salvaToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_newSimulation;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;


    }
}