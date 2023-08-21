using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social.Data;
using Social.DTO;
using Social.Entities;
using Social.Interfaces;
using Social.Services;
using System.Security.Cryptography;
using System.Text;

namespace Social.Controllers
{
    public class AccountController : BaseAPIController
    {
        private DataContext context;
        private readonly ITokenService tokenService;

        public AccountController(DataContext context,ITokenService _tokenService) { 
        this.context = context;
            tokenService = _tokenService;
        }

        [HttpPost("register")] //api/account/register
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.username)) 
                return BadRequest("User Exists");
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDTO.username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.password)),
                PasswordSalt = hmac.Key
            };

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();
            return new UserDTO
            {
                username = user.UserName,
                token = tokenService.CreateToken(user),
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO) 
        {
            
            var user = await this.context.Users.SingleOrDefaultAsync(x => x.UserName == loginDTO.username);
            if (user == null)
                return Unauthorized("User Not Exists");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.password));

            for(int i =0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new UserDTO
            {
                username = user.UserName,
                token = tokenService.CreateToken(user),
            }; ;
        }
        private async Task<bool> UserExists(string username)
        {
            return await this.context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
