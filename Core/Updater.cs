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
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Koffeinfrei.Zueribad.Properties;

namespace Koffeinfrei.Zueribad.Core
{
    public class Updater
    {
        private bool? hasNewerVersion;

        public string NewerVersion { get; set; }

        public bool HasNewerVersion()
        {
            if (!hasNewerVersion.HasValue)
            {

                string serverVersion = new WebClient().DownloadString(Settings.Default.VersionUrl);

                if (Application.ProductVersion != serverVersion)
                {
                    NewerVersion = serverVersion;
                    hasNewerVersion = true;
                }
                else
                {
                    hasNewerVersion = false;
                }
            }
            return hasNewerVersion.Value;
        }

        public void Update()
        {
            if (!string.IsNullOrEmpty(NewerVersion))
            {
                Process.Start(string.Format(Settings.Default.DownloadUrlFormat, NewerVersion));
            }
        }
    }
}
