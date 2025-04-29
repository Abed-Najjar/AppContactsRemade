using API.DTOs.UserRoleDtos;
using API.Response;

namespace API.Services.userRoleService;

public interface IUserRoleService
{
    Task<AppResponse<UsersRolesOut>> AddUserRoleService(UserRoleIn userRole);
    Task<AppResponse<List<UsersRolesOut>>> GetAllUserRolesService();
    Task<AppResponse<UsersRolesOut>> UpdateUserRoleService(UserRoleUpdateIn updatedUserRoleDetails);
    Task<AppResponse<UserRoleDeleteOut>> DeleteUserRoleService(UserRoleDeleteIn userRole);
    Task<AppResponse<UsersRolesOut>> GetUserRoleService(int userId, int roleId);
    Task<AppResponse<UsersRolesOut>> AssignRoleToUserAsync(int userId, int roleId);
    Task<AppResponse<UserRoleDeleteOut>> RemoveRoleFromUserAsync(int userId, int roleId);
    Task<AppResponse<string>> LoginAsync(UserRoleLoginDto loginDto);
}
