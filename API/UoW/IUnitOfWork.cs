using API.Repositories.ContactsRepos;
using API.Repositories.RolesRepos;
using API.Repositories.UserRoleRepos;
using API.Repositories.UsersRepos;

public interface IUnitOfWork
{
    IContactRepository ContactRepository { get; }
    IUserRepository UserRepository { get; }
    IRolesRepository RolesRepository { get; }
    IUserRoleRepository UserRoleRepository { get; }
    
    Task<bool> Complete();
}
