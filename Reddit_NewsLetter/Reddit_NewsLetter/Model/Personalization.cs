using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Reddit_NewsLetter.Model
{
    public class Personalization
    { 
        [JsonPropertyName("to")]
        public To[] To { get; set; }
       
    }
}