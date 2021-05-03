using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reddit_NewsLetter.Model;
using Reddit_NewsLetter.Services;
using Reddit_NewsLetter.ViewDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Reddit_NewsLetter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser user;
        private IMapper mapper;
        public  UserController(IUser user, IMapper mapper)
        {
            this.user = user;
            this.mapper = mapper;
        }
        // add new user
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Post(UserCreate createuser )
        {
            var newuser = mapper.Map<UserModel>(createuser);
            await user.AddUser(newuser);
            var link = CreateUserLink(newuser.UserId);
            var userView= mapper.Map<UserDTO>(newuser);
            userView.UpdateLink = link;
            return Created("created",userView);
        }

        // Update a user details
        
        [HttpPut("{id}",Name = "UpdateUser")]
        public async Task<ActionResult<UserDTO>> Put(Guid id,UserUpdate updateduser)
        {
            if(id == null) 
            {
                return NotFound();
            }
            var updateuser = mapper.Map<UserModel>(updateduser);
            await user.UpdateUser(updateuser, id);
            return Ok("Sucessfully Updated");
        }

        public LinkDto CreateUserLink(Guid id)
        {
            var links = new LinkDto(Url.Link("UpdateUser", new { id }),
           "update_user",
           "PUT");
            return links;

        }
        
    }
}
