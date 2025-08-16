using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI1.Models;
using WebAPI1.Service;

namespace WebAPI1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly CommonService _commonService;

        public LoginController(IConfiguration configuration, CommonService commonService)
        {
            _config = configuration;
            _commonService = commonService;
        }

        // User loggin method
        [HttpPost("LoginUser")]
        //[AllowAnonymous]
        public IActionResult LoginUser([FromBody] LoginRequest loginRequestModel)
        {
            //if (loginRequestModel != null && loginRequestModel.Username == "shan" && loginRequestModel.Password == "1234")
            byte userId = _commonService.IsAuthenticatedUser(loginRequestModel);

            if (userId > 0)
            {
                List<Roles> roles = _commonService.GetUserRoles(userId);

                var result = GeneretateJWT(loginRequestModel, roles);

                //This will add token to response header
                Response.Headers.Append("Authorization", "Bearer " + result);

                //This will add the token to response body
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        // Token creation
        private string GeneretateJWT(LoginRequest user, List<Roles> roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            /*StringBuilder roleList = new StringBuilder();

            foreach (var role in roles)
            {
                roleList.Append(role.RoleName );
            }

            if(roles.Count > 1)
            {
                roleList.Length = roleList.Length - 1;
            }
            
            var userClaims = new[]
            {
                new Claim(ClaimTypes.UserData, user.Username!),
                new Claim(ClaimTypes.Role, roleList.ToString())
            };*/

            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.UserData, user.Username!),
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, Convert.ToString(role.RoleName) ?? ""));
            }

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                //claims: userClaims,
                claims: authClaims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
