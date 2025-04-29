using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using Microsoft.IdentityModel.Tokens;

namespace API.Services.tokenService;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(UsersRoles user)
    {
        var tokenKey = config["Jwt:Key"] ??
         throw new Exception("Cannot access token key from appsettings");
        
        if(tokenKey.Length < 64 )
         throw new Exception("Your TokenKey needs to be longer");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.User.UserName),
            new Claim(ClaimTypes.Role, user.Role.RoleName) 
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescripter = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescripter);

        return tokenHandler.WriteToken(token);
    }
}