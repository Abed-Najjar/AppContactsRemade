using API.Models;

namespace API.Services.tokenService;

public interface ITokenService
{
  string CreateToken(UsersRoles user);
}