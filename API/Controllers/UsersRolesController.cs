using API.DTOs.UserRoleDtos;
using API.Response;
using API.Services.userRoleService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersRolesController(IUserRoleService userRoleService) : ControllerBase
    {
    private readonly IUserRoleService _userRoleService = userRoleService;

    [HttpPost("add")]
    public async Task<ActionResult<AppResponse<UsersRolesOut>>> AddUserRole([FromBody] UserRoleIn userRole)
    {
        return await _userRoleService.AddUserRoleService(userRole);
    }

    [HttpPut("update")]
    public async Task<ActionResult<AppResponse<UsersRolesOut>>> UpdateUserRole([FromBody] UserRoleIn updatedUserRole)
    {
        return await _userRoleService.UpdateUserRoleService(updatedUserRole);
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<AppResponse<UserRoleDeleteOut>>> DeleteUserRole([FromBody] UserRoleDeleteIn userRole)
    {
        return await _userRoleService.DeleteUserRoleService(userRole);
    }

    [HttpGet("get/{userId:int}/{roleId:int}")]
    public async Task<ActionResult<AppResponse<UsersRolesOut>>> GetUserRole(int userId, int roleId)
    {
        return await _userRoleService.GetUserRoleService(userId, roleId);
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<AppResponse<List<UsersRolesOut>>>> GetAllUserRoles()
    {
        return await _userRoleService.GetAllUserRolesService();
    }
    }
}
