using AutoMapper;
using Reddit_NewsLetter.Model;
using Reddit_NewsLetter.ViewDTO;

namespace Reddit_NewsLetter.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserModel, UserDTO>();

            CreateMap<UserCreate, UserModel>();

            CreateMap<UserUpdate, UserModel>();
        }
    }
}
