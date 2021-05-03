using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Reddit_NewsLetter.Model.PostModel
{
    public class Preview
    {
        [JsonPropertyName("images")]
        public List<Image> Images { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }
}