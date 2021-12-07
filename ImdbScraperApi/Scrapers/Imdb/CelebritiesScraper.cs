using ImdbScraperApi.Models;
using ImdbScraperApi.Scrapers.Imdb;
using ImdbScraperApi.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ImdbScraperApi.Scrapers.Imdb
{
    public static class CelebritiesScraper 
    {
      /// <summary>
      /// web scraper designed to scrape imdb and extract celebrities info into a json file, using agilityHTML nuger package.
      /// </summary>
        public struct FirstPageCelebInfo
        {
            public string Link { get; init; }
            public string Role { get; init; }
            public int CelebIndex { get; init; } //will be converted later to int
        }  // a data struct intended to help functions in this class transfer data 

        public static void ScrapeImdb()  // the scraping class main activation function.
        {
            string url = "https://www.imdb.com/list/ls052283250/";  //scraping url
            var doc = ScraperUtils.GetDocument(url);
            var CelebrityFirstPageInfoList = GetCelebritiesFistPageInfo(url);  // gathers celebrities info from front page (original url)
            var celebList = CelebrityListGenerator(CelebrityFirstPageInfoList, url); // gathers info from each celebrity personal page, returning celebrity list 

            JsonUtils.SerializeCelebritiesToJsonFile(celebList);  //writing the celebrity list into a json file
        }

        public static List<FirstPageCelebInfo> GetCelebritiesFistPageInfo(string url)  //gathers available info from the list of celebrities webpage
        {
            try
            {
                var doc = ScraperUtils.GetDocument(url);
                var baseUri = new Uri("https://www.imdb.com");
                var FirstPageCelebList = new List<FirstPageCelebInfo>();   //FirstPageCelebInfo struct orginize relevant info from the first page.
                var linkNodes = doc.DocumentNode.SelectNodes("//div[@class=\"lister-item-content\"]"); //selects each celebrity info node

                foreach (var node in linkNodes)  //runs on each celeb node individually to extract data
                {
                    
                    var newFirstPageCelebInfo = new FirstPageCelebInfo()
                    {
                        Link = ScraperUtils.ExtractUriFromNode(node,".//h3/a","href"),   //extract the inner page of each celebrity for further scraping
                        CelebIndex = ScraperUtils.ExtractIndexFromNode(node,".//h3/span"),  //extract celeb popularity index and makes it into id [key]
                        Role = ScraperUtils.ExtractRoleFromNode(node, ".//p[@class=\"text-muted text-small\"]") //extract the celebrity role
                    };
                    FirstPageCelebList.Add(newFirstPageCelebInfo); 
                }

                return FirstPageCelebList;
            }
            catch (Exception e) // i dont like general exceptions, will change in futhure if needed.
            {
                throw new Exception(e.Message);
            }
        }

        public static List<Celebrity> CelebrityListGenerator(List<FirstPageCelebInfo> celebrityFirstPageInfoList, string url) //gathers info from each celebrity personal page and combine it
        {                                                                                                            //with the first page info inorder to create a celebrity(model)
            var celebrityList = new List<Celebrity>();

/*            foreach (var link in celebrityLinksList)*/       /// not doing for each because scraping takes alot of time
            for (int i=0;i<5;i++)  
            {
                var link = celebrityFirstPageInfoList[i].Link;
                var doc = ScraperUtils.GetDocument(link);
                Celebrity celeb = new Celebrity()
                {
                    Id = celebrityFirstPageInfoList[i].CelebIndex,    
                    FullName = doc.DocumentNode.SelectSingleNode("//h1/span").InnerText,  //extracts full name of celebrity from celebrity personal poage
                    BirthDate = ScraperUtils.ExtractBirthDateFromCelebPage(doc, "//div[@id=\"name-born-info\"]/time", "datetime"), //extracts birthday (datetime)
                    Gender = ScraperUtils.ExtractGenderFromCelebPage(doc),//extracts gender
                    Role = celebrityFirstPageInfoList[i].Role,
                    ImageUrl = ScraperUtils.ExtractNodeAttribute(doc, "//img[@id=\"name-poster\"]","src")//extracts image url
                };
                celebrityList.Add(celeb);
                Console.WriteLine($"add celeb {celeb.FullName}");
            }
            return celebrityList;
        }

        
    }
}
