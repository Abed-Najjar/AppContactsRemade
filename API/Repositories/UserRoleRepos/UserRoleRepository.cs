using System;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.UserRoleRepos;

public class UserRoleRepository(AppDbContext context) : IUserRoleRepository
{
    private readonly AppDbContext _context = context;
    public async Task<UsersRoles> AddUsersRolesRepository(UsersRoles user)
    {
        if(user == null) throw new Exception("Couldnt add " + user + " in database");
        await _context.UsersRoles.AddAsync(user);
        return user;
    }

    public async Task<UsersRoles> DeleteUsersRolesRepository(UsersRoles user)
    {
        if(user == null) throw new Exception("Couldnt delete " + user + " from database, Not found");
        var users = await _context.UsersRoles.FirstOrDefaultAsync(u => u.UserId == user.UserId) 
                                             ?? throw new Exception("Failed to delete " + user.User!.UserName);
        _context.UsersRoles.Remove(users);
        return users;
    }

    public async Task<List<UsersRoles>> GetAllUserRolesRepository()
    {
        return await _context.UsersRoles
                             .Include(ur => ur.User)
                             .Include(ur => ur.Role)
                             .ToListAsync();
    }

    public async Task<UsersRoles> GetUserRoleRepository(int userId, int roleId)
    {
        var user = await _context.UsersRoles
                                 .Include(ur => ur.User)
                                 .Include(ur => ur.Role)
                                 .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        if(user == null) throw new Exception("Couldnt find UserRole in database");
        return user;
    }

    public async Task<UsersRoles> UpdateUsersRolesRepository(UsersRoles updatedUsersRolesDetails)
    {
        var user = await _context.UsersRoles
                                 .Where(ur => ur.UserId == updatedUsersRolesDetails.UserId && ur.RoleId == updatedUsersRolesDetails.RoleId)
                                 .FirstOrDefaultAsync();

        if (user == null)
            throw new Exception("UserRole entry not found.");

        user.Role = updatedUsersRolesDetails.Role;
        user.User = updatedUsersRolesDetails.User;

        return user;
    }

}
