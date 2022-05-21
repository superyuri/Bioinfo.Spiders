using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;

namespace BMRBSpider.Consoles
{
    public class SerlizeHelper
    {
        public static IEnumerable<T> LoadData<T>(string filename) where T:class
        {
            using (var streamToReadFrom = new FileStream(filename, FileMode.Open))
            using (var reader = new StreamReader(streamToReadFrom))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<T>();
                return records.ToList();
            }
        }
    }
}
