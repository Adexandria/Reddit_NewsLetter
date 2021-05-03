using System.Text.Json.Serialization;

namespace Reddit_NewsLetter.Model.PostModel
{
    public class Child
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("data")]
        public Data2 Data { get; set; }
    }
}