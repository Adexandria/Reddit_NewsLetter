using AutoMapper;
using Reddit_NewsLetter.Model;
using Reddit_NewsLetter.ViewDTO;


namespace Reddit_NewsLetter.Profiles
{
    public class SubredditProfile :Profile
    {
        public SubredditProfile()
        {
            CreateMap<SubredditModel, SubredditDTO>()
                .ForMember(s => s.Subreddit, m => m.MapFrom(s => s.Subreddit));

            CreateMap<SubredditCreate, SubredditModel>()
               .ForMember(s => s.Subreddit, m => m.MapFrom(s => s.Subreddit));
        }
    }
}
