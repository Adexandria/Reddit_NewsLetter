using AutoMapper;
using Reddit_NewsLetter.Model;
using Reddit_NewsLetter.ViewDTO;

namespace Reddit_NewsLetter.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserModel, UserDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(s => s.Email));
            CreateMap<UserCreate, UserModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(s => s.Email));
            CreateMap<UserUpdate, UserModel>()
               .ForMember(dest => dest.Email, opt => opt.MapFrom(s => s.Email));
        }
    }
}
