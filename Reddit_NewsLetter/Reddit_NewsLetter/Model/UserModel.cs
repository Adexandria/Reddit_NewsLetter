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
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
