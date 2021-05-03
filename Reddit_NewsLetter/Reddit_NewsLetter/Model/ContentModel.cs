using System;
using System.Text.Json.Serialization;


namespace Reddit_NewsLetter.Model
{
    public class ContentModel
    {
        [JsonPropertyName("personalizations")]
        public Personalization[] Personalizations { get; set; }
        [JsonPropertyName("from")]
        public From From { get; set; }
        [JsonPropertyName("subject")]
        public string Subject { get; set; }
        [JsonPropertyName("content")]
        public Content[] Content { get; set; }
        [JsonPropertyName("send_at")]
        public int SendAt { get; set; }
        [JsonPropertyName("batch_id")]
        public string BatchId { get; set; }

    }
}
