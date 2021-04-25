﻿using AngleSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
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
            var document = await BrowsingContext.New(Cfg).OpenAsync($"https://www.{WebAddress}");
            var links = document.QuerySelectorAll("a");
            foreach (var item in links)
            {
                try
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
                catch (Exception e)
                {

                    Console.WriteLine(item + " - " + e.Message);
                }

            }
            return linksList;
        }
        public async Task<List<string>> XmlReadLinks()
        {
            Uri url = new Uri($"https://{WebAddress}");
            Console.WriteLine(url.Host);
            var documentSiteMap = await BrowsingContext.New(Cfg).OpenAsync($"https://{url.Host}/sitemap.xml");

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
                Console.WriteLine($"Empty page - {WebAddress}");
            }
        }

        public void Compare(List<string> linksHtml, List<string> linksXml)
        {
            if (linksXml != null)
            {
                Console.WriteLine("Starting comparing HTML links and Sitemap links");
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
