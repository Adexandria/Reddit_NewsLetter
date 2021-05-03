using System.Text.Json.Serialization;

namespace Reddit_NewsLetter.Model
{
    public class From
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
       
    }
}