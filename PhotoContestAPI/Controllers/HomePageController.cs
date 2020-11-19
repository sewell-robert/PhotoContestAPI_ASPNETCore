using Microsoft.AspNetCore.Mvc;
using PhotoContestAPI.Models;
using PhotoContestAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomePageController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;
        public HomePageController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        // GET: api/<HomePageController>
        [HttpGet]
        public async Task<List<PhotoData>> Get()
        {
            var response = await _cosmosDbService.GetItemsAsync("select * from c");

            double totalPhotos = response.Count();

            var takeAmount = (int)Math.Ceiling(totalPhotos / 4);

            var results = response.OrderByDescending(p => p.Votes).Take(takeAmount).ToList();

            //var results = response.OrderByDescending(x => x.Votes).GroupBy(p => p.ContestWeek).SelectMany(y => y.Take(2)).ToList();

            return results;
        }
    }
}
