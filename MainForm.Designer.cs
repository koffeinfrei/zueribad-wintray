namespace Koffeinfrei.Zueribad
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tray = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemBaths = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.textTitle = new System.Windows.Forms.Label();
            this.labelTemperature = new System.Windows.Forms.Label();
            this.textTemperature = new System.Windows.Forms.Label();
            this.pictureHeader = new System.Windows.Forms.PictureBox();
            this.textStatus = new System.Windows.Forms.Label();
            this.textModified = new System.Windows.Forms.Label();
            this.labelModified = new System.Windows.Forms.Label();
            this.linkHomepage = new System.Windows.Forms.Label();
            this.dataWorker = new System.ComponentModel.BackgroundWorker();
            this.dataUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.trayMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // tray
            // 
            this.tray.ContextMenuStrip = this.trayMenu;
            this.tray.Icon = ((System.Drawing.Icon)(resources.GetObject("tray.Icon")));
            this.tray.Visible = true;
            this.tray.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tray_MouseClick);
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemBaths,
            this.menuItemSettings,
            this.menuItemQuit});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(117, 70);
            // 
            // menuItemBaths
            // 
            this.menuItemBaths.Image = global::Koffeinfrei.Zueribad.Properties.Resources.baths;
            this.menuItemBaths.Name = "menuItemBaths";
            this.menuItemBaths.Size = new System.Drawing.Size(116, 22);
            this.menuItemBaths.Text = "Baths";
            // 
            // menuItemSettings
            // 
            this.menuItemSettings.Image = global::Koffeinfrei.Zueribad.Properties.Resources.settings;
            this.menuItemSettings.Name = "menuItemSettings";
            this.menuItemSettings.Size = new System.Drawing.Size(116, 22);
            this.menuItemSettings.Text = "Settings";
            this.menuItemSettings.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // menuItemQuit
            // 
            this.menuItemQuit.Image = global::Koffeinfrei.Zueribad.Properties.Resources.exit;
            this.menuItemQuit.Name = "menuItemQuit";
            this.menuItemQuit.Size = new System.Drawing.Size(116, 22);
            this.menuItemQuit.Text = "Exit";
            this.menuItemQuit.Click += new System.EventHandler(this.menuItemQuit_Click);
            // 
            // textTitle
            // 
            this.textTitle.AutoSize = true;
            this.textTitle.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textTitle.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textTitle.ForeColor = System.Drawing.Color.Silver;
            this.textTitle.Location = new System.Drawing.Point(12, 75);
            this.textTitle.Name = "textTitle";
            this.textTitle.Size = new System.Drawing.Size(77, 19);
            this.textTitle.TabIndex = 2;
            this.textTitle.Text = "labelTitle";
            // 
            // labelTemperature
            // 
            this.labelTemperature.AutoSize = true;
            this.labelTemperature.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.labelTemperature.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTemperature.ForeColor = System.Drawing.Color.Silver;
            this.labelTemperature.Location = new System.Drawing.Point(12, 107);
            this.labelTemperature.Name = "labelTemperature";
            this.labelTemperature.Size = new System.Drawing.Size(95, 18);
            this.labelTemperature.TabIndex = 3;
            this.labelTemperature.Text = "temperature";
            // 
            // textTemperature
            // 
            this.textTemperature.AutoSize = true;
            this.textTemperature.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textTemperature.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textTemperature.ForeColor = System.Drawing.Color.White;
            this.textTemperature.Location = new System.Drawing.Point(222, 107);
            this.textTemperature.MaximumSize = new System.Drawing.Size(50, 0);
            this.textTemperature.MinimumSize = new System.Drawing.Size(50, 0);
            this.textTemperature.Name = "textTemperature";
            this.textTemperature.Size = new System.Drawing.Size(50, 18);
            this.textTemperature.TabIndex = 4;
            this.textTemperature.Text = "-";
            this.textTemperature.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pictureHeader
            // 
            this.pictureHeader.Image = global::Koffeinfrei.Zueribad.Properties.Resources.background;
            this.pictureHeader.InitialImage = null;
            this.pictureHeader.Location = new System.Drawing.Point(0, 0);
            this.pictureHeader.Name = "pictureHeader";
            this.pictureHeader.Size = new System.Drawing.Size(286, 266);
            this.pictureHeader.TabIndex = 1;
            this.pictureHeader.TabStop = false;
            // 
            // textStatus
            // 
            this.textStatus.AutoSize = true;
            this.textStatus.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textStatus.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textStatus.ForeColor = System.Drawing.Color.White;
            this.textStatus.Location = new System.Drawing.Point(12, 138);
            this.textStatus.MaximumSize = new System.Drawing.Size(350, 0);
            this.textStatus.MinimumSize = new System.Drawing.Size(50, 0);
            this.textStatus.Name = "textStatus";
            this.textStatus.Size = new System.Drawing.Size(50, 18);
            this.textStatus.TabIndex = 7;
            this.textStatus.Text = "-";
            // 
            // textModified
            // 
            this.textModified.AutoSize = true;
            this.textModified.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textModified.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textModified.ForeColor = System.Drawing.Color.White;
            this.textModified.Location = new System.Drawing.Point(122, 241);
            this.textModified.MaximumSize = new System.Drawing.Size(150, 0);
            this.textModified.MinimumSize = new System.Drawing.Size(150, 0);
            this.textModified.Name = "textModified";
            this.textModified.Size = new System.Drawing.Size(150, 14);
            this.textModified.TabIndex = 8;
            this.textModified.Text = "-";
            this.textModified.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelModified
            // 
            this.labelModified.AutoSize = true;
            this.labelModified.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.labelModified.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelModified.ForeColor = System.Drawing.Color.Silver;
            this.labelModified.Location = new System.Drawing.Point(12, 241);
            this.labelModified.Name = "labelModified";
            this.labelModified.Size = new System.Drawing.Size(77, 14);
            this.labelModified.TabIndex = 9;
            this.labelModified.Text = "temperature";
            // 
            // linkHomepage
            // 
            this.linkHomepage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHomepage.Image = global::Koffeinfrei.Zueribad.Properties.Resources.homepage;
            this.linkHomepage.Location = new System.Drawing.Point(13, 194);
            this.linkHomepage.Name = "linkHomepage";
            this.linkHomepage.Size = new System.Drawing.Size(32, 32);
            this.linkHomepage.TabIndex = 10;
            this.linkHomepage.Click += new System.EventHandler(this.linkHomepage_Click);
            // 
            // dataWorker
            // 
            this.dataWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.dataWorker_DoWork);
            this.dataWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.dataWorker_RunWorkerCompleted);
            // 
            // dataUpdateTimer
            // 
            this.dataUpdateTimer.Enabled = true;
            this.dataUpdateTimer.Interval = global::Koffeinfrei.Zueribad.Settings.Default.DataUpdateInterval;
            this.dataUpdateTimer.Tick += new System.EventHandler(this.dataUpdateTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.ControlBox = false;
            this.Controls.Add(this.labelModified);
            this.Controls.Add(this.linkHomepage);
            this.Controls.Add(this.textStatus);
            this.Controls.Add(this.textModified);
            this.Controls.Add(this.labelTemperature);
            this.Controls.Add(this.textTemperature);
            this.Controls.Add(this.textTitle);
            this.Controls.Add(this.pictureHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Opacity = 0D;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.trayMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureHeader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.NotifyIcon tray;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItemQuit;
        private System.Windows.Forms.ToolStripMenuItem menuItemBaths;
        private System.Windows.Forms.PictureBox pictureHeader;
        private System.Windows.Forms.Label textTitle;
        private System.Windows.Forms.Label labelTemperature;
        private System.Windows.Forms.Label textTemperature;
        private System.Windows.Forms.Label textStatus;
        private System.Windows.Forms.Label textModified;
        private System.Windows.Forms.Label labelModified;
        private System.Windows.Forms.Label linkHomepage;
        private System.ComponentModel.BackgroundWorker dataWorker;
        private System.Windows.Forms.Timer dataUpdateTimer;
        private System.Windows.Forms.ToolStripMenuItem menuItemSettings;
    }
}

