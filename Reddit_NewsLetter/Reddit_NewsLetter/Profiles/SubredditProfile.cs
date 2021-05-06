using AutoMapper;
using Reddit_NewsLetter.Model;
using Reddit_NewsLetter.Model.PostModel;
using Reddit_NewsLetter.ViewDTO;


namespace Reddit_NewsLetter.Profiles
{
    public class SubredditProfile :Profile
    {
        public SubredditProfile()
        {
            CreateMap<SubredditModel, SubredditDTO>();

            CreateMap<SubredditCreate, SubredditModel>();

            CreateMap<UserModel, To>()
                .ForMember(x => x.Email, opt => opt.MapFrom(e => e.Email));

            CreateMap<Data2, MessageModel>();
               
        }
    }
}
