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
            try
            {
                await _context.Users.AddAsync(user); 
                return user;  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Users> DeleteUserRepository(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id) ?? throw new Exception("Cant delete user, User not found in database"); 
                _context.Users.Remove(user);
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Users>> GetAllUsersRepository()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Users> GetUserByIdRepository(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id) ?? throw new Exception("Cant get user, User not found in database");
                return user;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<Users> GetUserByNameRepository(string username)
        {

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username) 
                                            ?? throw new Exception("Username does not exist");
                return user;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<Users> GetUserByEmail(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email) 
                           ?? throw new Exception("Email does not exist");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public async Task<Users> CheckEmailUnique(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                return user!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        

        public async Task<Users> UpdateUserRepository(int id, Users updatedUserDetails)
        {

            try
            {
                var user = await _context.Users.FindAsync(id) ?? throw new Exception("Cant update, User not found in database");
                user.UserName = updatedUserDetails.UserName;
                user.Email = updatedUserDetails.Email;
                user.PasswordHash = updatedUserDetails.PasswordHash;
                return user;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public async Task<Users> LoginRepository(string username, string passwordhash)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username 
                                                             && u.PasswordHash == passwordhash);
            
            if (user == null) throw new Exception("Username or password is incorrect");
            return user;
        }
    }
}