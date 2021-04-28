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

namespace Reddit_NewsLetter.Controllers
{
    [Route("api/reddit")]
    [ApiController]
    public class RedditController : ControllerBase
    {
        private readonly string client_Id = Environment.GetEnvironmentVariable("Client_Id");
        private readonly string client_Secret = Environment.GetEnvironmentVariable("Client_Secret");
        private readonly string urlBase = Environment.GetEnvironmentVariable("AccessUrl");

        [HttpPost("search/{name}")]
        public  ActionResult Post(string name)
        {
            var result = GetAccessCode().Result;
            var subreddits = GetAvailableSubreddit(name, result);
            return Ok(subreddits);

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
                var response = await GetResponse(urlBase);
                var contenttype = await response.Content.ReadAsStringAsync();
                var accessCode = JsonConvert.DeserializeObject<ResultModel>(contenttype);
                return accessCode.AccessCode;
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }
        private async Task<string> GetRefreshCode(string token) 
        {
            try
            {
                var parameter = $"?grant_type=refresh_token&refresh_token={token}";
                var url = urlBase + parameter;
                var response = await GetResponse(url);
                var contenttype = await response.Content.ReadAsStringAsync();
                return contenttype;
            }
            catch(Exception e)
            {
                return e.Message;
            }
            
        }
        private async Task<HttpResponseMessage> GetResponse(string url) 
        {
            var client = new HttpClient();
            var Id = client_Id + ":" + client_Secret;
            var base64Credentials = GetEncodedString(Id);
            String authorizationHeader = "Basic " + base64Credentials;
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
            HttpResponseMessage response = await client.PostAsync(url, null);
            return response;
        }
        private string GetEncodedString(string id)
        {
            byte[] toEncodeAsBytes

              = ASCIIEncoding.ASCII.GetBytes(id);

            string returnValue

                  = Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }
    }
}

