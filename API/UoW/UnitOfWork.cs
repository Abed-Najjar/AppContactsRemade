using API.Data;
using API.Repositories.ContactsRepos;
using API.Repositories.RolesRepos;
using API.Repositories.UserRoleRepos;
using API.Repositories.UsersRepos;

namespace API.UoW
{
    public class UnitOfWork(AppDbContext context,
    IContactRepository contactRepository,
    IUserRepository userRepository,
    IRolesRepository rolesRepository,
    IUserRoleRepository userRoleRepository) : IUnitOfWork
    {
        public IContactRepository ContactRepository => contactRepository;
        public IUserRepository UserRepository => userRepository;
        public IRolesRepository RolesRepository => rolesRepository;
        public IUserRoleRepository UserRoleRepository => userRoleRepository;

        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }

    }
}