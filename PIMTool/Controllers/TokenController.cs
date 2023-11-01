using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PIMTool.Payload.Request.Authentication;
using PIMTool.Payload.Response;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PIMTool.Controllers;

[ApiController]
[Route("auth")]
public class TokenController : ControllerBase
{
    private IConfiguration _configuration;

    public TokenController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> SigninCredential(UserAuthentication userAuthentication)
    {
        if (userAuthentication != null && userAuthentication.Username != null && userAuthentication.Password != null)
        {
            var user = GetUser(userAuthentication.Username, userAuthentication.Password);
            if (user != null)
            {
                //Create claims details based on the user information
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("UserName", user.Username),
                    new Claim("Role", user.Role.ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn
                );
                return Ok(new BaseResponse(200, "Valid Credential!",
                    new JwtSecurityTokenHandler().WriteToken(token)));
            }
            else
            {
                return BadRequest("Invalid Credential!");
            }
        }
        else
        {
            return BadRequest("Null username or password");
        }
    }

    private UserAuthentication GetUser(string username, string password)
    {
        var users = new List<UserAuthentication>()
        {
            new UserAuthentication("nguyenho", "123456", Role.Admin),
            new UserAuthentication("quynhgiang", "123456", Role.Customer),
            new UserAuthentication("quynngfamily", "123456", Role.Customer)
        };
        return users.First(u => u.Username == username && u.Password == password);
    }
}