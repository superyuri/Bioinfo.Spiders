using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace BMRBSpider.Consoles
{
    public class MyDownloader
    {
        public static async Task Download1()
        {
            var entities = SerlizeHelper.LoadData<PDBEntity>("pdb.csv");

            await Task.WhenAll(entities.Where(e => e.Status.Equals("OK"))
                .AsParallel()
                .WithDegreeOfParallelism(4)
                .Select((e, i) =>
                {
                    FileDownloader.DownloadAsync(e.RCSBFileLink, e.Filename).ConfigureAwait(false).GetAwaiter().GetResult();
                    Console.WriteLine($"Downloaded {e.BMRBId} with {e.Filename}");
                    return Task.CompletedTask;
                }));

            //foreach (var e in entities.Where(e => e.Status.Equals("OK")))
            //{
            //    await FileDownloader.DownloadAsync(e.RCSBFileLink, e.Filename);
            //    Console.WriteLine($"Downloaded {e.BMRBId} with {e.Filename}");
            //}

            //await Task.WhenAll(entities.Where(e => e.Status.Equals("OK")).Select(async e =>
            //{
            //    await FileDownloader.DownloadAsync(e.RCSBFileLink, e.Filename);
            //    Console.WriteLine($"Downloaded {e.BMRBId} with {e.Filename}");
            //}));
            Console.WriteLine("Finished");
        }
    }
}
