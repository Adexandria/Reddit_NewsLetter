using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Reddit_NewsLetter.Model.PostModel
{
    public class Data
    {
        [JsonPropertyName("modhash")]
        public string Modhash { get; set; }

        [JsonPropertyName("dist")]
        public int Dist { get; set; }

        [JsonPropertyName("children")]
        public List<Child> Children { get; set; }

    }
}