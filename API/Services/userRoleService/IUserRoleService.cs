using API.DTOs.UserRoleDtos;
using API.Response;

namespace API.Services.userRoleService;

public interface IUserRoleService
{
    Task<AppResponse<UsersRolesOut>> AddUserRoleService(UserRoleIn userRole);
    Task<AppResponse<List<UsersRolesOut>>> GetAllUserRolesService();
    Task<AppResponse<UsersRolesOut>> UpdateUserRoleService(UserRoleIn updatedUserRoleDetails);
    Task<AppResponse<UserRoleDeleteOut>> DeleteUserRoleService(UserRoleDeleteIn userRole);
    Task<AppResponse<UsersRolesOut>> GetUserRoleService(int userId, int roleId);
}
