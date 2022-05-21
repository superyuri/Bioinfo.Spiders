using PlaywrightSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Globalization;
using CsvHelper;

namespace BMRBSpider.Consoles
{
    public class Business
    {
        public static async Task<string[]> GetAllPDBIdsFromBMRBData(IBrowserContext context)
        {
            var page = await context.NewPageAsync();
            await page.GoToAsync($"https://bmrb.io/search/simplesearch.php?bmrbid=3&show_bmrbid=on&dbname=PDB&pdbid=&title=&author=&molecule=&output=html");
            var elements = await page.QuerySelectorAllAsync("//*[@id=\"bmrb_pagecontentcolumn\"]//tr[@class=\"hiliteonhover\"]");

            var results = await Task.WhenAll(elements.Select(async element =>
            {
                return await element.GetInnerTextAsync();
            }));
            await page.CloseAsync();
            return results;
        }
        public static async Task<string[]> GetAllPDBIdsFromBMRBDataCSV(IBrowserContext context)
        {
            HttpClient client = new HttpClient();

            using (HttpResponseMessage response = await client.GetAsync("https://bmrb.io/search/simplesearch.php?bmrbid=3&show_bmrbid=on&dbname=PDB&pdbid=&title=&author=&molecule=&output=csv"))
            using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(streamToReadFrom))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<BMRBID>();
                return records.Select(e => e.BMRBId).ToArray();
            }
        }
        public static string[] GetAllPDBIdsFromLocalCSV()
        {
            return SerlizeHelper.LoadData<BMRBID>("pedding.csv").Select(e => e.BMRBId).ToArray();
        }
        public static async Task<Tuple<string, string>> GetRelatedPDBDataFromBMRBData(IBrowserContext context, string id)
        {
            var page = await context.NewPageAsync();
            string url = $"https://bmrb.io/data_library/summary/index.php?bmrbId={id}";
            await page.GoToAsync(url);
            var element = await page.QuerySelectorAsync("//td[contains(text(),'PDB')]/following-sibling::*[position()=1]//a[1]");
            string result = null;
            if (element!=null)
            {
                result = await element.GetInnerTextAsync();
            }
            await page.CloseAsync();
            return new Tuple<string, string>(result, url);
        }
        public static async Task<string> GetPDBFileLinkFromRCSBData(IBrowserContext context, string id)
        {
            throw new NotImplementedException();
            //var page = await context.NewPageAsync();
            //await page.GoToAsync($"http://www.rcsb.org/pdb/explore.do?structureId={id}");
            //var linkElement = await page.QuerySelectorAsync("//ul[@aria-labelledby='dropdownMenuDownloadFiles']//a[normalize-space(.)='PDB Format']");

            ////var temp = Debug(linkPrep);

            ////var temp = Debug(linkPrep);
            //return await linkElement.GetTextContentAsync();
        }
        public static Tuple<string, string> GetPDBFileLinkFromRCSBDataFast(string id)
        {
            if (id!=null)
            {
                string url = $"http://www.rcsb.org/pdb/explore.do?structureId={id}";

                string fileLink = $"https://files.rcsb.org/download/{id}.pdb";
                return new Tuple<string, string>(fileLink, url);
            }
            else
            {
                return new Tuple<string, string>(null, null);
            }
        }

        //public static async Task<string> GetLinkFromLinkElement(IElementHandle hanlder)
        //{
        //    var linkPrep = await hanlder.GetPropertyAsync("href");
        //    linkPrep.ToString().Remove("")
        //}
        public static object Debug(IElementHandle hanlder)
        {
            if (hanlder == default(IElementHandle))
            {
                return null;
            }
            try
            {
                var GetPropertiesAsync = hanlder.GetPropertiesAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                //string GetJsonValueAsync = hanlder.GetJsonValueAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                string GetTextContentAsync = hanlder.GetTextContentAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                string GetInnerHtmlAsync = hanlder.GetInnerHtmlAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                string GetInnerTextAsync = hanlder.GetInnerTextAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                return new
                {
                    GetPropertiesAsync = GetPropertiesAsync,
                    //GetJsonValueAsync = GetJsonValueAsync,
                    GetTextContentAsync = GetTextContentAsync,
                    GetInnerHtmlAsync = GetInnerHtmlAsync,
                    GetInnerTextAsync = GetInnerTextAsync,
                    //GetOwnerFrameAsync = hanlder.GetOwnerFrameAsync().ConfigureAwait(false).GetAwaiter().GetResult(),
                };
            }
            catch (Exception)
            {
                return null;
            }
            //return System.Text.Json.JsonSerializer.Serialize();
        }
    }
}
