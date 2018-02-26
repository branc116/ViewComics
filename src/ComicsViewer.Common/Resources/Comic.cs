using System;
using System.Collections.Generic;
using System.Text;

namespace ComicsViewer.Common.Resources
{
    public class Comic
    {
        public int Id { get; set; }
        public DateTime LastSync { get; set; }
        public string Name { get; set; }
        public List<Issue> Issues { get; set; }
    }
}
