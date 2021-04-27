using Microsoft.EntityFrameworkCore;
using Reddit_NewsLetter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit_NewsLetter.Services
{
    public class UserRepository : IUser
    {
        private readonly RedditDb db;
        public UserRepository(RedditDb db)
        {
            this.db = db;
        }
        public async Task<UserModel> AddUser(UserModel user)
        {
            if(user == null) 
            {
                throw new NullReferenceException(nameof(user));
            }
            user.Id = Guid.NewGuid();
            await db.User.AddAsync(user);
            await db.SaveChangesAsync();
            return user;
        }
        
        public async Task<UserModel> UpdateUser(UserModel updateduser)
        {
            var query = await GetUser(updateduser.Id);
            if(query == null) 
            {
                throw new NullReferenceException(nameof(query));
            }
            var user = db.User.Attach(updateduser);
            user.State = EntityState.Modified;
            return updateduser;
        }
        private async Task<UserModel> GetUser(Guid id) 
        {
            if (id == null)
            {
                throw new NullReferenceException(nameof(id));
            }
            return await db.User.Where(s => s.Id == id).FirstOrDefaultAsync();
        }
    }
}
