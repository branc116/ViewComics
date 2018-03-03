using ComicsViewer.Common.Repository;
using ComicsViewer.Common.Resources;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using static ComicsViewer.NameFixer.Helper.DataHelper;

namespace ComicsViewer.NameFixer
{
    internal class Program
    {
        private readonly static ComicRepository _repository = GetRepository();
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly static Random _random = new Random();
        private static void Main(string[] args)
        {
#if RELEASE
            _logger.Debug("RELEASE");
#else
            _logger.Debug("DEBUG");
#endif
            var unfixedIssues = _repository.GetAllIssue("MasterComic");
            for (int i = 4; i < 10; i++)
            {
                var ui = new List<Issue>(unfixedIssues);
                var comics = GetComicNames(ui, "MasterComic", i).ToList().OrderBy(j => (j.issues?.Count ?? -1)).ToList();
                _logger.Trace($"Depth {i} finished, got {comics.Count} comic names!");
                foreach(var comic in comics)
                {
                    _logger.Trace($"{comic.comicName}{new string(' ', 100 - (comic.comicName?.Length ?? 0))}{comic.issues?.Count ?? -1}");
                }
            }
            //out1:;
            //foreach (var issue in unfixedIssues)
            //{
            //    var words = issue.IssueNumber.Split(' ');
            //    var curName = issue.IssueNumber;
            //    var startName = curName;
            //    for (int i = words.Length; i > 0; i--)
            //    {
            //        curName = words.Take(i).Aggregate((j, h) => $"{j} {h}");
            //        if (curName.ToLower() == "the" || curName.ToLower() == "a" || curName.TrimEnd(' ').EndsWith("01"))
            //            break;
            //        var similar = unfixedIssues.Where(j => j.IssueNumber.StartsWith(curName)).ToList();
            //        if (similar.Count > 1)
            //        {
            //            _logger.Debug($"Foud from issue name: {startName}, Comic name: {curName} for: ");
            //            foreach (var sim in similar.OrderBy(j => j.IssueNumber).ToList())
            //            {
            //                _logger.Debug($"    {sim.IssueNumber}");
            //            }
            //            unfixedIssues.RemoveAll(h => similar.Any(j => j.IssueNumber == h.IssueNumber));
            //            _logger.Debug($"{unfixedIssues.Count} left!");q
            //            goto out1;
            //        }
            //    }
            //    _logger.Warn($"Can't find comic name for {startName}");
            //    unfixedIssues.RemoveAll(h => h.IssueNumber == startName);
            //    goto out1;
            //}

        }

        public static IEnumerable<(List<Issue> issues, string comicName)> GetComicNames(List<Issue> issues, string baseName, int maxDepth, string similarName = null)
        {
            if (maxDepth <= 0)
            {
                yield return (issues, baseName);
                yield break;
            }
            _logger.Debug($"{baseName}{new string(' ', 100-baseName.Length)}{similarName ?? ""}{new string(' ', 100 - (similarName?.Length ?? 0))}{maxDepth}");

            var listUnlisted = new List<Issue>();
            var unordered = new List<Issue>(issues.OrderBy(i => _random.Next(0, 50000)));
            Issue issue;
            var potentialSingletons = similarName != null ? issues.Where(i => i.IssueNumber == baseName).ToList() : new List<Issue>();
            if (similarName != null) {
                unordered.RemoveAll(h => potentialSingletons.Any(j => j.IssueNumber == h.IssueNumber));
            }
            while ((issue = unordered.FirstOrDefault()) != null)
            {
                var words = issue.IssueNumber.Split(' ');
                var curName = issue.IssueNumber;
                var startName = curName;
                for (int i = words.Length; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        unordered.RemoveAll(h => h.IssueNumber == startName);
                        yield return (new List<Issue> { issue }, issue.IssueNumber);
                        break;
                    }
                    curName = words.Take(i).Aggregate((j, h) => $"{j} {h}");
                    //if ((curName.ToLower() == "the" || curName.ToLower() == "a") && i == 1)
                    //{
                    //    unordered.RemoveAll(j => j.Id == issue.Id);
                    //    yield return (new List<Issue> { issue }, issue.IssueNumber);
                    //    break;
                    //}
                    //if (curName.TrimEnd(' ').EndsWith("01"))
                    //{
                    //    unordered.RemoveAll(j => j.Id == issue.Id);
                    //    potentialSingletons.Add(issue);
                    //    break;
                    //}
                    var similar = unordered.Where(j => j.IssueNumber.StartsWith(curName))
                                           .Union(potentialSingletons.Where(j => j.IssueNumber.StartsWith(curName)))
                                           .ToList();
                    if (similar.Count > 1)
                    {
                        //_logger.Debug($"Foud from issue name: {startName}, Comic name: {curName} for: ");
                        //foreach (var sim in similar.OrderBy(j => j.IssueNumber).ToList())
                        //{
                        //    _logger.Debug($"    {sim.IssueNumber}");
                        //}
                        unordered.RemoveAll(h => similar.Any(j => j.IssueNumber == h.IssueNumber));
                        potentialSingletons.RemoveAll(h => similar.Any(j => j.IssueNumber == h.IssueNumber));
                        //_logger.Debug($"{unordered.Count} left! deepth {maxDepth}");
                        foreach (var comic in GetComicNames(similar, curName, maxDepth-1, startName).ToList())
                        {
                            if (comic.issues != null && comic.issues.Any())
                                yield return comic;
                        }
                        break;
                    }
                }
                //_logger.Warn($"Can't find comic name for {startName}");
                
            }

            if (listUnlisted != null && listUnlisted.Count > 0)
                yield return (listUnlisted, baseName);
            foreach (var single in potentialSingletons)
            {
                if (issue != null) 
                    yield return (new List<Issue> { issue }, issue?.IssueNumber);
            }
        }

    }
}
