using ImdbScraperApi.Models;
using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ImdbScraperApi.Scrapers.Imdb
{
    public static class ScraperUtils
    {
        public static int ExtractIndexFromNode(HtmlNode node, string nodeXPath)  //function that extract string contains celeb index,
        {                                                                        // and filter that string to extract index as int.
            var chosenNode = node.SelectSingleNode(nodeXPath);
            var nodeInnerText = chosenNode.InnerText;
            return int.Parse(Regex.Replace(nodeInnerText.Split()[0], @"[^0-9a-zA-Z\ ]+", ""));

        }
        public static string ExtractNodeAttribute(HtmlDocument doc, string nodeXPath, string attribute) //generic utility node attribute helper function
        {
            var node = doc.DocumentNode.SelectSingleNode(nodeXPath);
            var link = node.Attributes[attribute].Value;
            return link;
        }
        public static string ExtractRoleFromNode(HtmlNode node, string nodeXPath)  //function that extract string contains celeb role,
        {                                                                         // filter that string to extract the role
            var chosenNode = node.SelectSingleNode(nodeXPath);
            var nodeInnerText = chosenNode.InnerText;
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            nodeInnerText = rgx.Replace(nodeInnerText, "");
            nodeInnerText = nodeInnerText.Trim();
            return nodeInnerText.Split(' ').FirstOrDefault();
        }
        public static string ExtractUriFromNode(HtmlNode node, string nodeXPath, string attribute)//generic utility func, extracting uri form node 
        {
            var baseUri = new Uri("https://www.imdb.com");
            var chosenNode = node.SelectSingleNode(nodeXPath);
            var link = new Uri(baseUri, chosenNode.Attributes[attribute].Value).AbsoluteUri;
            return link;
        }

        public static string ExtractGenderFromCelebPage(HtmlDocument doc) //checkes if the celebrity has ACTOR[male] or ACTRESS[female] in his filmography  
        {                                                                // in oreder to identify that celebrity gender. return string of the gender if found.
            if(doc.DocumentNode.SelectSingleNode("//div[@id=\"filmography\"]/div[@id=\"filmo-head-actor\"]") != null)
            {
                return "Male";
            }
            else if(doc.DocumentNode.SelectSingleNode("//div[@id=\"filmography\"]/div[@id=\"filmo-head-actress\"]") != null)
            {
                return "Female";
            }
            else
            {
                return "Unkown";
            }

        }
        public static HtmlDocument GetDocument(string url)//generic utility func
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            return doc;
        }

        public static DateTime ExtractBirthDateFromCelebPage(HtmlDocument doc, string nodeXPath, string attribute)//extracting date as strinf from page, return datetime obj.
        {
            var dateString = ExtractNodeAttribute(doc, nodeXPath, attribute);
            return Convert.ToDateTime(dateString);

        }

    }
}
