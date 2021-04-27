using System;
using System.Threading.Tasks;

namespace RequestsTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var TestLinks = new LinksReader();
            Console.WriteLine("Write your link \nExamples: http://google.com, http://dl.nure.ua, http://youtube.com/watch?v=sFrubDwkh70");
            TestLinks.Link.WebAddress = Console.ReadLine();

            var htmlLinks = TestLinks.CompareHtml(TestLinks.HtmlReadLinks().Result, TestLinks.XmlReadLinks().Result);
            var xmlLinks = TestLinks.CompareXml(TestLinks.XmlReadLinks().Result, TestLinks.HtmlReadLinks().Result);
            
            LinksReader.Output(htmlLinks, "Html");
            LinksReader.Output(xmlLinks, "Xml");

            TestLinks.OutputElapseTime(htmlLinks);
            TestLinks.OutputElapseTime(xmlLinks);
            
            Console.ReadKey();
        }


    }
}
