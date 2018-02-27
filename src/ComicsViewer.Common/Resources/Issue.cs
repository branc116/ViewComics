using System.Collections.Generic;

namespace ComicsViewer.Common.Resources
{
    public class Issue
    {
        public int Id { get; set; }
        public string IssueNumber { get; set; }
        public Comic Comic { get; set; }
        public List<IssuePicture> IssuePictureLinks { get; set; }
    }
}
