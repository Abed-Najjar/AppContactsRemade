using API.DTOs.UserRoleDtos;
using API.Models;

namespace API.Repositories.UserRoleRepos;

public interface IUserRoleRepository
{
    Task<List<UsersRoles>> GetAllUserRolesRepository();
    Task<UsersRoles> GetUserRoleRepository(int userId, int roleId);
    Task<UsersRoles> AddUsersRolesRepository(UsersRoles user);
    Task<UsersRoles> UpdateUsersRolesRepository(UsersRoles updatedUsersRolesDetails);
    Task<UsersRoles> DeleteUsersRolesRepository(UsersRoles user);
    Task<UsersRoles?> GetUserByUsernameWithRole(string username);
}
