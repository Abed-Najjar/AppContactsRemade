using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims; // Ensure this namespace is used for ClaimTypes
using System.Text;
using System.IO; // Add this only if needed elsewhere, but avoid conflicts
using Microsoft.IdentityModel.Tokens;
using API.Response;
using API.DTOs.UserRoleDtos;
using API.Models;
using API.Services.HashingServices;
using API.Services.tokenService;
using Microsoft.EntityFrameworkCore;

namespace API.Services.userRoleService;

public class UserRoleService(IUnitOfWork unitOfWork, IConfiguration configuration, ITokenService tokenService) : IUserRoleService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IConfiguration _configuration = configuration;

    public async Task<AppResponse<UsersRolesOut>> AddUserRoleService(UserRoleIn userRole)
    {
        if (userRole == null) return new AppResponse<UsersRolesOut>(null, "Your input fields are empty", 404, false);

        var existingEmail = await _unitOfWork.UserRepository.CheckEmailUnique(userRole.Email);
        if (existingEmail != null) return new AppResponse<UsersRolesOut>(null, "Email already exists", 400, false);

        if (userRole.RoleIds == null || !userRole.RoleIds.Any())
            return new AppResponse<UsersRolesOut>(null, "At least one role must be specified", 400, false);

        // Create a new user 
        var newUser = new Users
        {
            UserName = userRole.Username,
            Email = userRole.Email,
            PasswordHash = PasswordHasher.HashPassword(userRole.Password)
        };

        // Add the user to the database
        var createdUser = await _unitOfWork.UserRepository.AddUserRepository(newUser);
        await _unitOfWork.Complete(); // Save changes to get the user ID

        // Assign all roles to the user
        var roles = new List<RoleDto>();
        foreach (var roleId in userRole.RoleIds)
        {
            // Fetch the role from the database
            var role = await _unitOfWork.RolesRepository.GetRoleById(roleId);
            if (role == null) continue; // Skip invalid roles

            // Create the user-role association
            var userRoleAssociation = new UsersRoles
            {
                UserId = createdUser.UserId,
                RoleId = role.RolesId
            };

            await _unitOfWork.UserRoleRepository.AddUsersRolesRepository(userRoleAssociation);
            
            roles.Add(new RoleDto 
            { 
                RoleId = role.RolesId, 
                RoleName = role.RoleName 
            });
        }
        
        await _unitOfWork.Complete(); // Save all changes

        var result = new UsersRolesOut
        {
            UserId = createdUser.UserId,
            Username = createdUser.UserName,
            Email = createdUser.Email,
            Roles = roles
        };

        return new AppResponse<UsersRolesOut>(result);
    }

    public async Task<AppResponse<UsersRolesOut>> UpdateUserRoleService(UserRoleUpdateIn updatedUserRoleDetails)
    {
        if (updatedUserRoleDetails is null)
            return new(null, "Update details inputs are empty", 400, false);

        var user = await _unitOfWork.UserRepository.GetUserByIdRepository(updatedUserRoleDetails.UserId);
        if (user == null)
            return new(null, "Invalid user", 404, false);

        // Update user fields
        user.UserName = updatedUserRoleDetails.Username;
        user.PasswordHash = PasswordHasher.HashPassword(updatedUserRoleDetails.PasswordHash);
        user.Email = updatedUserRoleDetails.Email;

        // Remove existing roles
        var existingRoles = await _unitOfWork.UserRoleRepository.GetUserRolesByUserIdRepository(user.UserId);
        foreach (var existingRole in existingRoles)
        {
            await _unitOfWork.UserRoleRepository.DeleteUsersRolesRepository(existingRole);
        }

        // Assign new roles
        var roles = new List<RoleDto>();
        foreach (var roleId in updatedUserRoleDetails.RoleIds)
        {
            var role = await _unitOfWork.RolesRepository.GetRoleById(roleId);
            if (role == null) continue;

            var userRole = new UsersRoles
            {
                UserId = user.UserId,
                RoleId = role.RolesId
            };

            await _unitOfWork.UserRoleRepository.AddUsersRolesRepository(userRole);

            roles.Add(new RoleDto
            {
                RoleId = role.RolesId,
                RoleName = role.RoleName
            });
        }

        await _unitOfWork.Complete();

        return new(new UsersRolesOut
        {
            UserId = user.UserId,
            Username = user.UserName,
            Email = user.Email,
            Roles = roles
        });
    }

    public async Task<AppResponse<UserRoleDeleteOut>> DeleteUserRoleService(UserRoleDeleteIn userRole)
    {
        if (userRole is null)
            return new(null, "Your input fields are empty", 400, false);

        var existing = await _unitOfWork.UserRoleRepository.GetUserRoleRepository(userRole.UserId, userRole.RoleId);
        if (existing is null)
            return new(null, "UserRole doesn't exist", 404, false);

        var deleted = await _unitOfWork.UserRoleRepository.DeleteUsersRolesRepository(existing);
        await _unitOfWork.Complete();

        return new(new UserRoleDeleteOut
        {
            UserId = deleted.UserId,
            Username = deleted.User.UserName,
            Email = deleted.User.Email,
            RoleId = deleted.RoleId,
        });
    }

    public async Task<AppResponse<UsersRolesOut>> GetUserRoleService(int userId, int roleId)
    {
        var userRole = await _unitOfWork.UserRoleRepository.GetUserRoleRepository(userId, roleId);
        return userRole is null
            ? new(null, "UserRole does not exist", 404, false)
            : new(new UsersRolesOut
            {
                UserId = userRole.UserId,
                Username = userRole.User.UserName,
                Email = userRole.User.Email,
                Roles = new List<RoleDto>
                {
                    new RoleDto
                    {
                        RoleId = userRole.Role.RolesId,
                        RoleName = userRole.Role.RoleName
                    }
                }
            });
    }

    public async Task<AppResponse<List<UsersRolesOut>>> GetAllUserRolesService()
    {
        try {
            var users = await _unitOfWork.UserRoleRepository.GetAllUsersWithRolesRepository();
            if (users is null || users.Count == 0)
                return new(null, "No users found", 404, false);

            // Group users by their ID to handle multiple roles per user
            var usersWithRoles = users
                .Select(user => new UsersRolesOut {
                    UserId = user.UserId,
                    Username = user.UserName,
                    Email = user.Email,
                    Roles = user.UserRoles
                        .Select(ur => new RoleDto {
                            RoleId = ur.Role.RolesId,
                            RoleName = ur.Role.RoleName
                        }).ToList()
                }).ToList();

            return new(usersWithRoles);
        }
        catch (Exception ex) {
            return new(null, $"Error retrieving users with roles: {ex.Message}", 500, false);
        }
    }

    public async Task<AppResponse<UsersRolesOut>> AssignRoleToUserAsync(int userId, int roleId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdRepository(userId);
        var role = await _unitOfWork.RolesRepository.GetRoleById(roleId);
        if (user == null || role == null)
            return new(null, "User or Role not found", 404, false);

        // Check if this role is already assigned to the user
        var existingUserRole = await _unitOfWork.UserRoleRepository.GetUserRoleRepository(userId, roleId);
        if (existingUserRole != null)
            return new(null, "Role already assigned to user", 400, false);

        var newUserRole = new UsersRoles { UserId = userId, RoleId = roleId };
        await _unitOfWork.UserRoleRepository.AddUsersRolesRepository(newUserRole);
        await _unitOfWork.Complete();

        // Fetch all roles for the user to return the complete list of roles
        var userRoles = await _unitOfWork.UserRoleRepository.GetUserRolesByUserIdRepository(userId);
        var roleDtos = userRoles.Select(ur => new RoleDto { 
            RoleId = ur.Role.RolesId, 
            RoleName = ur.Role.RoleName 
        }).ToList();

        return new(new UsersRolesOut
        {
            UserId = userId,
            Username = user.UserName,
            Email = user.Email,
            Roles = roleDtos
        }, "Role successfully assigned to user", 200, true);
    }

    public async Task<AppResponse<UserRoleDeleteOut>> RemoveRoleFromUserAsync(int userId, int roleId)
    {
        var userRole = await _unitOfWork.UserRoleRepository.GetUserRoleRepository(userId, roleId);
        if (userRole is null)
            return new(null, "Role not assigned to user", 404, false);

        // Now, fetch all roles currently assigned to the user
        var RolesForUser = await _unitOfWork.UserRoleRepository.GetUserRolesByUserIdRepository(userId);
        
        if(RolesForUser.Count == 1)
            return new(null, "Cannot remove the last role from the user", 400, false);

        await _unitOfWork.UserRoleRepository.DeleteUsersRolesRepository(userRole);
        await _unitOfWork.Complete();

        // Fetch all remaining roles for the user
        var userRoles = await _unitOfWork.UserRoleRepository.GetUserRolesByUserIdRepository(userId);
        var roleDtos = userRoles.Select(ur => new RoleDto
        {
            RoleId = ur.Role.RolesId,
            RoleName = ur.Role.RoleName
        }).ToList();

        return new(new UserRoleDeleteOut
        {
            UserId = userId,
            Username = userRole.User.UserName,
            Email = userRole.User.Email,
            Roles = roleDtos
        });
    }

    public async Task<AppResponse<string>> LoginAsync(UserRoleLoginDto loginDto)
    {
        // Get user with all roles for authentication
        var user = await _unitOfWork.UserRoleRepository.GetUserByUsername(loginDto.Username);
        
        if (user == null)
            return new AppResponse<string>("User not found", 404, false);

        if (!PasswordHasher.VerifyPassword(user.PasswordHash, loginDto.Password))
            return new AppResponse<string>(null,"Wrong password", 401, false);

        if (!user.UserRoles.Any())
            return new AppResponse<string>(null, "User has no assigned roles", 403, false);

        // Get any user role for token creation (TokenService will fetch all roles)
        var userRole = user.UserRoles.First();

        return new AppResponse<string>(tokenService.CreateToken(userRole), "Login successful", 200, true);
    }

}