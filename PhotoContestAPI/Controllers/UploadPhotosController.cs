using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Azure.Cosmos.Linq;
using PhotoContestAPI.Services;
using PhotoContestAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PhotoContestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadPhotosController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;
        public UploadPhotosController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        // GET: api/<UploadPhotosController>
        [HttpGet]
        public async Task<PhotoData> Get()
        {
            //byte[] imageArray = System.IO.File.ReadAllBytes("C:\\Users\\rober\\test-api\\images\\test-pic.jpg");
            //string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            //return base64ImageRepresentation;

            var results = await _cosmosDbService.GetItemsAsync("select * from c");

            var resultsList = results.ToList();

            var result = resultsList[resultsList.Count - 1];

            return result;
        }

        // GET api/<UploadPhotosController>/5
        [HttpGet("{id}")]
        public async Task<PhotoData> GetItemAsync(int id)
        {

            var test = await _cosmosDbService.GetItemAsync(id.ToString());

            return test;
        }

        // POST api/<UploadPhotosController>
        [HttpPost]
        public async void Post([FromForm] IFormFile file, [FromForm] string index)
        {
            var fileName = file.FileName;

            var convertedIndex = Convert.ToInt32(index) + 1;

            byte[] fileBytes;
            //string byteArrayToString;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
                //byteArrayToString = Convert.ToBase64String(fileBytes);
                // act on the Base64 data
            }

            var mimeType = file.ContentType;

            var blobStorageService = new BlobStorageService();

            var url = blobStorageService.UploadFileToBlob(fileName, fileBytes, mimeType);

            var photoData = new PhotoData
            {
                Id = convertedIndex.ToString(),
                Author = "Robert",
                Description = "Test " + convertedIndex.ToString(),
                ImgUrl = url,
                Partition = 1
            };

            await _cosmosDbService.AddItemAsync(photoData);
        }

        // PUT api/<UploadPhotosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UploadPhotosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
