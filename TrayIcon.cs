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
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Koffeinfrei.Zueribad
{
    /// <summary>
    ///   Description of TrayIcon.
    /// </summary>
    public class TrayIcon
    {
        [DllImport("user32.dll", EntryPoint = "DestroyIcon")]
        private static extern bool DestroyIcon(IntPtr hIcon);

        private const int Height = 16;
        private const int Width = 16;

        private readonly NotifyIcon icon;

        public Font Font { get; set; }
        public Color FontColor { get; set; }
        public Color BackgroundColor { get; set; }

        // TODO don't pass the notifyicon ref, make callback or something
        public TrayIcon(NotifyIcon icon, Font font, Color fontColor, Color backgroundColor)
        {
            this.icon = icon;
            Font = font;
            FontColor = fontColor;
            BackgroundColor = backgroundColor;
        }

        public void Set(string text)
        {
            string temperature = string.IsNullOrEmpty(text) ? "-" : text;

            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            Bitmap bitmap = new Bitmap(Width, Height);
            SolidBrush brush = new SolidBrush(FontColor);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(new SolidBrush(BackgroundColor), 0, 0, Width, Height);
            graphics.TextContrast = 0; // high contrast (0-12)
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.DrawString(temperature, Font, brush, Width/2, Height/2, stringFormat);

            IntPtr hIcon = bitmap.GetHicon();
            icon.Icon = Icon.FromHandle(hIcon);

            // manually destroy the unmanaged handle created by GetHicon
            DestroyIcon(hIcon);
        }
    }
}