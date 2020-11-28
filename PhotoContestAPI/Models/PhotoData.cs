//using System;
namespace PhotoContestAPI.Models
{
    using Newtonsoft.Json;
    using System;

    public class PhotoData
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "uuid")]
        public string UUID { get; set; }

        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "imgUrl")]
        public string ImgUrl { get; set; }

        [JsonProperty(PropertyName = "imgUrlHighQuality")]
        public string ImgUrlHighQuality { get; set; }

        [JsonProperty(PropertyName = "imgUrlLowQuality")]
        public string ImgUrlLowQuality { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public int Votes { get; set; }

        [JsonProperty(PropertyName = "submitDt")]
        public DateTime SubmitDt { get; set; }

        [JsonProperty(PropertyName = "partition")]
        public int Partition { get; set; }
    }
}
