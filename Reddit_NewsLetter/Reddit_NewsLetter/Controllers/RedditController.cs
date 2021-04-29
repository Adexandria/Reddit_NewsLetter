using Microsoft.AspNetCore.Mvc;
using RedditSharp;
using Reddit.AuthTokenRetriever.EventArgs;
using Reddit.AuthTokenRetriever;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Reddit_NewsLetter.RedditModel;
using Reddit_NewsLetter.Model;
using AutoMapper;
using Reddit_NewsLetter.ViewDTO;
using Reddit_NewsLetter.Services;

namespace Reddit_NewsLetter.Controllers
{
    [Route("api/reddit")]
    [ApiController]
    public class RedditController : ControllerBase
    {
        private readonly string client_Id = Environment.GetEnvironmentVariable("Client_Id");
        private readonly string client_Secret = Environment.GetEnvironmentVariable("Client_Secret");
        private readonly string urlBase = Environment.GetEnvironmentVariable("AccessUrl");

        private readonly IMapper mapper;
        private readonly ISubreddit reddit;
        private readonly IUser user;
        public RedditController(IMapper mapper, ISubreddit reddit, IUser user)
        {
            this.reddit = reddit;
            this.mapper = mapper;
            this.user = user;
                
        }

        [HttpPost("search/{name}")]
        public async Task<ActionResult> Post(string name)
        {
            var code = await GetAccessCode();
            var subreddits = GetAvailableSubreddit(name, code);
            return Ok(subreddits);

        }
        [HttpPost("{id}")]
        public async Task<ActionResult<SubredditDTO>> Subscribe(SubredditCreate subreddit, Guid id) 
        {
            var getUser = await user.GetUser(id);
            var link = CreateUserLink();
            if ( getUser == null) 
            {
                return NotFound($"You need to subscribe, follow this link {link.Href} using the {link.Method} method");
            }
            var code = await GetAccessCode();
            var subredditName = mapper.Map<SubredditModel>(subreddit);
            subredditName.UserId = id;
            var isAvailable = GetAvailableSubreddit(subredditName.Subreddit,code);
            if(isAvailable == null) 
            {
                return NotFound("Subreddit not available");
            }
            var subscribed = await reddit.UpdateSubreddit(subredditName, subredditName.Id);
            var subscribedView = mapper.Map<SubredditDTO>(subscribed);
            return Ok(subscribedView);
        }
        private List<string> GetAvailableSubreddit(string name,string result) 
        {
            var reddit = new Reddit.RedditClient(appId: client_Id, appSecret: client_Secret, accessToken: result);
            var subreddits = reddit.SearchSubredditNames(name);
            var subredditList = subreddits.Select(s => s.Name).ToList();
            return subredditList;
        }
        private async Task<string> GetAccessCode()
        {
            try
            {
                var client = new HttpClient();
                var Id = client_Id + ":" + client_Secret;
                var base64Credentials = GetEncodedString(Id);
                String authorizationHeader = "Basic " + base64Credentials;
                client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
                HttpResponseMessage response = await client.PostAsync(urlBase, null);
                var contenttype = await response.Content.ReadAsStringAsync();
                var accessCode = JsonConvert.DeserializeObject<ResultModel>(contenttype);
                return accessCode.AccessCode;
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }
        
        private string GetEncodedString(string id)
        {
            byte[] toEncodeAsBytes

              = ASCIIEncoding.ASCII.GetBytes(id);

            string returnValue

                  = Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }
        public LinkDto CreateUserLink()
        {
            var links = new LinkDto(Url.Link("AddUser",null),
           "Subscribe",
           "POST");
            return links;

        }
    }
}

