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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Koffeinfrei.Zueribad
{
    public class DataService
    {
        private readonly string xmlUrl;

        public DataService(string xmlUrl)
        {
            this.xmlUrl = xmlUrl;
        }

        public List<Bath> Load()
        {
            List<Bath> baths = null;
            string tempFileName = Path.GetTempFileName();
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("Accept", "text/xml");
                    client.Headers.Add("User-Agent", "Koffeinfrei.Zueribad");
                    client.Encoding = Encoding.ASCII;
                    client.DownloadFile(xmlUrl, tempFileName);
                }

                XDocument document = XDocument.Load(tempFileName);
                baths = (from bathElements in document.Descendants("baths").Descendants("bath")
                         select new Bath
                         {
                             TemperatureWater = bathElements.Element("temperatureWater").Value.Trim(),
                             Title = bathElements.Element("title").Value.Trim()
                         }
                        ).ToList();
            }
            catch (WebException e)
            {
                //TODO
            }
            finally
            {
                File.Delete(tempFileName);
            }

            return baths ?? new List<Bath>();
        }
    }
}