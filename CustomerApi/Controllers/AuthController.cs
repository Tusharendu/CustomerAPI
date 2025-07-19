using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CustomerApi.Models;

namespace CustomerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginModel login)
        {
            if (!((login.Username.ToLower() == "admin" && login.Password == "password") || (login.Username.ToLower() == "user" && login.Password == "password")))
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SDDFE4W34GTDE54S#$%tg4ERSV@#$reFRE45RGRED"); // Use a secure key
            //var role = "Admin"; // or "User" depending on login, for demo assume Admin
            var role = login.Username.ToLower(); 

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Subject = new ClaimsIdentity(new[] {
                //    new Claim(ClaimTypes.Name, login.Username)
                //}),
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }
    }
}
