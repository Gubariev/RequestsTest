using System;
using System.Threading.Tasks;

namespace RequestsTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var TestLinks = new LinksReader();
            Console.WriteLine("Write your link \nExamples: google.com, dl.nure.ua, youtube.com/watch?v=sFrubDwkh70");
            TestLinks.Link.WebAddress = Console.ReadLine();

            var htmlLinks = TestLinks.CompareHtml(TestLinks.HtmlReadLinks().Result, TestLinks.XmlReadLinks().Result);
            var xmlLinks = TestLinks.CompareXml(TestLinks.XmlReadLinks().Result, TestLinks.HtmlReadLinks().Result);
            
            LinksReader.Output(htmlLinks, "html");
            LinksReader.Output(xmlLinks, "xml");

            foreach (var item in TestLinks.ElapseTime(htmlLinks))
            {
                Console.WriteLine($"{item.WebAddress} - {item.ElapseTime} seconds");
            }
            foreach (var item in TestLinks.ElapseTime(xmlLinks))
            {
                Console.WriteLine($"{item.WebAddress} - {item.ElapseTime} seconds");
            }
           // TestLinks.ElapseTime(TestLinks.XmlReadLinks().Result);
            Console.ReadKey();
        }


    }
}
