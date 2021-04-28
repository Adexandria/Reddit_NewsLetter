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

namespace Reddit_NewsLetter.Controllers
{
    [Route("api/reddit")]
    [ApiController]
    public class RedditController : ControllerBase
    {
        private readonly string client_Id = Environment.GetEnvironmentVariable("Client_Id");
        private readonly string client_Secret = Environment.GetEnvironmentVariable("Client_Secret");
        private readonly string urlBase = Environment.GetEnvironmentVariable("AccessUrl");

        [HttpPost]
        public  ActionResult Index()
        {
            var result = GetAccessCode().Result;
            return Ok(result);

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
                return contenttype;
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }
        private string GetEncodedString(string id)
        {
            byte[] toEncodeAsBytes

              = System.Text.ASCIIEncoding.ASCII.GetBytes(id);

            string returnValue

                  = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }
    }
}

