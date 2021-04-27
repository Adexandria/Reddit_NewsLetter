using Reddit_NewsLetter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit_NewsLetter.Services
{
    public interface IUser
    {
        Task<UserModel> AddUser(UserModel user);
        Task<UserModel> UpdateUser(UserModel updateduser,Guid id);
    }
}
