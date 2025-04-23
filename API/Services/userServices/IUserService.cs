using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.UsersDtos;
using API.Response;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.userServices
{
    public interface IUserService
    {
        Task<AppResponse<UsersOut>> CreateUserService(UserIn userDetails);
        Task<AppResponse<List<UsersOut>>> GetAllService();
        Task<AppResponse<UsersOut>> GetUserService(int id);
        Task<AppResponse<UsersOut>> GetByUsernameService(string username);
        Task<AppResponse<UsersOut>> UpdateUser(int id, UserIn updatedUserDto);
        Task<AppResponse<UserDeleteOut>> RemoveUserService(UserDeleteIn id);
        
    }
}