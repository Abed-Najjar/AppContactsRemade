using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using API.DTOs.RolesDtos;
using API.Services.roleService;
using API.Response;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController(IRoleService roleService) : ControllerBase
    {
        private readonly IRoleService _roleService = roleService;

        [HttpPost]
        public async Task<ActionResult<AppResponse<RolesOut>>> CreateRole([FromBody] RoleIn roleDto)
        {
            return await _roleService.CreateRole(roleDto);
        }

        [HttpGet]
        public async Task<ActionResult<AppResponse<List<RolesOut>>>> GetAllRoles()
        {
            return await _roleService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppResponse<RolesOut>>> GetRoleById(int id)
        {
            return await _roleService.GetById(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AppResponse<RolesOut>>> UpdateRole(int id, [FromBody] RoleIn updatedRoleDto)
        {
            return await _roleService.UpdateRole(id, updatedRoleDto);
       
        }

        [HttpDelete]
        public async Task<ActionResult<AppResponse<RoleDeleteOut>>> DeleteRole([FromBody] RoleDeleteIn role)
        {
            return await _roleService.RemoveRole(role);
            
        }
    }
}
