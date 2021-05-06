using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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
        private readonly IMapper _mapper;
        private readonly ISubreddit _reddit;
        private readonly IUser _user;
        private readonly Subreddits _subreddit;
        public RedditController(IMapper _mapper, ISubreddit _reddit, IUser _user, Subreddits _subreddit)
        {
            this._reddit = _reddit;
            this._mapper = _mapper;
            this._user = _user;
            this._subreddit = _subreddit;
                
        }

        [HttpPost("search/{name}")]
        public async Task<ActionResult> Post(string name)
        {
            var code = await _subreddit.GetAccessCode();
            var subreddits = _subreddit.GetAvailableSubreddit(name, code);
            return Ok(subreddits);

        }
        [HttpPost("{id}")]
        public async Task<ActionResult<SubredditDTO>> Subscribe(SubredditCreate createSubreddit, Guid id) 
        {
            var getUser = await _user.GetUser(id);
            var userLink = CreateUserLink();
            if ( getUser == null) 
            {
                return NotFound($"You need to subscribe, follow this link {userLink.Href} using the {userLink.Method} method");
            }
            var accessCode = await _subreddit.GetAccessCode();
            var subreddit = _mapper.Map<SubredditModel>(createSubreddit);
            subreddit.UserId = id;
            var isAvailable = _subreddit.GetAvailableSubreddit(subreddit.Subreddit,accessCode);
            if(isAvailable == null) 
            {
                return NotFound("Subreddit not available");
            }
            var subscribedsubreddit = await _reddit.UpdateSubreddit(subreddit, subreddit.Id);
            var subscribedSubredditView = _mapper.Map<SubredditDTO>(subscribedsubreddit);
            return Ok(subscribedSubredditView);
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

