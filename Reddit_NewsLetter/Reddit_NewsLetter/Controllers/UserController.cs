using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reddit_NewsLetter.Model;
using Reddit_NewsLetter.Services;
using Reddit_NewsLetter.ViewDTO;
using System;
using System.Threading.Tasks;


namespace Reddit_NewsLetter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;
        private IMapper _mapper;
        public  UserController(IUser _user, IMapper _mapper)
        {
            this._user = _user;
            this._mapper = _mapper;
        }
        // add new user
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Post(UserCreate createuser )
        {
            var newuser = _mapper.Map<UserModel>(createuser);
            await _user.AddUser(newuser);
            var link = CreateUserLink(newuser.UserId);
            var userView= _mapper.Map<UserDTO>(newuser);
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
            var updateuser = _mapper.Map<UserModel>(updateduser);
            await _user.UpdateUser(updateuser, id);
            return Ok("Sucessfully Updated");
        }
        [HttpPost("Unsubscribe/{id}")]
        public async Task<ActionResult> Unsubscribe(Guid id) 
        {
            var user = await _user.GetUser(id);
            if(user == null) 
            {
                return NotFound();
            }
            await _user.UserSubscription(user);
            return Ok("Sucessfully Unsubscribed");
        }
        [HttpPost("subscribe/{id}")]
        public async Task<ActionResult> Subscribe(Guid id)
        {
            var user = await _user.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            await _user.UserSubscription(user);
            return Ok("Sucessfully Subscribed");
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
