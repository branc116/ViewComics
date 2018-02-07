using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsViewer.Web.Models.Comics
{


    public class Comics
    {
        public Comic[] AllComics { get; set; }
    }   

    public class Comic
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public Issues Issues { get; set; }
    }

    public class Issues
    {
        public AllIssues[] value { get; set; }
        public int Count { get; set; }
    }

    public class AllIssues
    {
        public string Issue { get; set; }
        public string Href { get; set; }
        public string NameAndIssue { get; set; }
        public string Name { get; set; }
    }


}
