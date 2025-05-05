using API.DTOs.UserRoleDtos;
using API.Response;
using API.Services.userRoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting; // Add this using directive

namespace API.Controllers
{
    [Authorize(Roles = "admin")] // Existing authorization
    //[EnableRateLimiting("fixed")] // Apply the 'fixed' policy defined in Program.cs
    [ApiController]
    [Route("api/[controller]")] 
    public class UsersRolesController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UsersRolesController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AppResponse<UsersRolesOut>>> AddUserRole([FromBody] UserRoleIn userRole)
        {
            return await _userRoleService.AddUserRoleService(userRole);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AppResponse<string>>> Login([FromBody] UserRoleLoginDto loginDto)
        {
            return await _userRoleService.LoginAsync(loginDto);
        }

        [HttpPut("update")]
        public async Task<ActionResult<AppResponse<UsersRolesOut>>> UpdateUserRole([FromBody] UserRoleUpdateIn updatedUserRole)
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

        [HttpPost("assign-role/{userId:int}/{roleId:int}")]
        public async Task<ActionResult<AppResponse<UsersRolesOut>>> AssignRole([FromRoute] int userId, [FromRoute] int roleId)
        {
            return await _userRoleService.AssignRoleToUserAsync(userId, roleId);
        }

        [HttpDelete("remove-role/{userId:int}/{roleId:int}")]
        public async Task<ActionResult<AppResponse<UserRoleDeleteOut>>> RemoveRole([FromRoute] int userId, [FromRoute] int roleId)
        {
            return await _userRoleService.RemoveRoleFromUserAsync(userId, roleId);
        }
    }
}
