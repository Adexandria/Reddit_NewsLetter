using System.Text.Json.Serialization;

namespace Reddit_NewsLetter.Model
{
    public class Content
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "application/json";
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}