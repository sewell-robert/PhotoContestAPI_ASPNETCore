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
using TinifyAPI;
using PhotoContestAPI.Classes;

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
        public async Task<List<PhotoData>> Get()
        {
            //byte[] imageArray = System.IO.File.ReadAllBytes("C:\\Users\\rober\\test-api\\images\\test-pic.jpg");
            //string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            //return base64ImageRepresentation;

            var results = await _cosmosDbService.GetItemsAsync("select * from c");

            var resultsToList = results.ToList();

            //var result = resultsList[resultsList.Count - 1];

            return resultsToList;
        }

        // GET api/<UploadPhotosController>/5
        [HttpGet("{id}")]
        public async Task<PhotoData> GetItemAsync(int id)
        {

            var test = await _cosmosDbService.GetItemAsync(id.ToString());

            return test;
        }

        // GET api/<UploadPhotosController>/last
        [Route("/api/uploadphotos/last")]
        public async Task<PhotoData> GetItemsAsync()
        {
            var results = await _cosmosDbService.GetItemsAsync("select * from c");

            var resultsToList = results.ToList();

            var result = resultsToList[resultsToList.Count - 1];

            return result;
        }

        // GET api/<UploadPhotosController>/page/4
        [Route("/api/uploadphotos/page/{pageNumber}")]
        [HttpGet("{pageNumber}")]
        public async Task<List<PhotoData>> GetItemsAsync(int pageNumber)
        {
            MiscCalculations miscCalculations = new MiscCalculations();
            var contestWeek = miscCalculations.GetContestWeek();

            var results = await _cosmosDbService.GetItemsAsync("select * from c");

            results = results.Where(p => p.ContestWeek == contestWeek).Reverse().ToList();

            var skip = 6 * (pageNumber - 1);

            List<PhotoData> photoList = new List<PhotoData>();
            for (var i = 0; i <= (results.Count() - 1); i++)
            {
                if (i >= skip)
                {
                    photoList.Add(results.ElementAt(i));
                }

                if (photoList.Count() == 6)
                {
                    break;
                }
            }

            return photoList;
        }

        // GET api/<UploadPhotosController>/action/count
        [Route("/api/uploadphotos/count")]
        public async Task<int> GetCountAsync()
        {
            MiscCalculations miscCalculations = new MiscCalculations();
            var contestWeek = miscCalculations.GetContestWeek();

            var results = await _cosmosDbService.GetItemsAsync("select * from c");

            results = results.Where(p => p.ContestWeek == contestWeek).Reverse();

            return results.Count();
        }

        // POST api/<UploadPhotosController>
        [HttpPost]
        public async Task<PhotoData> Post([FromForm] IFormFile file, [FromForm] string index, [FromForm] string author, [FromForm] string uuid)
        {
            var fileName = file.FileName;
            var convertedIndex = Convert.ToInt32(index) + 1;

            PhotoData photoDataObject = new PhotoData();

            try
            {
                byte[] fileBytes;
                //byte[] resultData;
                //string byteArrayToString;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();

                    //These two lines compress the photo, but may not need to compress twice since we do it again down below with the Resize() method
                    /*Tinify.Key = "NC86NBC6Qrjhp2GtQxC6k0l8Dbv17NZc";*/ //API Key
                    /*resultData = await Tinify.FromBuffer(fileBytes).ToBuffer();*/ //Compresses image only

                    //byteArrayToString = Convert.ToBase64String(fileBytes);
                    // act on the Base64 data
                }

                var mimeType = file.ContentType;

                var blobStorageService = new BlobStorageService();
                var url = blobStorageService.UploadFileToBlob(fileName, fileBytes, mimeType);
                //var url = blobStorageService.UploadFileToBlob(fileName, resultData, mimeType);

                Tinify.Key = "NC86NBC6Qrjhp2GtQxC6k0l8Dbv17NZc";
                var source = Tinify.FromUrl(url);
                var resized = source.Resize(new
                {
                    //method = "scale",
                    //width = 568

                    method = "cover",
                    width = 156,
                    height = 156
                });
                var resultDataResized = await resized.ToBuffer();
                var url2 = blobStorageService.UploadFileToBlob(fileName, resultDataResized, mimeType);

                MiscCalculations miscCalculations = new MiscCalculations();
                var photoData = new PhotoData
                {
                    Id = convertedIndex.ToString(),
                    UUID = uuid,
                    Author = author,
                    Description = "Test " + convertedIndex.ToString(),
                    ImgUrlHighQuality = url,
                    ImgUrlLowQuality = url2,
                    Votes = 0,
                    SubmitDt = DateTime.Now,
                    ContestWeek = miscCalculations.GetContestWeek(),
                    Partition = 1
                };

                await _cosmosDbService.AddItemAsync(photoData);

                //get the newly created PhotoData object to send back in POST reponse
                photoDataObject = await _cosmosDbService.GetItemAsync(convertedIndex.ToString());
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }

            return photoDataObject;
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
