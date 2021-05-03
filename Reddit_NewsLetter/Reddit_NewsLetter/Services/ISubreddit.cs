using Reddit_NewsLetter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reddit_NewsLetter.Services
{
    public interface ISubreddit
    {
        IEnumerable<SubredditModel> GetAllSubreddit(Guid id);
        Task<SubredditModel> UpdateSubreddit(SubredditModel updateSubreddit,Guid id);
    }
}
