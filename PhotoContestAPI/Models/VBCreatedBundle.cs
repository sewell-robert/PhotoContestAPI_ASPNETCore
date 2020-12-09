using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContestAPI.Models
{
    public class VBCreatedBundle
    {
        [JsonProperty(PropertyName = "photoCount")]
        public int PhotoCount { get; set; }

        [JsonProperty(PropertyName = "photoDataList")]
        public List<PhotoData> PhotoDataList { get; set; }
    }
}
