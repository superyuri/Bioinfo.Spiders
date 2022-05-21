using PlaywrightSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Net.Http;
using NLog;

namespace BMRBSpider.Consoles
{
    class Step1
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public async Task Function()
        {
            await Playwright.InstallAsync();
            using var playwright = await Playwright.CreateAsync();
            //await using var browser = await playwright.Chromium.LaunchAsync(headless: false);
            await using var browser = await playwright.Chromium.LaunchAsync(headless: true);
            var context = await browser.NewContextAsync();

            var bmrbIds = Business.GetAllPDBIdsFromLocalCSV();
            HttpClient client = new HttpClient();
            using (var writer = new StreamWriter("output/Result.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                foreach (var bmrbId in bmrbIds)
                {
                    var rmdnResult = await Business.GetRelatedPDBDataFromBMRBData(context, bmrbId);
                    var pdbFileLink = Business.GetPDBFileLinkFromRCSBDataFast(rmdnResult.Item1);
                    PDBEntity result = null;
                    if (pdbFileLink.Item1!=null)
                    {
                        var filename = $"output/pdb/{rmdnResult.Item1}_{bmrbId}.pdb";
                        using (HttpResponseMessage response = await client.GetAsync(pdbFileLink.Item1))
                        using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                        using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                        {
                            await streamToReadFrom.CopyToAsync(stream);
                        }
                        result = new PDBEntity
                        {
                            BMRBId = bmrbId,
                            PDBId = rmdnResult.Item1,
                            RCSBFileLink = pdbFileLink.Item1,
                            BMRBUrl = rmdnResult.Item2,
                            RCSBUrl = pdbFileLink.Item2,
                            Filename = filename
                        };
                    }
                    else
                    {
                        result = new PDBEntity
                        {
                            BMRBId = bmrbId,
                            PDBId = rmdnResult.Item1,
                            RCSBFileLink = pdbFileLink.Item1,
                            BMRBUrl = rmdnResult.Item2,
                            RCSBUrl = pdbFileLink.Item2,
                        };
                    }

                    Console.WriteLine($"{bmrbId},{rmdnResult.Item1}");
                    logger.Info<PDBEntity>(result);
                    csv.WriteRecord(result);
                    csv.NextRecord();
                    csv.Flush();
                }
            }
        }
    }
}
