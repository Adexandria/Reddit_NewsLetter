using Microsoft.EntityFrameworkCore;
using Reddit_NewsLetter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit_NewsLetter.Services
{
    public class SubredditRepository : ISubreddit
    {
        private readonly RedditDb db;
        public SubredditRepository(RedditDb db)
        {
            this.db = db;
        }


        public async Task<SubredditModel> UpdateSubreddit(SubredditModel updateSubreddit,Guid id)
        {
            var query = await GetSubreddit(id);
            if(query == null)
            {
                await AddSubreddit(updateSubreddit);
            }
            else 
            {
                db.Entry(query).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                db.Entry(updateSubreddit).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            return updateSubreddit;
        }
        private async Task<SubredditModel>GetSubreddit(Guid id) 
        {
            if (id == null)
            {
                throw new NullReferenceException(nameof(id));
            }
            return await db.Subreddit.AsQueryable().Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public IEnumerable<SubredditModel> GetAllSubreddit(Guid id) 
        {
            if(id == null) 
            {
                throw new NullReferenceException(nameof(id));
            }
            return  db.Subreddit.AsQueryable().Where(x => x.UserId == id).AsNoTracking();
        }
        private async Task<SubredditModel> AddSubreddit(SubredditModel subreddit)
        {
           if (subreddit == null)
           {
              throw new NullReferenceException(nameof(subreddit));
           }
           subreddit.Id = Guid.NewGuid();
           await db.Subreddit.AddAsync(subreddit);
           await db.SaveChangesAsync();
           return subreddit;
        }
    }
}
