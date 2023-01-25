using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WeatherApi.Domain.Entities;
using WeatherApi.Dtos;
using WeatherApi.Repository;

namespace WeatherApi.Controllers
{
    /// <summary>
    /// This controller is used for registering and  login with jwt security
    /// </summary>
    [Route("api/[controller]")]

    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> UserManager, SignInManager<ApplicationUser>signInManager,IConfiguration config)
        {
            userManager = UserManager;
            this.signInManager = signInManager;
            this.config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var user = await userManager.FindByEmailAsync(registerDto.EmailAddress);
            if(user != null)
            {
                return BadRequest("Email already in use");

            }

            var newUser = new ApplicationUser()
            {
                Email = registerDto.EmailAddress,
                UserName = registerDto.EmailAddress
            };

            var newUserResponse = await userManager.CreateAsync(newUser, registerDto.Password);
            if (newUserResponse.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, UserRoles.User);

                return Ok();
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            //checking if the email already exists in the database
            var user = await userManager.FindByEmailAsync(login.EmailAddress);
            if(user != null)
            {
               
                var passwordCheck = await userManager.CheckPasswordAsync(user, login.Password);
                if (passwordCheck)
                {
                    var token = Generate(user);
                    var result = await signInManager.PasswordSignInAsync(user, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Ok(token);
                    }
                }
            }
            return NotFound("User Not Found");
        }


        private string Generate(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
                
            };

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
              config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
