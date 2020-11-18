using Microsoft.AspNetCore.Http;
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
    public class VoterBoxController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;
        public VoterBoxController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        // GET api/<VoterBoxController>/55
        [HttpGet("{id}")]
        public async Task<int> StoreVoteAsync(int id)
        {
            var photoDataObject = await _cosmosDbService.GetItemAsync(id.ToString());

            photoDataObject.Votes = photoDataObject.Votes + 1;

            await _cosmosDbService.UpdateItemAsync(photoDataObject);

            return photoDataObject.Votes;
        }
    }
}
