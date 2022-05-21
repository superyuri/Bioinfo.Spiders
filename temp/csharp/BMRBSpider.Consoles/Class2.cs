﻿//using DotnetSpider;
//using DotnetSpider.DataFlow.Parser;
//using DotnetSpider.Http;
//using DotnetSpider.Scheduler.Component;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace BMRBSpider.Consoles
//{
//    class Class2
//    {
//        public async Task Function()
//        {
            
//        }

//		public class EntitySpider : Spider
//		{
//			public static async Task RunAsync()
//			{
//				var builder = Builder.CreateDefaultBuilder<EntitySpider>();
//				//builder.UseSerilog();
//				builder.UseQueueDistinctBfsScheduler<HashSetDuplicateRemover>();
//				await builder.Build().RunAsync();
//			}

//			public EntitySpider(IOptions<SpiderOptions> options, SpiderServices services, ILogger<Spider> logger) : base(
//				options, services, logger)
//			{
//			}

//			protected override async Task InitializeAsync(CancellationToken stoppingToken)
//			{
//				AddDataFlow(new DataParser<CnblogsEntry>());
//				AddDataFlow(GetDefaultStorage());
//				await AddRequestsAsync(
//					new Request("https://news.cnblogs.com/n/page/1/", new Dictionary<string, string> { { "网站", "博客园" } }),
//					new Request("https://news.cnblogs.com/n/page/2/", new Dictionary<string, string> { { "网站", "博客园" } }));
//			}

//			protected override (string Id, string Name) GetIdAndName()
//			{
//				return (ObjectId.NewId.ToString(), "博客园");
//			}

//            protected override Task InitializeAsync(CancellationToken stoppingToken)
//            {
//                throw new NotImplementedException();
//            }

//            [Schema("cnblogs", "news")]
//			[EntitySelector(Expression = ".//div[@class='news_block']", Type = SelectorType.XPath)]
//			[GlobalValueSelector(Expression = ".//a[@class='current']", Name = "类别", Type = SelectorType.XPath)]
//			[FollowRequestSelector(XPaths = new[] { "//div[@class='pager']" })]
//			public class CnblogsEntry : EntityBase<CnblogsEntry>
//			{
//				protected override void Configure()
//				{
//					HasIndex(x => x.Title);
//					HasIndex(x => new { x.WebSite, x.Guid }, true);
//				}

//				public int Id { get; set; }

//				[Required]
//				[StringLength(200)]
//				[ValueSelector(Expression = "类别", Type = SelectorType.Environment)]
//				public string Category { get; set; }

//				[Required]
//				[StringLength(200)]
//				[ValueSelector(Expression = "网站", Type = SelectorType.Environment)]
//				public string WebSite { get; set; }

//				[StringLength(200)]
//				[ValueSelector(Expression = "//title")]
//				[ReplaceFormatter(NewValue = "", OldValue = " - 博客园")]
//				public string Title { get; set; }

//				[StringLength(40)]
//				[ValueSelector(Expression = "GUID", Type = SelectorType.Environment)]
//				public string Guid { get; set; }

//				[ValueSelector(Expression = ".//h2[@class='news_entry']/a")]
//				public string News { get; set; }

//				[ValueSelector(Expression = ".//h2[@class='news_entry']/a/@href")]
//				public string Url { get; set; }

//				[ValueSelector(Expression = ".//div[@class='entry_summary']")]
//				public string PlainText { get; set; }

//				[ValueSelector(Expression = "DATETIME", Type = SelectorType.Environment)]
//				public DateTime CreationTime { get; set; }
//			}
//		}
//	}
//}
