using API.Data;
using API.Models;
using API.Response;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.RolesRepos
{
    public class RolesRepository(AppDbContext context) : IRolesRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Roles> AddRole(Roles role)
        {
            await _context.Roles.AddAsync(role);
            return role;
        }

        public async Task<Roles> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id) ?? throw new Exception("Role not found in database");
            _context.Roles.Remove(role);
            return role;
        }

        public async Task<List<Roles>> GetAllRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Roles> GetRoleById(int id)
        {
            var role = await _context.Roles.FindAsync(id) ?? throw new Exception("Role not found in database");
            return role;
        }

        public async Task<Roles> UpdateRole(int id, Roles updatedRole)
        {
            var role = await _context.Roles.FindAsync(id) ?? throw new Exception("Role not found in database");;
            
            role.RoleName = updatedRole.RoleName;

            _context.Roles.Update(role);
            return role;
        }
    }
}