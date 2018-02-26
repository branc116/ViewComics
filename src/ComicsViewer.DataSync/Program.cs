using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Crawler.DataSync
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Issue> comics = new List<Issue>();

            HtmlWeb web = new HtmlWeb();
            for (int i = 1; i < 50; i++)
            {
                HtmlDocument doc = web.Load("http://viewcomic.com/page/" + i.ToString());

                var nodes = doc.DocumentNode.SelectNodes("//a[@class='front-link']");

                foreach (var item in nodes)
                {
                    var name = item.InnerText.Split('…').First().Trim();
                    var links = pictureLinks(item.Attributes["href"].Value);
                    var newIssue = new Issue
                    {
                        Name = name,
                        Links = links
                    };
                    comics.Add(newIssue);
                }
                Console.WriteLine("Done:" + i.ToString());
            }
            Console.WriteLine("Done");
            System.IO.File.WriteAllText(@"C:\Users\Nikola\Desktop\comic.json", JsonConvert.SerializeObject(comics, Formatting.Indented));
            Console.ReadKey();
        }

        static List<string> pictureLinks(string uri)
        {
            HtmlWeb web = new HtmlWeb();

            var doc = web.Load(uri);

            var links = doc.DocumentNode.SelectNodes("//div[@class='separator']").Descendants("a")
                .Select(i => i.Attributes["href"].Value).ToList();
            return links;


        }
    }

    public class Issue
    {
        public string Name { get; set; }
        public List<string> Links { get; set; }
    }

    public class Comic
    {
        public string Name { get; set; }
        public List<Issue> Issues { get; set; }
        public int Count { get; set; } = 0;

        public Comic(string name)
        {
            Name = name;
        }
    }

}
