using Microsoft.AspNetCore.Mvc;
using System;
using ImdbScraperApi.Utils;
using System.Collections.Generic;
using ImdbScraperApi.Models;
using ImdbScraperApi.Scrapers;
using ImdbScraperApi.Scrapers.Imdb;
using System.Linq;

namespace ImdbScraperApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    /// resful api controller for celebrities model and json ropository.
    public class CelebritiesController : ControllerBase
    {

        [HttpGet] //Get /celebrities
        public IEnumerable<Celebrity> GetCelebrities()
        {
            var celebsList = JsonUtils.DeserializeCelebritiesFromJson();
            if(celebsList == null)
            {
                return Enumerable.Empty<Celebrity>();
            }
            return celebsList;
        }


        [HttpDelete("{id}")] //Delete a celebrity by id
        public ActionResult Delete(int id)
        {
            if (JsonUtils.DeleteCelebrityfromJsonById(id) == false)
            {
                return NotFound(); 
            }
            return Ok(); 
        }
        [HttpPost]
        [Route("/[controller]/scrape")]     // Post to scrape imd and replace current json db.
        public ActionResult ScrapeFromImdb()
        {
            CelebritiesScraper.ScrapeImdb();
            return Ok();
        }
    }
}
