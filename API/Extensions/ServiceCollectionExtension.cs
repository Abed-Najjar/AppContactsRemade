using API.Data;
using API.Repositories.ContactsRepos;
using API.Repositories.RolesRepos;
using API.Repositories.UserRoleRepos;
using API.Repositories.UsersRepos;
using API.Services.contactServices;
using API.Services.ContactServs;
using API.Services.roleService;
using API.Services.userRoleService;
using API.Services.userServices;
using API.UoW;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            // ✅ Add DbContext
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

                
            
            // Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Repositories
            services.AddScoped<IContactRepository, ContactsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRolesRepository,RolesRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();

            // Services
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserRoleService, UserRoleService>();

            //UoW
            services.AddScoped<IUnitOfWork, UnitOfWork>();



            return services;
        }
    }
}
