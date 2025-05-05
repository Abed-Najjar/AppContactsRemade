using System.Text;
using API.Data;
using API.Repositories.ContactsRepos;
using API.Repositories.RolesRepos;
using API.Repositories.UserRoleRepos;
using API.Repositories.UsersRepos;
using API.Services.contactServices;
using API.Services.ContactServs;
using API.Services.roleService;
using API.Services.tokenService;
using API.Services.userRoleService;
using API.Services.userServices;
using API.UoW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;

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

            // Add Rate Limiter services
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter(policyName: "fixed", opt =>
                {
                    opt.PermitLimit = 5; // Max 5 requests
                    opt.Window = TimeSpan.FromSeconds(30); // per 30 seconds
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 0; // Set queue limit to 0 for immediate rejection
                });
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                // Simplified OnRejected handler
                options.OnRejected = (context, cancellationToken) =>
                {
                    // We will rely on RejectionStatusCode to send the 429.
                    return new ValueTask();
                };
            });

             // ✅ 2. Configure JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
                };
            });


            services.AddSwaggerGen(c =>
            {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            // Add JWT Authentication support in Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
            
            // Add this line to your service registrations
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
