using Reddit_NewsLetter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit_NewsLetter.ViewDTO
{
    public class UserDTO 
    {
        public string Email { get; set; }
        public LinkDto UpdateLink { get; set; }
    }
}
