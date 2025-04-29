using API.DTOs.UsersDtos;
using API.Response;
using API.Services.userServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : Controller
    {
        [HttpGet("get-users")]
        public async Task<AppResponse<List<UsersOut>>> GetAll()
        {
            return await userService.GetAllService();
        }

        [HttpGet("get-user/{id:int}")]
        public async Task<AppResponse<UsersOut>> GetById(int id)
        {            
            return await userService.GetUserService(id);   
        }

        [HttpGet("get-user/{username}")]
        public async Task<AppResponse<UsersOut>> GetByName(string username)
        {            
            return await userService.GetByUsernameService(username);
        }

        [AllowAnonymous]
        [HttpPost("create-user")]
        public async Task<AppResponse<UsersOut>> CreateUser(UserIn registerDto)
        {
            return await userService.CreateUserService(registerDto);
        }

        [HttpPost("login")]
        public async Task<AppResponse<UsersOut>> Login(loginIn loginDto)
        {
            return await userService.LoginService(loginDto);
        }

        [HttpDelete("delete-user")]
        public async Task<AppResponse<UserDeleteOut>> RemoveUser(UserDeleteIn userId)
        {
            return await userService.RemoveUserService(userId);
        }

        [HttpPut("update-user/{id:int}")]
        public async Task<AppResponse<UsersOut>> UpdateUser(int id, UserIn updatedUserDto)
        {
            return await userService.UpdateUser(id, updatedUserDto);
        }
    }
}