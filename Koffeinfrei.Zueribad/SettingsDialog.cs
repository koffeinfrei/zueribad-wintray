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
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Koffeinfrei.Zueribad.Core;
using Koffeinfrei.Zueribad.Model;
using Koffeinfrei.Zueribad.Properties;
using Koffeinfrei.Zueribad.UI;

namespace Koffeinfrei.Zueribad
{
    public partial class SettingsDialog : Form
    {
        private readonly GlobalErrorProvider errorProvider;
        
        private readonly List<Bath> baths;
        private ReadOnlyCollection<CultureInfo> availableCultures;

        private Color fontColor;
        private Color backColor;

        private bool isLoadingSettings;

        public SettingsDialog(List<Bath> baths)
        {
            errorProvider = new GlobalErrorProvider {Icon = Resources.error};

            this.baths = baths;

            InitializeComponent();
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {
            // favorite bath
            comboFavoriteBath.DataSource = baths.Select(x => x.Title).ToList();

            // available languages
            availableCultures = CultureInfoExtensions.GetAvailableCultures();
            comboLanguages.DataSource = availableCultures
                .Select(x => x.NativeName)
                //.Select(x => x.IsNeutralCulture ? x.NativeName : x.Parent.NativeName)
                .ToList();

            Localize();

            LoadSettings();
        }

        private void buttonChooseFontColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                fontColor = colorDialog.Color;
                textFontColor.Text = fontColor.GetHexCode();
            }
        }

        private void buttonChooseBackColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                backColor = colorDialog.Color;
                textBackColor.Text = backColor.GetHexCode();
            }
        }

        private void textFontColor_TextChanged(object sender, EventArgs e)
        {
            ValidateColorHexValue(textFontColor);
            fontColor = ReloadColor(textFontColor, textFontColorAlpha, pictureFontColorPreview, fontColor);
        }

        private void textBackColor_TextChanged(object sender, EventArgs e)
        {
            ValidateColorHexValue(textBackColor);
            backColor = ReloadColor(textBackColor, textBackColorAlpha, pictureBackColorPreview, backColor);
        }

        private void textFontColorAlpha_TextChanged(object sender, EventArgs e)
        {
            ValidateColorAlphaValue(textFontColorAlpha);
            fontColor = ReloadColor(textFontColor, textFontColorAlpha, pictureFontColorPreview, fontColor);
        }

        private void textBackColorAlpha_TextChanged(object sender, EventArgs e)
        {
            ValidateColorAlphaValue(textBackColorAlpha);
            backColor = ReloadColor(textBackColor, textBackColorAlpha, pictureBackColorPreview, backColor);
        }

        private void textUpdateInterval_TextChanged(object sender, EventArgs e)
        {
            int result;
            bool validValue = int.TryParse(textUpdateInterval.Text, out result);
            if (!validValue)
            {
                errorProvider.SetError(textUpdateInterval, Resources.ErrorNumber);
            }
            else
            {
                errorProvider.RemoveError(textUpdateInterval);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!errorProvider.HasErrors())
            {
                int updateInterval = (int) (float.Parse(textUpdateInterval.Text)*60*1000);

                Settings.Default.DataUpdateInterval = updateInterval;
                Settings.Default.FontColor = fontColor.ToArgb();
                Settings.Default.BackgroundColor = backColor.ToArgb();
                Settings.Default.FavoriteBath = comboFavoriteBath.SelectedItem.ToString();
                Settings.Default.Language =
                    availableCultures.Where(x => x.NativeName == comboLanguages.SelectedItem.ToString()).Single().LCID;
                Settings.Default.CheckForUpdates = checkBoxUpdateCheck.Checked;

                Settings.Default.Save();

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            // backup settings
            int language = Settings.Default.Language;
            int dataUpdateInterval = Settings.Default.DataUpdateInterval;
            string favoriteBath = Settings.Default.FavoriteBath;
            int fontColorTmp = Settings.Default.FontColor;
            int backolorTmp = Settings.Default.BackgroundColor;
            bool checkForUpdates = Settings.Default.CheckForUpdates;

            Settings.Default.Reset();
            LoadSettings();

            // restore to file
            Settings.Default.Language = language;
            Settings.Default.DataUpdateInterval = dataUpdateInterval;
            Settings.Default.FavoriteBath = favoriteBath;
            Settings.Default.FontColor = fontColorTmp;
            Settings.Default.BackgroundColor = backolorTmp;
            Settings.Default.CheckForUpdates = checkForUpdates;
            Settings.Default.Save();
        }

        private void ValidateColorAlphaValue(MaskedTextBox alphaBox)
        {
            float result;
            bool validFloat = float.TryParse(alphaBox.Text, out result);
            if (!validFloat || result > 1.0 || result < 0.0)
            {
                errorProvider.SetError(alphaBox, Resources.ErrorAlpha);
            }
            else
            {
                errorProvider.RemoveError(alphaBox);
            }
        }

        private void ValidateColorHexValue(MaskedTextBox colorHexBox)
        {
            bool isMatch = Regex.IsMatch(colorHexBox.Text, "[g-z]");
            if (isMatch)
            {
                errorProvider.SetError(colorHexBox, Resources.ErrorHexCode);
            }
            else
            {
                errorProvider.RemoveError(colorHexBox);
            }
        }

        /// <summary>
        ///   Reloads from the color/alpha text boxes and sets the preview image.
        ///   Returns the new color if the validation of the input fields succeeds, else
        ///   the original <paramref name = "color" />.
        /// </summary>
        /// <param name = "colorBox">The color box.</param>
        /// <param name = "alphaBox">The alpha box.</param>
        /// <param name = "preview">The preview.</param>
        /// <param name = "color">The color.</param>
        private Color ReloadColor(MaskedTextBox colorBox, MaskedTextBox alphaBox, PictureBox preview, Color color)
        {
            if (!isLoadingSettings && !errorProvider.HasError(colorBox) && !errorProvider.HasError(alphaBox))
            {
                color = ColorExtensions.FromHexCodeAndAlpha(colorBox.Text, alphaBox.Text);
                SetPreviewImage(preview, color);
            }
            return color;
        }

        private static void SetPreviewImage(PictureBox previewBox, Color color)
        {
            Bitmap previewImage = color.CreateImage(previewBox.Width, previewBox.Height);

            if (previewBox.Image != null)
            {
                previewBox.Image.Dispose();
            }
            previewBox.Image = previewImage;
        }

        private void Localize()
        {
            labelFontColor.Text = Resources.LabelSettingsFontColor;
            labelBackColor.Text = Resources.LabelSettingsBackColor;
            labelUpdateInterval.Text = Resources.LabelSettingsUpdateInterval;
            labelLanguage.Text = Resources.LabelSettingsLanguage;
            labelFavoriteBath.Text = Resources.LabelSettingsFavoriteBath;
            labelUpdateCheck.Text = Resources.LabelUpdates;
            checkBoxUpdateCheck.Text = Resources.LabelCheckUpdates;

            buttonSave.Text = Resources.ButtonSave;
            buttonCancel.Text = Resources.ButtonCancel;
            buttonReset.Text = Resources.ButtonReset;

            labelUpdateIntervalMinutes.Text = Resources.LabelMinutesAbbreviation;
        }

        private void LoadSettings()
        {
            isLoadingSettings = true;


            if (Settings.Default.Language > 0)
            {
                comboLanguages.SelectedItem = new CultureInfo(Settings.Default.Language).NativeName;
            }
            else
            {
                comboLanguages.SelectedIndex = 0;
            }
            
            textUpdateInterval.Text = (Settings.Default.DataUpdateInterval/60.0/1000.0).ToString();
            if (!string.IsNullOrEmpty(Settings.Default.FavoriteBath))
            {
                comboFavoriteBath.SelectedItem = Settings.Default.FavoriteBath;
            }
            else
            {
                comboFavoriteBath.SelectedIndex = 0;
            }

            checkBoxUpdateCheck.Checked = Settings.Default.CheckForUpdates;

            fontColor = Color.FromArgb(Settings.Default.FontColor);
            backColor = Color.FromArgb(Settings.Default.BackgroundColor);

            // populate colors
            SetPreviewImage(pictureFontColorPreview, fontColor);
            SetPreviewImage(pictureBackColorPreview, backColor);
            textFontColor.Text = fontColor.GetHexCode();
            textBackColor.Text = backColor.GetHexCode();
            textFontColorAlpha.Text = (fontColor.A/255.0).ToString();
            textBackColorAlpha.Text = (backColor.A/255.0).ToString();

            
            isLoadingSettings = false;
        }
    }
}