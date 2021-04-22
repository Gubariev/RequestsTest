using AngleSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace RequestsTest
{
    public class LinksReader
    {
        public string WebAddress { get; set; }

        public IConfiguration Cfg { get; set; }

        public LinksReader()
        {
            WebAddress = "google.com";
            Cfg = Configuration.Default.WithDefaultLoader();
        }


        public async Task<List<string>> HtmlReadLinks()
        {
            var linksList = new List<string>();
            var document = await BrowsingContext.New(Cfg).OpenAsync($"https://{WebAddress}");
            var links = document.QuerySelectorAll("a");
            foreach (var item in links)
            {
                var link = item.GetAttribute("href");
                if (link.Contains("http://") || link.Contains("https://"))
                {
                    linksList.Add(link);
                }
                else
                {
                    linksList.Add("http://" + WebAddress + link);
                }
            }
            return linksList;
        }
        public async Task<List<string>> XmlReadLinks()
        {

            var documentSiteMap = await BrowsingContext.New(Cfg).OpenAsync($"https://{WebAddress}/sitemap.xml");

            var links = documentSiteMap.QuerySelectorAll("loc");

            var linksList = new List<string>();
            foreach (var link in links)
            {
                linksList.Add(link.ToHtml()
                    .Replace("<loc>", "")
                    .Replace("</loc>", "")
                    .Replace($"https://{WebAddress}/sitemap.xml", ""));
            }
            if (linksList.Count == 0)
            {
                return null;
            }

            return linksList;
        }


        public void ElapseTime(List<string> links)
        {
            if (links != null)
            {
                using (var client = new WebClient())
                {
                        foreach (var link in links)
                        {
                            try
                            {
                                Stopwatch stopwatch = new Stopwatch();

                                stopwatch.Start();
                                var result = client.DownloadString(link);
                                stopwatch.Stop();
                                Console.WriteLine(link + " = " + stopwatch.Elapsed);
                            }
                            catch (WebException e)
                            {
                                Console.WriteLine(link + " - " + e.Message);
                            }

                        }
                    Console.WriteLine("Elapsing is finished");
                }
            }
            else
            {
                Console.WriteLine("It's empty");
            }
        }

        public void Compare(List<string> linksHtml, List<string> linksXml)
        {
            if (linksXml != null)
            {
                for (int i = 0; i < linksXml.Count; i++)
                {
                    for (int j = 0; j < linksHtml.Count; j++)
                    {
                        if (linksXml[i] == linksHtml[j])
                        {
                            Console.WriteLine(linksXml[i]);
                        }
                    }
                }

                Console.WriteLine("Comparing is finished");
            }
            else
            {
                Console.WriteLine("Sitemap xml is empty");
            }


        }





    }
}
