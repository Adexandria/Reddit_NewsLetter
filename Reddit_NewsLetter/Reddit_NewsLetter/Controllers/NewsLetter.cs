using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reddit_NewsLetter.Model;
using Reddit_NewsLetter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace Reddit_NewsLetter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsLetter : ControllerBase
    {
        
        private readonly string api_key = Environment.GetEnvironmentVariable("redditnewsletter");
        private readonly string officialemail = Environment.GetEnvironmentVariable("email");
        private readonly string sendgridUrl = Environment.GetEnvironmentVariable("sendgrid");
        private readonly string Batchid = Environment.GetEnvironmentVariable("batch_id");

        private readonly IMapper _mapper;
        private readonly Subreddits _subreddits;
        private readonly IUser _user;
        private readonly ISubreddit _reddit;
       

        public NewsLetter(IMapper _mapper, Subreddits _subreddits, IUser _user, ISubreddit _reddit)
        {
            this._mapper = _mapper;
            this._subreddits = _subreddits;
            this._reddit = _reddit;
            this._user = _user;
        }
        [HttpPost]
        public async Task<ActionResult> SendAllMessages()
        {
            var response = await UserSubreddit();
            return Ok(response);
           
        }
        private async Task<string> UserSubreddit() 
        {
            var users = _user.GetUsers.ToList();
            List<List<MessageModel>> messages = new List<List<MessageModel>>();
            var date = DateTime.Today;
            var responses = new List<string>();
            
            foreach (var user in users)
            {
                var subreddit =  _reddit.GetAllSubreddit(user.UserId).ToList();
                foreach (var item in subreddit)
                {
                    var message = await _subreddits.HotPost(item.Subreddit);
                    messages.Add(message);
                }
                var content = JsonConvert.SerializeObject(messages);
                var Content = new Content
                {
                    Type = "application/json",
                    Value = content
                };
                var x = date.AddHours(user.Hourtime);
                var unixTimeSeconds = (int)new DateTimeOffset(x).ToUnixTimeSeconds();
                var response = await SendMessage(Content, user, unixTimeSeconds);
                responses.Add(response);

                messages.Clear();
            }
            if (responses.Contains("Unsucessful")) 
            {
              return "Unable to send to all subscribers";
            }
            return "Sucessfully sent to all subscribers";
        }
        private async Task<string> SendMessage(Content content, UserModel users,int time) 
        {
            try
            {
                var client = new HttpClient();
                var url = sendgridUrl + "v3/mail/send";
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {api_key}");
                MediaTypeWithQualityHeaderValue mediaType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(mediaType);
              
                var emails = _mapper.Map<To>(users);
                var person = new Personalization
                {
                    To = new To[] {emails}
                };
                ContentModel model = new ContentModel
                {
                    Content = new Content[] { content },

                    From = new From
                    {
                        Email = officialemail
                    },
                    Subject = "Reddit NewsLetter",
                    SendAt = time,
                    BatchId = Batchid,
                    Personalizations = new Personalization[] {person}
                    
                };
                
                var json = JsonConvert.SerializeObject(model,Formatting.Indented);
                HttpResponseMessage response;
                using (var stringcontent = new StringContent(json))
                {
                  stringcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                  response = await client.PostAsync(url, stringcontent);
                }
                
                if (response.IsSuccessStatusCode) 
                {
                    return "Sucessful";
                }
                return "Unsucessful";
            }
            catch (Exception e)
            {

                return e.Message;
            } 
        }
    }
}
