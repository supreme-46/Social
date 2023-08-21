using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social.Data;
using Social.Entities;

namespace Social.Controllers
{
    [Authorize]

    public class UserController : BaseAPIController
    {
        private readonly DataContext context;

        public UserController(DataContext context) 
        {
            this.context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult <IEnumerable<AppUser>>> GetUsers()
        {
            var users = await this.context.Users.ToListAsync();
            return users;
        }
        [HttpGet("{id}")] ///api/users/1
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await this.context.Users.FindAsync(id);
            return user;
        }


    }
}
