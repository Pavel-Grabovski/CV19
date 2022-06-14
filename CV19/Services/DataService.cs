using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CV19.Models;

namespace CV19.Services
{
    internal class DataService
    {
        private const string _DataSourceAddress = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";

        private static async Task<Stream> GetDataStream()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(
                _DataSourceAddress,
                HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();
        }

        private static IEnumerable<string> GetDataLines()
        {
            using var data_stream = (SynchronizationContext.Current is null ? GetDataStream() : Task.Run(GetDataStream)).Result;
            using var data_reader = new StreamReader(data_stream);

            while (!data_reader.EndOfStream)
            {
                var line = data_reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                //line = line.Replace("Korea,", "Korea -")
                //    .Replace("Bonaire,", "Bonaire -")
                //    .Replace("Helena,", "Helena -");
                //yield return line;

                if (line.Contains('"'))
                    line = line.Insert(line.IndexOf(',', line.IndexOf('"')) + 1, " -").Remove(line.IndexOf(',', line.IndexOf('"')), 1);
                yield return line;
            }
        }

        private static DateTime[] GetDates() => GetDataLines()
           .First()
           .Split(',')
           .Skip(4)
           .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture))
           .ToArray();

        private static IEnumerable<(string Province, string Contry, (double Lat, double Lon) Place, int[] Counts)> GetCountriesData()
        {
            var lines = GetDataLines()
               .Skip(1)
               .Select(line => line.Split(','));

        
            foreach (var row in lines)
            {
                var province = row[0].Trim();
                var country_name = row[1].Trim(' ', '"');

                //NumberStyles style = NumberStyles.AllowDecimalPoint;
                //IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
                //double latitude;
                //double longitude;
                //Double.TryParse(row[2], style, formatter, out latitude);
                //Double.TryParse(row[3], style, formatter, out longitude);

                if(row[2] == "" && row[3] == "")
                {
                    continue;
                }
                var latitude =  Convert.ToDouble(row[2].Replace('.', ','));
                var longitude = Convert.ToDouble(row[2].Replace('.', ','));

                var counts = row.Skip(4).Select(int.Parse).ToArray();

                if (country_name == "Angola")
                {
                    
                }
                yield return (province, country_name, (latitude, longitude), counts);
            }
        }
        public IEnumerable<CountryInfo> GetData()
        {
            var dates = GetDates();
            var data = GetCountriesData().GroupBy(d => d.Contry);
            foreach (var country_info in data)
            {
                var country = new CountryInfo
                {
                    Name = country_info.Key,
                    Provinces = country_info.Select(c => new PlaceInfo
                    {
                        Name = c.Province,
                        Location = new Point(c.Place.Lat, c.Place.Lon),
                        Counts = dates.Zip(c.Counts, (date, count) => new ConfirmedCount { Date = date, Count = count })
                    })
                };
                yield return country;

            }
        }
    }
}
