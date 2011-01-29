//  Koffeinfrei Zueribad Wintray
//  Copyright (C) 2011  Alexis Reigel
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Koffeinfrei.Zueribad.Core;
using Koffeinfrei.Zueribad.Model;
using Koffeinfrei.Zueribad.Properties;
using Koffeinfrei.Zueribad.UI;

namespace Koffeinfrei.Zueribad
{
    /// <summary>
    ///   The main form, i.e. the main application container. Not a visible window.
    /// </summary>
    public partial class MainForm : DockableForm
    {
        private readonly DataService dataService;
        private List<Bath> baths;
        private int currentBathIndex;
        private readonly TrayIcon trayIcon;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MainForm" /> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            dataService = new DataService(Settings.Default.DataFile);

            trayIcon = new TrayIcon( tray, new FontDialog().Font, Color.Empty, Color.Empty);
        }

        private void LoadSettings()
        {
            trayIcon.FontColor = Color.FromArgb(Settings.Default.FontColor);
            trayIcon.BackgroundColor = Color.FromArgb(Settings.Default.BackgroundColor);
            if (Settings.Default.Language > 0)
            {
                Resources.Culture = new CultureInfo(Settings.Default.Language);
            }
        }

        private void LoadBathSettings()
        {
            if (baths != null && baths.Count > 0)
            {
                currentBathIndex = Math.Max(0, baths.FindIndex(x => x.Title == Settings.Default.FavoriteBath));

                UpdateCurrentBath();
            }
        }

        /// <summary>
        ///   Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name = "sender">The source of the event.</param>
        /// <param name = "e">The <see cref = "System.EventArgs" /> instance containing the event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Settings.Default.CheckForUpdates)
            {
                versionUpdateWorker.RunWorkerAsync();
            }

            UpdateBathData();

            SetWindowPosition();
            WindowState = FormWindowState.Minimized;
            Visible = false;
            Hide();
            TopMost = true;

            LoadSettings();
            Localize();
            SetTransparentBackground();
        }

        /// <summary>
        ///   Handle the visibility on start.
        /// </summary>
        /// <remarks>
        ///   Initial opacity is 0%, such that on start the window is not visible.
        ///   When initially shown, we hide the window and set its opacity back to 100%.
        ///   This way, we don't have any flickering on startup.
        /// </remarks>
        /// <param name = "sender">The source of the event.</param>
        /// <param name = "e">The <see cref = "System.EventArgs" /> instance containing the event data.</param>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            Hide();
            Opacity = 1;
        }

        /// <summary>
        ///   Handles the MouseClick event of the tray control.
        /// </summary>
        /// <param name = "sender">The source of the event.</param>
        /// <param name = "e">The <see cref = "System.Windows.Forms.MouseEventArgs" /> instance containing the event data.</param>
        private void tray_MouseClick(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button == MouseButtons.Left)
            {
                if (FormWindowState.Minimized == WindowState)
                {
                    Show();
                    WindowState = FormWindowState.Normal;
                }
                else if (FormWindowState.Normal == WindowState)
                {
                    Hide();
                    WindowState = FormWindowState.Minimized;
                }
            }
        }

        /// <summary>
        ///   Handles the Click event of the menuItemQuit control.
        /// </summary>
        /// <param name = "sender">The source of the event.</param>
        /// <param name = "e">The <see cref = "System.EventArgs" /> instance containing the event data.</param>
        private void menuItemQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        ///   Handles the Click event of the menuItemBaths control.
        /// </summary>
        /// <param name = "sender">The source of the event.</param>
        /// <param name = "e">The <see cref = "System.EventArgs" /> instance containing the event data.</param>
        private void menuItemBaths_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            currentBathIndex = baths.IndexOf(baths.Where(x => x.Title == item.Name).SingleOrDefault());
            UpdateCurrentBath();
        }

        /// <summary>
        ///   Handles the FormClosed event of the MainForm control.
        /// </summary>
        /// <param name = "sender">The source of the event.</param>
        /// <param name = "e">The <see cref = "System.Windows.Forms.FormClosedEventArgs" /> instance containing the event data.</param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();
            tray.Dispose();
        }

        private void linkHomepage_Click(object sender, EventArgs e)
        {
            Process.Start(baths[currentBathIndex].Url);
        }

        private void dataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            baths = dataService.Load();
        }

        private void dataWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PopulateBaths();
            LoadBathSettings();
            //UpdateCurrentBath();
        }

        private void dataUpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateBathData();
        }

        /// <summary>
        ///   Updates the bath data, fetches the data from the web service asynchronously.
        /// </summary>
        private void UpdateBathData()
        {
            if (!dataWorker.IsBusy)
            {
                trayIcon.SetAnimated(Resources.loading2, Resources.loading1);
                dataWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        ///   Populates the available baths into the context menu
        /// </summary>
        private void PopulateBaths()
        {
            foreach (Bath bath in baths)
            {
                ToolStripMenuItem item = new ToolStripMenuItem
                {
                    Name = bath.Title,
                    Text = bath.Title
                };
                item.Click += menuItemBaths_Click;

                menuItemBaths.DropDownItems.Add(item);
            }
        }

        /// <summary>
        ///   Updates the current bath. i.e. updates the check mark in the menu,
        ///   the info in the main window and the tray icon.
        /// </summary>
        private void UpdateCurrentBath()
        {
            // menu: uncheck all others, check the current
            foreach (var item in menuItemBaths.DropDownItems)
            {
                ((ToolStripMenuItem) item).Checked = false;
            }
            ((ToolStripMenuItem) menuItemBaths.DropDownItems[currentBathIndex]).Checked = true;

            // icon
            trayIcon.SetStatic(baths[currentBathIndex].TemperatureWater);

            // data in main window
            textTitle.Text = baths[currentBathIndex].Title;
            textTemperature.Text = baths[currentBathIndex].TemperatureWater + " °C";
            textStatus.Text = baths[currentBathIndex].Status;
            textModified.Text = baths[currentBathIndex].Modified;
        }

        private void SetTransparentBackground()
        {
            textTitle.Parent = pictureHeader;
            textTitle.BackColor = Color.Transparent;

            labelTemperature.Parent = pictureHeader;
            labelTemperature.BackColor = Color.Transparent;

            textTemperature.Parent = pictureHeader;
            textTemperature.BackColor = Color.Transparent;

            linkHomepage.Parent = pictureHeader;
            linkHomepage.BackColor = Color.Transparent;
            linkHomepage.ForeColor = Color.Transparent;

            textStatus.Parent = pictureHeader;
            textStatus.BackColor = Color.Transparent;

            labelModified.Parent = pictureHeader;
            labelModified.BackColor = Color.Transparent;

            textModified.Parent = pictureHeader;
            textModified.BackColor = Color.Transparent;
        }

        private void Localize()
        {
            labelTemperature.Text = Resources.LabelWaterTemperatur;
            labelModified.Text = Resources.LabelModified;

            menuItemBaths.Text = Resources.MenuBaths;
            menuItemQuit.Text = Resources.MenuQuit;
            menuItemSettings.Text = Resources.MenuSettings;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsDialog settingsDialog = new SettingsDialog(baths);
            if (settingsDialog.ShowDialog() == DialogResult.OK)
            {
                LoadSettings();
                Localize();
                UpdateCurrentBath();
            }
        }

        private void versionUpdateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Updater updater = new Updater();
            updater.HasNewerVersion();
            e.Result = updater;
        }

        private void versionUpdateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Updater updater = (Updater) e.Result;
            if (updater.HasNewerVersion())
            {
                DialogResult result = MessageBox.Show(
                    string.Format(Resources.DialogVersionUpdateQuestionFormat, updater.NewerVersion),
                    Resources.DialogVersionUpdate,
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    updater.Update();
                }
            }
        }

    }
}