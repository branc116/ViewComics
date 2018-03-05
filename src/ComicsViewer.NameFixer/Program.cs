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
        private static void Main(string[] args)
        {
#if RELEASE
            _logger.Debug("RELEASE");
#else
            _logger.Debug("DEBUG");
#endif
            var unfixedIssues = _repository.GetAllIssue("MasterComic");
            var ui = new List<Issue>(unfixedIssues);
            var comics = GetComicNames(ui, "MasterComic", ui.Count)
                .ToList()
                .OrderBy(j => (j.comicName))
                .ToList();
            foreach(var comic in comics)
            {
                _logger.Trace($"{comic.comicName}{new string(' ', 100 - (comic.comicName?.Length ?? 0))}{comic.issues?.Count ?? -1}");
            }
            
            _logger.Trace($"Depth finished, got {comics.Count} comic names! {unfixedIssues.Count} == {comics.Sum(i => i.issues.Count)}");
            Update(comics);
        }
        public static void Update(List<(List<Issue> issues, string name)> comics)
        {
            foreach(var comic in comics)
            {
                foreach(var issue in comic.issues)
                {
                    _repository.MoveIssue(issue.Id, comic.name);
                }
            }
        }

        public static IEnumerable<(List<Issue> issues, string comicName)> GetComicNames(List<Issue> issues, string baseName, int maxDepth, string similarName = null)
        {
            if (maxDepth <= 0)
            {
                yield return (issues, baseName);
                yield break;
            }
            _logger.Debug($"{baseName}{new string(' ', 100-baseName.Length)}{similarName ?? ""}{new string(' ', 100 - (similarName?.Length ?? 0))}{maxDepth}");
            
            var unordered = new List<Issue>(issues.ToList());
            Issue issue;
            var potentialSingletons = similarName != null ? issues.Where(i => i.IssueNumber == similarName).ToList() : new List<Issue>();
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
                    var similar = unordered.Where(j => j.IssueNumber.StartsWith(curName))
                                           .Union(potentialSingletons.Where(j => j.IssueNumber.StartsWith(curName)))
                                           .ToList();
                    if (similar.Count > 1)
                    {
                        unordered.RemoveAll(h => similar.Any(j => j.IssueNumber == h.IssueNumber));
                        potentialSingletons.RemoveAll(h => similar.Any(j => j.IssueNumber == h.IssueNumber));
                        var rec = GetComicNames(similar, curName, Math.Min(maxDepth - 1, similar.Count), startName).ToList();
                        var recSum = rec.Sum(j => j.issues.Count);
                        if (recSum != similar.Count)
                        {
                            _logger.Error($"{recSum} != {similar.Count}");
                            foreach(var r in rec)
                            {
                                _logger.Error(r.comicName);
                                foreach(var c in r.issues)
                                {
                                    _logger.Error("     " + c.IssueNumber);
                                }
                            }
                            _logger.Error("Sim: ");
                            foreach (var c in similar)
                            {
                                _logger.Error("     " + c.IssueNumber);
                            }
                        }

                        foreach (var comic in rec)
                        {
                            if (comic.issues != null && comic.issues.Any())
                                yield return comic;
                        }
                        break;
                    }
                }
            }
            
            foreach (var single in potentialSingletons)
            {
                if (single != null) 
                    yield return (new List<Issue> { single }, single?.IssueNumber);
            }
        }

    }
}   
