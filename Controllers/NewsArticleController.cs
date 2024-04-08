using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using TimesNewsApi.Models;

namespace TimesNewsApi.Controllers
{
    public class NewsArticleController : ApiController
    {
        public async Task<string> Get()
        {
            string htmlCode = "";
            using (HttpClient client = new HttpClient())
            {
                htmlCode = await client.GetStringAsync("https://time.com/");
            }

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlCode);

            List<NewsArticles> articles = new List<NewsArticles>();
            var latestStoriesNodes = doc.DocumentNode.SelectNodes("//li[@class='latest-stories__item']");

            for (int i = 0; i < Math.Min(6, latestStoriesNodes.Count); i++)
            {
                var storyNode = latestStoriesNodes[i];

                string link = storyNode.SelectSingleNode(".//a[@href]").Attributes["href"].Value;
                string title = storyNode.SelectSingleNode(".//h3[@class='latest-stories__item-headline']").InnerText;

                articles.Add(new NewsArticles { Title = title, Link = "https://time.com" + link });
            }

            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(articles);
        }
    }
}
