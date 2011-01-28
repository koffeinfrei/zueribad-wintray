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

using System.Collections.Generic;
using System.Windows.Forms;

namespace Koffeinfrei.Zueribad
{
    /// <summary>
    /// Enhances the <see cref="ErrorProvider"/> with the ability to
    /// track all open errors. Additionally, the api to add and remove
    /// errors is improved.
    /// </summary>
    public class GlobalErrorProvider : ErrorProvider
    {
        private readonly Dictionary<Control, string> controlError;

        public GlobalErrorProvider()
        {
            controlError = new Dictionary<Control, string>();
        }

        public void RemoveError(Control control)
        {
            controlError.Remove(control);
            base.SetError(control, null);
        }

        public new void SetError(Control control, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                RemoveError(control);
            }
            else
            {
                controlError[control] = value;
            }

            base.SetError(control, value);
        }

        public bool HasError(Control control)
        {
            return controlError.ContainsKey(control);
        }

        public bool HasErrors()
        {
            return controlError.Count > 0;
        }
    }
}