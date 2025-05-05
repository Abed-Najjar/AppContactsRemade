using API.DTOs.UserRoleDtos;
using API.Models;

namespace API.Repositories.UserRoleRepos;

public interface IUserRoleRepository
{
    Task<List<UsersRoles>> GetAllUserRolesRepository();
    Task<UsersRoles?> GetUserRoleRepository(int userId, int roleId);
    Task<UsersRoles> AddUsersRolesRepository(UsersRoles user);
    Task<UsersRoles> UpdateUsersRolesRepository(UsersRoles updatedUsersRolesDetails);
    Task<UsersRoles> DeleteUsersRolesRepository(UsersRoles user);
    Task<Users> CheckEmailUnique(string email);
    Task<UsersRoles?> GetUserByUsernameWithRole(string username);
    Task<Users?> GetUserByUsername(string username);
    Task<List<UsersRoles>> GetUserRolesByUserIdRepository(int userId);
    Task<List<Users>> GetAllUsersWithRolesRepository();
}
