using AngleSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            var document = await BrowsingContext.New(Link.Cfg).OpenAsync(Link.WebAddress);
            var links = document.QuerySelectorAll("a");
            if (links != null)
            {
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
                            linksList.Add(Link.WebAddress + link);
                        }
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(item + " - " + e.Message);
                    }

                }
                if (linksList.Count!=0)
                {
                    return linksList;
                }
            }
                Console.WriteLine("Couldnt download page");
                return null;
            
        }
        public async Task<List<string>> XmlReadLinks() // Считывание линков с Sitemap
        {
            string url = ("https://" + new Uri(Link.WebAddress).Host + "/sitemap.xml");
            var documentSiteMap = await BrowsingContext.New(Link.Cfg).OpenAsync(url);

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


        public List<LinkModel> ElapseTime(List<string> links) // Подсчет времени загрузки страниц
        {
            var UrlsElapsed = new List<LinkModel>();
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
                            client.DownloadString(link);
                            stopwatch.Stop();
                            UrlsElapsed.Add(new LinkModel() { ElapseTime = stopwatch.Elapsed.TotalSeconds, WebAddress = link });
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(link + " - " + e.Message);
                        }

                    }
                    Console.WriteLine($"\nElapsing is finished\nLinks count - {links.Count} ");
                    return new List<LinkModel>(UrlsElapsed.OrderBy(l => l.ElapseTime));
                }
            }
            Console.WriteLine($"Empty Sitemap page  - {Link.WebAddress}");
            return null;
        }

        public List<string> CompareXml(List<string> linksXml, List<string> linksHtml) // Сравнение линков Sitemap'ы и линков данной HTML страницы
        {
            if (linksXml != null)
            {
                for (int i = 0; i < linksXml.Count; i++)
                {
                    for (int j = 0; j < linksHtml.Count; j++)
                    {
                        if (linksXml[i] == linksHtml[j])
                        {
                            linksXml.Remove(linksXml[i]);
                            i--;
                        }
                    }
                }
                return linksXml;
            }
            return null;
        }

        public List<string> CompareHtml(List<string> linksHtml, List<string> linksXml) // Сравнение линков Sitemap'ы и линков данной HTML страницы
        {
            if (linksXml != null)
            {
                for (int i = 0; i < linksHtml.Count; i++)
                {
                    for (int j = 0; j < linksXml.Count; j++)
                    {
                        if (linksHtml[i] == linksXml[j])
                        {
                            linksHtml.Remove(linksHtml[i]);
                            i--;
                        }
                    }
                }
            }
            return linksHtml;
        }

        public static void Output(List<string> links, string message)
        {
            if (links != null)
            {
                Console.WriteLine($"\n{message} links");
                foreach (var item in links)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("This document is empty");
            }

        }

        public void OutputElapseTime(List<string> links)
        {
            if (links != null)
            {
                foreach (var item in ElapseTime(links))
                {
                    Console.WriteLine($"{item.WebAddress} - {item.ElapseTime} seconds");
                }
            }
            else
            {
                Console.WriteLine("LinksList is empty");
            }

        }





    }
}
