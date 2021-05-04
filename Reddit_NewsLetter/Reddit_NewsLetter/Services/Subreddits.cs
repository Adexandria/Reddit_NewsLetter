using AutoMapper;
using Newtonsoft.Json;
using Reddit_NewsLetter.Model;
using Reddit_NewsLetter.Model.PostModel;
using Reddit_NewsLetter.RedditModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Reddit_NewsLetter.Services
{
   
    public class Subreddits
    {
        private readonly string client_Id = Environment.GetEnvironmentVariable("Client_Id");
        private readonly string client_Secret = Environment.GetEnvironmentVariable("Client_Secret");
        private readonly string redditUrl = Environment.GetEnvironmentVariable("RedditUrl");
        private readonly string urlBase = Environment.GetEnvironmentVariable("AccessUrl");

        private readonly IMapper mapper;
        public Subreddits( IMapper mapper)
        {
            
            this.mapper = mapper;
        }
        public async Task<List<MessageModel>> HotPost(string name)
        {
            try
            {
                var client = GetClient();
                var parameter = $"r/{name}/hot.json?sort=hot&limit=3";
                string urlBase = redditUrl + parameter;
                HttpResponseMessage response = await client.GetAsync(urlBase);
                var content = await response.Content.ReadAsStringAsync();
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(content);
                var data = myDeserializedClass.Data.Children.Select(data => data.Data).Where(data => data.Pinned == false).Where(data => data.Stickied == false).ToList();
                List<MessageModel> messages = mapper.Map<List<MessageModel>>(data);
                return messages;
            }
            catch (Exception)
            {

                throw;
            }


        }
        public async Task<string> GetAccessCode()
        {
            try
            {
                var client = GetClient();
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
        public List<string> GetAvailableSubreddit(string name, string result)
        {
            var reddit = new Reddit.RedditClient(appId: client_Id, appSecret: client_Secret, accessToken: result);
            var subreddits = reddit.SearchSubredditNames(name);
            var subredditList = subreddits.Select(s => s.Name).ToList();
            return subredditList;
        }
        private HttpClient GetClient()
        {
            var client = new HttpClient();
            var Id = client_Id + ":" + client_Secret;
            var base64Credentials = GetEncodedString(Id);
            String authorizationHeader = "Basic " + base64Credentials;
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
            return client;
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
