using AngleSharp;
using AngleSharp.Extensions;
using System;
using System.Threading.Tasks;

namespace RequestsTest
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var TestLinks = new LinksReader();
            TestLinks.WebAddress = Console.ReadLine();
            var xmlLinks = TestLinks.XmlReadLinks();
            var htmlLinks = TestLinks.HtmlReadLinks();

            TestLinks.Compare(htmlLinks.Result, xmlLinks.Result);
            TestLinks.ElapseTime(htmlLinks.Result);

            Console.ReadKey();
        }
    }
}
