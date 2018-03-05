using System.Collections.Generic;

namespace ComicsViewer.Web.Models.Comics
{
    public class IssueViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<string> Links { get; set; }
    }
}
