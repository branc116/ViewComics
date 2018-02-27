using System.Collections.Generic;

namespace ComicsViewer.Web.Models.Comics
{
    public class ComicViewModel
    {
        public string Name { get; set; }
        public List<IssueViewModel> AllIssues { get; set; }
    }
}
