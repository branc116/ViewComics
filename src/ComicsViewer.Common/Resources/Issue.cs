using System;
using System.Collections.Generic;
using System.Text;

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
