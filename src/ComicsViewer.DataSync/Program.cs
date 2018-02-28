using ComicsViewer.Common.Repository;
using ComicsViewer.Common.Resources;
using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using static ComicsViewer.DataSync.Helper.DataHelper;

namespace ComicsViewer.DataSync
{
    public class Program
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly static ComicRepository _comicRepository = GetRepository();
        private readonly static HtmlWeb web = new HtmlWeb();
        private readonly static Comic masterComic = _comicRepository.GetComic("MasterComic");

        private static void Main(string[] args)
        {
            int skip = 0;
            int take = 2112;
            if (args.Length > 0)
                skip = int.TryParse(args[0], out int arg1) ? arg1 : skip;
            if (args.Length > 1)
                take = int.TryParse(args[1], out int arg1) ? arg1 : take;
            _logger.Trace($"START skip: {skip} take: {take}");
            foreach (var page in ComicViewerPages().Skip(skip).Take(take))
            {
                var hasPassed = false;
                HtmlDocument doc;
                List<Issue> issues;
                HtmlNodeCollection nodes;
                while (!hasPassed)
                {
                    try
                    {
                        doc = web.Load(page);
                        issues = new List<Issue>();
                        nodes = doc.DocumentNode.SelectNodes("//a[@class='front-link']");

                        if (nodes.Count != 20)
                        {
                            _logger.Warn($"node count not 20 {doc.DocumentNode.InnerHtml}");
                        }
                        foreach (var item in nodes)
                        {
                            var name = item.InnerText.Split('…').First().Trim();
                            name = name.Replace("&#038;", "&");
                            name = name.Replace("&#8211;", "–");
                            name = name.Replace("&#8217;", "’");
                            name = name.Replace("&#8216;", "‘");
                            name = name.Replace("&#8230;", "");
                            name = name.Replace("[email&#160;protected]", "");
                            name = name.Replace("&#8242;", "′");
                            if (!_comicRepository.Contains(name))
                            {
                                var links = pictureLinks(item.Attributes["href"].Value);
                                if (!links.Any())
                                    continue;

                                var issue = new Issue
                                {
                                    Comic = masterComic,
                                    IssueNumber = name
                                };
                                issue.IssuePictureLinks = links.Select(i => new IssuePicture
                                {
                                    Issue = issue,
                                    Url = i
                                }).ToList();
                                issues.Add(issue);
                            }
                        }
                        _comicRepository.AddNewIssues(issues, false);
                        _logger.Debug("Done:" + page);
                        hasPassed = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            _logger.Trace("END");
        }

        private static List<string> pictureLinks(string uri)
        {
            var doc = web.Load(uri);
            try
            {
                List<string> returnLinks = new List<string>();
                List<HtmlNodeCollection> selector = new List<HtmlNodeCollection>();
                selector.Add(doc.DocumentNode.SelectNodes("//img[@class='picture aligncenter']"));
                selector.Add(doc.DocumentNode.SelectNodes("//img[@border='0']"));
                selector.Add(doc.DocumentNode.SelectNodes("//img[@alt='']"));
                if (selector.Any(i=> i != null))
                {
                    returnLinks = selector.First(j=> j != null).Select(i => i.Attributes["src"].Value).ToList();
                }
                else
                {
                    _logger.Warn($"No links on uri: {uri}");
                }
                return returnLinks;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }
    }
}
