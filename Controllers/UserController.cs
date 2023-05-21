using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using nWeaveTask.BL.DTOs.Token;
using nWeaveTask.BL.DTOs.User;
using nWeaveTask.DAL.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace nWeaveTask.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public UserController(IConfiguration configuration, UserManager<User> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

   
    #region Authentication With ASP Identity
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<TokenDTO>> Login(LoginDTO credentials)
    {
        var user = await _userManager.FindByEmailAsync(credentials.Email);

        if (user == null)
        {
            return BadRequest("User not found");
        }
        if (await _userManager.IsLockedOutAsync(user))
        {
            return BadRequest("Try again");
        }

        bool isAuthenticated = await _userManager.CheckPasswordAsync(user, credentials.Password);
        if (!isAuthenticated)
        {
            await _userManager.AccessFailedAsync(user);
            return Unauthorized("Wrong Credentials");
        }


        var userClaims = await _userManager.GetClaimsAsync(user);

        //Generate Key
        var secretKey = _configuration.GetValue<string>("SecretKey");
        var secretKeyInBytes = Encoding.ASCII.GetBytes(secretKey);
        var key = new SymmetricSecurityKey(secretKeyInBytes);

        //Determine how to generate hashing result
        var methodUsedInGeneratingToken = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var exp = DateTime.Now.AddMinutes(15);

        //Genete Token 
        var jwt = new JwtSecurityToken(
            claims: userClaims,
            notBefore: DateTime.Now,
            issuer: "backendApplication",
            audience: "Products",
            expires: exp,
            signingCredentials: methodUsedInGeneratingToken);

        var tokenHandler = new JwtSecurityTokenHandler();
        string tokenString = tokenHandler.WriteToken(jwt);

        return new TokenDTO
        {
            Token = tokenString,
            ExpiryDate = exp
        };
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<string>> Register(RegisterDTO registerDTO)
    {
        var newUser = new User
        {
            UserName = registerDTO.FirstName + registerDTO.LastName,
            FirstName = registerDTO.FirstName,
            LastName = registerDTO.LastName,
            Email= registerDTO.Email,
        };

        var creationResult = await _userManager.CreateAsync(newUser, registerDTO.Password);

        if (!creationResult.Succeeded)
        {
            return BadRequest(creationResult.Errors);
        }

        var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.UserName),
                new Claim(ClaimTypes.Email, newUser.Email),
                new Claim(ClaimTypes.Role,"User"),
            };

        await _userManager.AddClaimsAsync(newUser, userClaims);

        return Ok("Done");
    }
    #endregion

}
