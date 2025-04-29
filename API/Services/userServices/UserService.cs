using API.DTOs.UsersDtos;
using API.Models;
using API.Response;
using API.Services.HashingServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.userServices
{
    public class UserService(IUnitOfWork unitOfWork) : IUserService
    {   
        private readonly IUnitOfWork UnitOfWork = unitOfWork;
        public async Task<AppResponse<UsersOut>> CreateUserService(UserIn userDetails)
        {
            if(userDetails == null)
                return new AppResponse<UsersOut>(null, "Your input fields are empty", 404, false);
            
            // Check if email already exists
            var existingEmail = await UnitOfWork.UserRepository.CheckEmailUnique(userDetails.Email);
            if (existingEmail != null) return new AppResponse<UsersOut>(null, "Email already exists", 400, false);

            userDetails.Username = userDetails.Username.ToLower();
            
            var user = new Users
            {
                UserName = userDetails.Username,
                Email = userDetails.Email,
                PasswordHash = PasswordHasher.HashPassword(userDetails.Passwordhash),
            };

            await UnitOfWork.UserRepository.AddUserRepository(user);
            await UnitOfWork.Complete();

            var result = new UsersOut
            {
                Userid = user.UserId,
                Username = user.UserName,
                Email = user.Email,
            };

            return new AppResponse<UsersOut>(result);
        }

        public async Task<AppResponse<List<UsersOut>>> GetAllService()
        {
            var users = await UnitOfWork.UserRepository.GetAllUsersRepository();
            if(users == null) return new AppResponse<List<UsersOut>>(null,"No records in users table",404,false);

            var resultDto = users.Select(u => new UsersOut
            {
                Userid = u.UserId,
                Username = u.UserName,
                Email = u.Email!,
            }).ToList();

            return new AppResponse<List<UsersOut>>(resultDto);
        }

        public async Task<AppResponse<UsersOut>> GetUserService(int id)
        {
            var user = await UnitOfWork.UserRepository.GetUserByIdRepository(id);
            if(user == null) return new AppResponse<UsersOut>(null, "Cant get by id, User not found", 404, false);

            var result = new UsersOut
            {
                Userid = user.UserId,
                Username = user.UserName,
                Email = user.Email!,
            };

            return new AppResponse<UsersOut>(result);

        }

        public async Task<AppResponse<UsersOut>> GetByUsernameService([FromRoute] string username)
        {
            username = username.ToLower();
            var user = await UnitOfWork.UserRepository.GetUserByNameRepository(username);
            if(user == null) return new AppResponse<UsersOut>(null, "Cant get by name, Username not found", 404,false);

            var result = new UsersOut{
                Userid = user.UserId,
                Username = user.UserName,
                Email = user.Email!,
            };

            return new AppResponse<UsersOut>(result);
        }

        public async Task<AppResponse<UserDeleteOut>> RemoveUserService(UserDeleteIn userIn)
        {
            var user = await UnitOfWork.UserRepository.GetUserByIdRepository(userIn.Id);
            if(user == null) return new AppResponse<UserDeleteOut>(null, "Cant remove, User not found",404,false);

            await UnitOfWork.UserRepository.DeleteUserRepository(user.UserId);
            await UnitOfWork.Complete();

            var result = new UserDeleteOut
            {
                Userid = user.UserId,
                Username = user.UserName,
                Email = user.Email!
            };
            return new AppResponse<UserDeleteOut>(result,"Removed " + user.UserName + " successfully", 200, true);
            
        }

        public async Task<AppResponse<UsersOut>> UpdateUser(int id, UserIn updatedUserDto)
        {
            var user = await UnitOfWork.UserRepository.GetUserByIdRepository(id);
            if(user == null) return new AppResponse<UsersOut>(null, "Cant update, User not found",404,false);

            user.UserName = updatedUserDto.Username;
            user.Email = updatedUserDto.Email;
            user.PasswordHash = PasswordHasher.HashPassword(updatedUserDto.Passwordhash);

            await UnitOfWork.UserRepository.UpdateUserRepository(id, user);
            await UnitOfWork.Complete();

            var result = new UsersOut
            {
                Userid = user.UserId,
                Username = user.UserName,
                Email = user.Email,
            };

            return new AppResponse<UsersOut>(result ,"Updated " + user.UserName + " successfully", 200, true);
        }

        public async Task<AppResponse<UsersOut>> LoginService(loginIn userDetails)
        {
            if(userDetails == null)
                return new AppResponse<UsersOut>(null, "Your input fields are empty", 404, false);
            
            var user = await UnitOfWork.UserRepository.LoginRepository(userDetails.Username.ToLower(), userDetails.Passwordhash);
            if(user == null) return new AppResponse<UsersOut>(null, "Cant login, User not found", 404, false);

            var result = new UsersOut
            {
                Userid = user.UserId,
                Username = user.UserName,
                Email = user.Email!,
            };

            return new AppResponse<UsersOut>(result,"Logged in successfully", 200, true);
        }
    }
}