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
        public Font Font { get; set; }
        public Color FontColor { get; set; }
        public Color BackgroundColor { get; set; }

        [DllImport("user32.dll", EntryPoint = "DestroyIcon")]
        private static extern bool DestroyIcon(IntPtr hIcon);

        private const int Height = 16;
        private const int Width = 16;

        private readonly NotifyIcon icon;

        private List<Bitmap> animatedPictures;
        private int currentAnimatedPicture;
        private Timer animatedTimer;

        // TODO don't pass the notifyicon ref, make callback or something
        public TrayIcon(NotifyIcon icon, Font font, Color fontColor, Color backgroundColor)
        {
            this.icon = icon;
            Font = font;
            FontColor = fontColor;
            BackgroundColor = backgroundColor;
        }

        public void SetStatic(string text)
        {
            StopTimer();

            string temperature = string.IsNullOrEmpty(text) ? "-" : text;

            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            IntPtr hIcon;
            using (Bitmap bitmap = new Bitmap(Width, Height))
            {
                using (SolidBrush brush = new SolidBrush(FontColor))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.FillRectangle(new SolidBrush(BackgroundColor), 0, 0, Width, Height);
                        graphics.TextContrast = 0; // high contrast (0-12)
                        graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                        graphics.DrawString(temperature, Font, brush, Width/2, Height/2, stringFormat);
                    }
                }

                hIcon = bitmap.GetHicon();
            }
            icon.Icon = Icon.FromHandle(hIcon);

            // manually destroy the unmanaged handle created by GetHicon
            DestroyIcon(hIcon);
        }

        public void SetAnimated(params Bitmap[] fileNames)
        {
            StopTimer();

            if (fileNames != null)
            {
                animatedPictures = new List<Bitmap>(fileNames);

                animatedTimer = new Timer {Interval = 300};
                animatedTimer.Tick += timer_Tick;
                animatedTimer.Start();
            }
        }

        private void StopTimer()
        {
            if (animatedTimer != null && animatedTimer.Enabled)
            {
                animatedTimer.Stop();
                animatedTimer.Dispose();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            IntPtr hIcon = animatedPictures[currentAnimatedPicture].GetHicon();
            icon.Icon = Icon.FromHandle(hIcon);

            // manually destroy the unmanaged handle created by GetHicon
            DestroyIcon(hIcon);

            ++currentAnimatedPicture;
            if (currentAnimatedPicture >= animatedPictures.Count)
            {
                currentAnimatedPicture = 0;
            }
        }
    }
}