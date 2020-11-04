//using System;
namespace PhotoContestAPI.Models
{
    using Newtonsoft.Json;

    public class PhotoData
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "imgUrl")]
        public string ImgUrl { get; set; }

        [JsonProperty(PropertyName = "partition")]
        public int Partition { get; set; }
    }
}
