using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this._repo = repo;
            this._config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
                         
                var user = await this._repo.Login(userLoginDTO.Username.ToLower(), userLoginDTO.Password);
                if (user == null)
                    return Unauthorized();

                //JWT - json web token
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Usename)
                };

                //creating security key
                var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(this._config.GetSection("AppSettings:Token").Value));

                //Encrypting key to hashing algorithm
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                //Create Token
                var tokenDescriptor = new SecurityTokenDescriptor
                {

                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new { token = tokenHandler.WriteToken(token) });
           
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserPasswordDTOs UserForRegisterDto)
        {
          
            //make sure put [FromBody] before the parameter

            //IF we are not using [ApiController] uncomment below code to display error
            //from property validation

            //if(!ModelState.IsValid)
            // return BadRequest(ModelState);

            UserForRegisterDto.Username = UserForRegisterDto.Username.ToLower();

            if (await this._repo.UserExists(UserForRegisterDto.Username))
                return BadRequest("Username already exists");
            var userToCreate = new User
            {
                Usename = UserForRegisterDto.Username
            };

            var createdUser = await this._repo.Register(userToCreate, UserForRegisterDto.Password);

            return StatusCode(201);
        }

    }

}