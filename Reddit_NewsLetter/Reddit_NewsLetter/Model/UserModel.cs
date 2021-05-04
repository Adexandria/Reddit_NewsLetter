using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit_NewsLetter.Model
{
    public class UserModel
    {
        [Key]
        public Guid UserId { get; set; } 
        public string Email { get; set; }
        public int Hourtime { get; set; } = 8;
        public bool Toogle { get; set; } = true;
    }
}
