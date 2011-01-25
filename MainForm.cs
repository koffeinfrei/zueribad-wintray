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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MainForm" /> class.
        /// </summary>
        public MainForm()
        {
            dataService = new DataService(Settings.Default.DataFile);

            InitializeComponent();
        }

        /// <summary>
        ///   Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name = "sender">The source of the event.</param>
        /// <param name = "e">The <see cref = "System.EventArgs" /> instance containing the event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            baths = dataService.Load();

            InitBaths();
            UpdateCurrentBath();

            SetWindowPosition();
            Visible = false;
            Hide();
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
                // TODO open main window
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

        /// <summary>
        /// Populates the available baths into the context menu
        /// </summary>
        private void InitBaths()
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
        /// Updates the current bath. i.e. updates the check mark in the menu,
        /// the info in the main window and the tray icon.
        /// </summary>
        private void UpdateCurrentBath()
        {
            // menu: uncheck all others, check the current
            foreach (var item in menuItemBaths.DropDownItems)
            {
                ((ToolStripMenuItem)item).Checked = false;
            }
            ((ToolStripMenuItem)menuItemBaths.DropDownItems[currentBathIndex]).Checked = true;

            // icon
            FontDialog fontDialog = new FontDialog();

            TrayIcon icon = new TrayIcon(tray, fontDialog.Font, Color.WhiteSmoke, Color.Transparent);
            icon.Set(baths[currentBathIndex].TemperatureWater);
        }
    }
}