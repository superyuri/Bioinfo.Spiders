using PlaywrightSharp;
using System;
using System.Drawing;

namespace BMRBSpider.Consoles
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            //await BaseUsageSpider.RunAsync();
            await MyDownloader.Download1();
            //   await new Step1().Function();
            //using var playwright = await Playwright.CreateAsync();
            //await using var browser = await playwright.Chromium.LaunchAsync();

            //var context = await browser.NewContextAsync();
            //var page = await context.NewPageAsync();
            //await page.GoToAsync("https://www.example.com/");
            //var dimensions = await page.EvaluateAsync<Size>(@"() => {
            //    return {
            //        width: document.documentElement.clientWidth,
            //        height: document.documentElement.clientHeight,
            //    }
            //}");
            //Console.WriteLine(dimensions);


        }
    }
}
