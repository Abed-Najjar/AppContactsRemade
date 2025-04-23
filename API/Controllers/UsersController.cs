using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.UsersDtos;
using API.Response;
using API.Services.userServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
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


        [HttpPost("create-user")]
        public async Task<AppResponse<UsersOut>> CreateUser(UserIn registerDto)
        {
            return await userService.CreateUserService(registerDto);
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