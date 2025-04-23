using API.Models;

namespace API.Repositories.RolesRepos
{
    public interface IRolesRepository
    {
    Task<Roles> AddRole(Roles role);
    Task<List<Roles>> GetAllRoles();
    Task<Roles> GetRoleById(int id);
    Task<Roles> DeleteRole(int id);
    Task<Roles> UpdateRole(int id, Roles updatedRole);
    }
}