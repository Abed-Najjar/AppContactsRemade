using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.UserRoleRepos;

public class UserRoleRepository(AppDbContext context) : IUserRoleRepository
{
    private readonly AppDbContext _context = context;
    public async Task<UsersRoles> AddUsersRolesRepository(UsersRoles user)
    {
        try
        {
            if (user == null)
            throw new ArgumentNullException(nameof(user), "User-role association cannot be null.");

            await _context.UsersRoles.AddAsync(user);
            return user;
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            Console.WriteLine($"An error occurred while adding the user role: {ex.Message}");
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

    public async Task<UsersRoles> DeleteUsersRolesRepository(UsersRoles user)
    {
        try
        {
            if(user == null) throw new Exception("Couldnt delete " + user + " from database, Not found");
            var users = await _context.UsersRoles.FirstOrDefaultAsync(u => u.UserId == user.UserId) 
                                                ?? throw new ArgumentException("Failed to delete " + user.User.UserName);

            _context.UsersRoles.Remove(users);
            return users;
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            Console.WriteLine($"An error occurred while deleting the user role: {ex.Message}");
            throw;
        }

    }

    public async Task<List<UsersRoles>> GetAllUserRolesRepository()
    {
        try
        {
            var users = await _context.UsersRoles
                                 .Include(ur => ur.User)
                                 .Include(ur => ur.Role)
                                 .ToListAsync();
            if (users == null || users.Count == 0)
                throw new Exception("No user roles found");
            
            return users;            
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            Console.WriteLine($"An error occurred while retrieving user roles: {ex.Message}");
            throw;
        }
    }

    public async Task<UsersRoles?> GetUserRoleRepository(int userId, int roleId)
    {
        try
        {
            var user = await _context.UsersRoles
                                 .Include(ur => ur.User)
                                 .Include(ur => ur.Role)
                                 .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            return user; // Return null if not found instead of throwing an exception
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            Console.WriteLine($"An error occurred while retrieving the user role: {ex.Message}");
            throw;
        }
    }

    public async Task<UsersRoles> UpdateUsersRolesRepository(UsersRoles updatedUsersRolesDetails)
    {

        try
        {
            var existingUserRole = await _context.UsersRoles
                                    .Include(ur => ur.User)
                                    .Include(ur => ur.Role)
                                    .FirstOrDefaultAsync(ur => ur.UserId == updatedUsersRolesDetails.UserId 
                                                      && ur.RoleId == updatedUsersRolesDetails.RoleId);

            if (existingUserRole == null)
                throw new Exception("UserEntry is not found");

            // Optionally update navigation properties if needed
            existingUserRole.UserId = updatedUsersRolesDetails.UserId;
            existingUserRole.User.UserName = updatedUsersRolesDetails.User.UserName;
            existingUserRole.User.Email = updatedUsersRolesDetails.User.Email;
            existingUserRole.RoleId = updatedUsersRolesDetails.RoleId;
            existingUserRole.Role.RoleName = updatedUsersRolesDetails.Role.RoleName;

            _context.UsersRoles.Update(existingUserRole);
            return existingUserRole;
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            Console.WriteLine($"An error occurred while updating the user role: {ex.Message}");
            throw;
        }
        
    }

    public async Task<UsersRoles?> GetUserByUsernameWithRole(string username)
    {
        return await _context.UsersRoles
            .Include(ur => ur.User)
            .Include(ur => ur.Role)
            .FirstOrDefaultAsync(ur => ur.User.UserName == username);
    }
    
    public async Task<Users?> GetUserByUsername(string username)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<List<UsersRoles>> GetUserRolesByUserIdRepository(int userId)
    {
        try
        {
            var userRoles = await _context.UsersRoles
                                 .Include(ur => ur.User)
                                 .Include(ur => ur.Role)
                                 .Where(ur => ur.UserId == userId)
                                 .ToListAsync();
            
            return userRoles;            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while retrieving user roles for user {userId}: {ex.Message}");
            throw;
        }
    }
    
    public async Task<List<Users>> GetAllUsersWithRolesRepository()
    {
        try
        {
            var users = await _context.Users
                                .Include(u => u.UserRoles)
                                .ThenInclude(ur => ur.Role)
                                .ToListAsync();
            
            return users;            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while retrieving all users with roles: {ex.Message}");
            throw;
        }
    }
}
