using API.DTOs.RolesDtos;
using API.Response;

namespace API.Services.roleService
{
    public interface IRoleService
    {
    Task<AppResponse<RolesOut>> CreateRole(RoleIn roleDto);
    Task<AppResponse<List<RolesOut>>> GetAll();
    Task<AppResponse<RolesOut>> GetById(int id);
    Task<AppResponse<RoleDeleteOut>> RemoveRole(RoleDeleteIn role);
    Task<AppResponse<RolesOut>> UpdateRole(int id, RoleIn updatedRoleDto);
    }
}