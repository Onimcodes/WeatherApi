using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WeatherApi.Domain.Entities;

namespace WeatherApi.Controllers
{
    /// <summary>
    /// This Controller is used to test the authentication and authorization functionality , having 
    /// endpoints  that allow only admin then users and endpoints that require both and followed by public endpoint
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Admin email example : Abrahammicheal55@gmail.com
        //Admin password: Coding@1234?
        [HttpGet("Admins")]
        [Authorize(Roles = "admin")]
        public IActionResult AdminsEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.UserName}, you are an administrator");
        }

        [HttpGet("Users")]
        [Authorize(Roles = "user")]
        //User email example:Barnabas@gmail.com
        //Password: Coding@1234
        //You can also register a new user with the register endpoint
        public IActionResult UsersEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.UserName}, you are a user");
        }

        [HttpGet("AdminsAndUsers")]
        [Authorize(Roles = "admin,User")]
        public IActionResult AdminsAndUsersEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.UserName}, you are an admin and a user");
        }

        [HttpGet("Public")]
        public IActionResult Public()
        {
            return Ok("Hi, you're on public property");
        }
        private ApplicationUser GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new ApplicationUser
                {
                    UserName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value
                };
            }
            return null;
        }
    }
}
