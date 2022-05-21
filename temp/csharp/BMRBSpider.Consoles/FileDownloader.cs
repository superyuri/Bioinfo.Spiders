using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BMRBSpider.Consoles
{
    public class FileDownloader
    {
        static HttpClient client = new HttpClient();
        public static async Task DownloadAsync(string url,string path,bool skipIfExists = true)
        {
            if (skipIfExists && !File.Exists(path))
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                {
                    await streamToReadFrom.CopyToAsync(stream);
                }
            }
        }
    }
}
