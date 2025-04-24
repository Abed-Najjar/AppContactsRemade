using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.UsersRepos
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;
        public async Task<Users> AddUserRepository(Users user)
        {
            await _context.Users.AddAsync(user); 
            return user;  
        }

        public async Task<Users> DeleteUserRepository(int id)
        {
            var user = await _context.Users.FindAsync(id) ?? throw new Exception("Cant delete user, User not found in database"); 
            _context.Users.Remove(user);
            return user;
        }

        public async Task<List<Users>> GetAllUsersRepository()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users> GetUserByIdRepository(int id)
        {
            var user = await _context.Users.FindAsync(id) ?? throw new Exception("Cant get user, User not found in database");
            return user;
        }

        public async Task<Users> GetUserByNameRepository(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username) 
                                            ?? throw new Exception("Username does not exist");
            return user;
        }

        public async Task<Users> UpdateUserRepository(int id, Users updatedUserDetails)
        {
            var user = await _context.Users.FindAsync(id) ?? throw new Exception("Cant update, User not found in database");
            
            user.UserName = updatedUserDetails.UserName;
            user.Email = updatedUserDetails.Email;
            user.PasswordHash = updatedUserDetails.PasswordHash;

            return user;
        }
    }
}