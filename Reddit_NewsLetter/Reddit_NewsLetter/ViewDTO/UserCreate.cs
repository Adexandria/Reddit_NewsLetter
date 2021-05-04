using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit_NewsLetter.ViewDTO
{
    public class UserCreate
    {
        [Required(ErrorMessage ="Enter Email")]
        [EmailAddress]
        public string Email { get; set; }
        public int Hours { get; set; }
    }
}
