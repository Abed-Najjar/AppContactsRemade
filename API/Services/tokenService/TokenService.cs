using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Services.tokenService;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _context;

    public TokenService(IConfiguration config, AppDbContext context)
    {
        _config = config;
        _context = context;
    }
    
    public string CreateToken(UsersRoles userRole)
    {
        var tokenKey = _config["Jwt:Key"] ??
         throw new Exception("Cannot access token key from appsettings");
        
        if(tokenKey.Length < 64 )
         throw new Exception("Your TokenKey needs to be longer");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        // Get all roles for this user
        var userRoles = _context.UsersRoles
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == userRole.UserId)
            .ToList();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userRole.UserId.ToString()),
            new Claim(ClaimTypes.Name, userRole.User.UserName)
        };
        
        // Add each role as a separate claim
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Role.RoleName));
        }

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