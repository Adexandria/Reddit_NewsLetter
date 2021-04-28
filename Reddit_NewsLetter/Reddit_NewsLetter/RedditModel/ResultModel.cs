using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit_NewsLetter.RedditModel
{
    public class ResultModel
    {
        [JsonProperty("access_token")]
        public string AccessCode { get; set; }
    }
}
