using API.DTOs.UserRoleDtos;
using API.Models;
using API.Response;
using API.Services.HashingServices;

namespace API.Services.userRoleService;

public class UserRoleService(IUnitOfWork unitOfWork) : IUserRoleService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AppResponse<UsersRolesOut>> AddUserRoleService(UserRoleIn userRole)
    {
        if (userRole == null) return new AppResponse<UsersRolesOut>(null, "Your input fields are empty", 404, false);

        // Fetch the existing role from the database
        var role = await _unitOfWork.RolesRepository.GetRoleById(userRole.RoleId);
        if (role == null) return new AppResponse<UsersRolesOut>(null, "Role not found", 404, false);

        // Create a new user and assign the existing role
        var userDetails = new UsersRoles
        {
            User = new Users
            {
                UserName = userRole.Username,
                Email = userRole.Email,
                PasswordHash = PasswordHasher.HashPassword(userRole.PasswordHash)
            },
            
            RoleId = role.RolesId
        };

        var createdUserRole = await _unitOfWork.UserRoleRepository.AddUsersRolesRepository(userDetails);
        await _unitOfWork.Complete(); // Save changes

        var result = new UsersRolesOut
        {
            UserId = createdUserRole.UserId,
            Username = createdUserRole.User.UserName,
            Email = createdUserRole.User.Email,
            RoleId = createdUserRole.RoleId,
            RoleName = createdUserRole.Role.RoleName
        };

        return new AppResponse<UsersRolesOut>(result);
    }

    public async Task<AppResponse<UsersRolesOut>> UpdateUserRoleService(UserRoleUpdateIn updatedUserRoleDetails)
    {
        if (updatedUserRoleDetails is null)
            return new(null, "Update details inputs are empty", 400, false);

        var role = await _unitOfWork.RolesRepository.GetRoleById(updatedUserRoleDetails.RoleId);
        if (role == null)
            return new(null, "Invalid role", 404, false);

        var user = await _unitOfWork.UserRepository.GetUserByIdRepository(updatedUserRoleDetails.UserId);
        if (user == null)
            return new(null, "Invalid user", 404, false);

        // Update user fields
        user.UserName = updatedUserRoleDetails.Username;
        user.PasswordHash = PasswordHasher.HashPassword(updatedUserRoleDetails.PasswordHash);


        var updated = new UsersRoles
        {
            UserId = user.UserId,
            User = user,
            RoleId = role.RolesId,
            Role = role,
        };

        // Save updates
        await _unitOfWork.UserRoleRepository.UpdateUsersRolesRepository(updated);
        await _unitOfWork.Complete();

        return new(new UsersRolesOut
        {
            UserId = user.UserId,
            Username = user.UserName,
            RoleId = role.RolesId,
            RoleName = role.RoleName
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
            Username = deleted.User!.UserName,
            RoleId = deleted.RoleId,
            RoleName = deleted.Role!.RoleName
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
                RoleId = userRole.RoleId,
                RoleName = userRole.Role.RoleName
            });
    }

    public async Task<AppResponse<List<UsersRolesOut>>> GetAllUserRolesService()
    {
        var users = await _unitOfWork.UserRoleRepository.GetAllUserRolesRepository();
        if (users is null || users.Count == 0)
            return new(null, "No user roles found", 404, false);

        var result = users.Select(u => new UsersRolesOut
        {
            UserId = u.UserId,
            Username = u.User.UserName,
            Email = u.User.Email,
            RoleId = u.RoleId,
            RoleName = u.Role.RoleName
        }).ToList();

        return new(result);
    }

    public async Task<AppResponse<UsersRolesOut>> AssignRoleToUserAsync(int userId, int roleId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdRepository(userId);
        if (user is null)
            return new(null, "User not found", 404, false);

        var role = await _unitOfWork.RolesRepository.GetRoleById(roleId);
        if (role is null)
            return new(null, "Role not found", 404, false);

        var alreadyAssigned = await _unitOfWork.UserRoleRepository.GetUserRoleRepository(userId, roleId);
        if (alreadyAssigned != null)
            return new(null, "Role already assigned to user", 400, false);

        var userRole = new UsersRoles { UserId = userId, RoleId = roleId };
        await _unitOfWork.UserRoleRepository.AddUsersRolesRepository(userRole);
        await _unitOfWork.Complete();

        return new(new UsersRolesOut
        {
            UserId = userId,
            Username = user.UserName,
            RoleId = roleId,
            RoleName = role.RoleName
        });
    }

    public async Task<AppResponse<UserRoleDeleteOut>> RemoveRoleFromUserAsync(int userId, int roleId)
    {
        var userRole = await _unitOfWork.UserRoleRepository.GetUserRoleRepository(userId, roleId);
        if (userRole is null)
            return new(null, "Role not assigned to user", 404, false);

        await _unitOfWork.UserRoleRepository.DeleteUsersRolesRepository(userRole);
        await _unitOfWork.Complete();

        return new(new UserRoleDeleteOut
        {
            UserId = userId,
            Username = userRole.User.UserName,
            RoleId = roleId,
            RoleName = userRole.Role.RoleName
        });
    }
}