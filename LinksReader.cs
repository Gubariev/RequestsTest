using AngleSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace RequestsTest
{
    public class LinksReader
    {
        public LinkModel Link { get; set; }
        public LinksReader()
        {
            Link = new LinkModel();
        }

        public async Task<List<string>> HtmlReadLinks() //Считывание линков с HTML страницы
        {
            var linksList = new List<string>();
            var document = await BrowsingContext.New(Link.Cfg).OpenAsync($"https://www.{Link.WebAddress}");
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
                        linksList.Add("http://" + Link.WebAddress + link);
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(item + " - " + e.Message);
                }

            }
            return linksList;
        }
        public async Task<List<string>> XmlReadLinks() // Считывание линков с Sitemap
        {
            Uri url = new Uri($"https://{Link.WebAddress}");
            var documentSiteMap = await BrowsingContext.New(Link.Cfg).OpenAsync($"https://{url.Host}/sitemap.xml");

            var links = documentSiteMap.QuerySelectorAll("loc");

            var linksList = new List<string>();
            foreach (var link in links)
            {
                linksList.Add(link.ToHtml()
                    .Replace("<loc>", "")
                    .Replace("</loc>", "")
                    .Replace($"https://{Link.WebAddress}/sitemap.xml", ""));
            }
            if (linksList.Count == 0)
            {
                return null;
            }

            return linksList;
        }


        public void ElapseTime(List<string> links) // Подсчет времени загрузки страниц
        {
            Console.WriteLine("Starting making request by links");
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
                Console.WriteLine($"Empty Sitemap page  - {Link.WebAddress}");
            }
        }

        public void Compare(List<string> linksHtml, List<string> linksXml) // Сравнение линков Sitemap'ы и линков данной HTML страницы
        {
            int count = 0;
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
                            count++;
                        }
                    }
                }
                if (count>0)
                {
                Console.WriteLine("Comparing is finished\n");
                }
                else
                {
                    Console.WriteLine("Sitemap and this link has nothing in common\n");
                }
            }
            else
            {
                Console.WriteLine("Sitemap xml is empty");
            }


        }





    }
}
