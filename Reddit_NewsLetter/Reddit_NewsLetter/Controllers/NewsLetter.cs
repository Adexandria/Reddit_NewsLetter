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
        
        private readonly string sendgridApi_key = Environment.GetEnvironmentVariable("redditnewsletter");
        private readonly string officialEmail = Environment.GetEnvironmentVariable("email");
        private readonly string sendgridUrl = Environment.GetEnvironmentVariable("sendgrid");
        private readonly string sendgridBatchid = Environment.GetEnvironmentVariable("batch_id");

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
            
            foreach (var user in users.Where(s=>s.Toogle==true))
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
                var dateTime = date.AddHours(user.Hourtime);
                var unixTimeSeconds = (int)new DateTimeOffset(dateTime).ToUnixTimeSeconds();
                var response = await SendMessage(Content, user, unixTimeSeconds);
                responses.Add(response);

                messages.Clear();
            }
            if (responses.Contains("Unsuccessful")) 
            {
              return "Unable to send to all subscribers";
            }
            return "Successfully sent to all subscribers";
        }
        private async Task<string> SendMessage(Content content, UserModel users,int time) 
        {
            try
            {
                var client = new HttpClient();
                var url = sendgridUrl + "v3/mail/send";
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {sendgridApi_key}");
                MediaTypeWithQualityHeaderValue mediaType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(mediaType);
              
                var userEmail = _mapper.Map<To>(users);
                var person = new Personalization
                {
                    To = new To[] {userEmail}
                };
                ContentModel model = new ContentModel
                {
                    Content = new Content[] { content },

                    From = new From
                    {
                        Email = officialEmail
                    },
                    Subject = "Reddit Daily NewsLetter",
                    SendAt = time,
                    BatchId = sendgridBatchid,
                    Personalizations = new Personalization[] {person}
                    
                };
                
                var jsonContent = JsonConvert.SerializeObject(model,Formatting.Indented);
                HttpResponseMessage response;
                using (var stringContent = new StringContent(jsonContent))
                {
                  stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                  response = await client.PostAsync(url, stringContent);
                }
                
                if (response.IsSuccessStatusCode) 
                {
                    return "Successful";
                }
                else
                {
                    return "Unsuccessful";
                }
            }
            catch (Exception e)
            {

                return e.Message;
            } 
        }
    }
}
