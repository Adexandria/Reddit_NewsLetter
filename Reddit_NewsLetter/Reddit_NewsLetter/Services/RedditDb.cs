using Microsoft.EntityFrameworkCore;
using Reddit_NewsLetter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit_NewsLetter.Services
{
    public class RedditDb:DbContext
    {
        public RedditDb(DbContextOptions<RedditDb> options):base(options)
        {

        }
        public DbSet<UserModel> User { get; set; }
        public DbSet<SubredditModel> Subreddit { get; set; }
    }
}
