using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Reddit_NewsLetter.Model
{
    public class To
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
    }
}