using ComicsViewer.Common.Repository;
using NLog;
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
            var unfixedIssues = _repository.GetAllIssue("MasterComic");
            out1:;
            foreach (var issue in unfixedIssues)
            {
                var words = issue.IssueNumber.Split(' ');
                var curName = issue.IssueNumber;
                var startName = curName;
                for (int i = words.Length; i > 0; i--)
                {
                    curName = words.Take(i).Aggregate((j, h) => $"{j} {h}");
                    var similar = unfixedIssues.Where(j => j.IssueNumber.StartsWith(curName)).ToList();
                    if (similar.Count > 1)
                    {
                        _logger.Debug($"Foud from issue name: {startName}, Comic name: {curName} for: ");
                        foreach (var sim in similar.OrderBy(j => j.IssueNumber).ToList())
                        {
                            _logger.Debug($"    {sim.IssueNumber}");
                        }
                        unfixedIssues.RemoveAll(h => similar.Any(j => j.IssueNumber == h.IssueNumber));
                        _logger.Debug($"{unfixedIssues.Count} left!");
                        goto out1;
                    }
                }
                _logger.Warn($"Can't find comic name for {startName}");
                unfixedIssues.RemoveAll(h => h.IssueNumber == startName);
                goto out1;
            }
        }
    }
}
