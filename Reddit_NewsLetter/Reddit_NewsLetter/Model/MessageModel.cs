using System;

namespace Reddit_NewsLetter.Model
{
    public class MessageModel
    {
        public string Subreddit { get; set; }
        public string Title { get; set; }
        public int Ups { get; set; }
        public string Url { get; set; }
    }
}
