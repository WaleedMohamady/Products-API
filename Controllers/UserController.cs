using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using nWeaveTask.BL.DTOs.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace nWeaveTask.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public UserController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    [Route("staticlogin")]
    public ActionResult<string> Login(LoginDTO credentials)
    {
        if (credentials.Email == "admin" && credentials.Password == "pass") 
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "User"),
                new Claim(ClaimTypes.Email, credentials.Email),
                new Claim("Nationality", "Egyptian")
            };

            //Generate Key
            var secretKey = _configuration.GetValue<string>("SecretKey");
            var secretKeyInBytes = Encoding.ASCII.GetBytes(secretKey);
            var key = new SymmetricSecurityKey(secretKeyInBytes);

            //Determine how to generate hashing result
            var methodUsedInGeneratingToken = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            //Generate Token
            var jwt = new JwtSecurityToken(
                claims: userClaims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: methodUsedInGeneratingToken);

            var tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(jwt);

            return Ok(tokenString);
        }
        return Unauthorized("Wrong Credentials");
    }
}
