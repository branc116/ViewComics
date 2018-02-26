using System;
using System.Collections.Generic;
using System.Text;

namespace ComicsViewer.Common.Resources
{
    public class IssuePicture
    {
        public int Id { get; set; }
        public Issue Issue { get; set; }
        public string Url { get; set; }
    }
}
