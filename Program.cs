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
            TestLinks.WebAddress = Console.ReadLine();
            var xmlLinks = TestLinks.XmlReadLinks();
            var htmlLinks = TestLinks.HtmlReadLinks();

            TestLinks.Compare(htmlLinks.Result, xmlLinks.Result);
            TestLinks.ElapseTime(htmlLinks.Result);
            TestLinks.ElapseTime(xmlLinks.Result);
            Console.ReadKey();
        }


    }
}
