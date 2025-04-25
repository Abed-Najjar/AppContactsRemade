using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Repositories.UsersRepos
{
    public interface IUserRepository
    {
        Task<Users> AddUserRepository(Users user);
        Task<List<Users>> GetAllUsersRepository();
        Task<Users> GetUserByIdRepository(int id);
        Task<Users> GetUserByNameRepository(string username);
        Task<Users> GetUserByEmail(string email);
        Task<Users> UpdateUserRepository(int id, Users updatedUserDetails);
        Task<Users> DeleteUserRepository(int id);
    }
}