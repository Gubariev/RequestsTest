using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;

namespace RequestsTest
{
    public class LinkModel
    {
        public string WebAddress { get; set; }

        public IConfiguration Cfg { get; set; }

        public LinkModel()
        {
            WebAddress = "google.com";
            Cfg = Configuration.Default.WithDefaultLoader();
        }
    }
}
