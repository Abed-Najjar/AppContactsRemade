using API.DTOs.UsersDtos;
using API.Models;
using API.Response;
using API.UoW;

namespace API.Services.userRoleService;

public class UserRoleService : IUserRoleService
{
public class UserRoleService(IUnitOfWork unitOfWork) : IUserRoleService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<AppResponse<UsersOut>> AddUserRoleService(UserIn userRole)
    {
        var result = await _unitOfWork.UserRoleRepository.AddUsersRolesRepository(userRole);
        await _unitOfWork.Complete(); // Save changes
        return result;
    }

    public async Task<UsersRoles> UpdateUserRoleService(UsersRoles updatedUserRole)
    {
        var result = await _unitOfWork.UserRoleRepository.UpdateUsersRolesRepository(updatedUserRole);
        await _unitOfWork.Complete(); // Save changes
        return result;
    }

    public async Task<UsersRoles> DeleteUserRoleService(UsersRoles userRole)
    {
        var result = await _unitOfWork.UserRoleRepository.DeleteUsersRolesRepository(userRole);
        await _unitOfWork.Complete(); // Save changes
        return result;
    }

    public async Task<UsersRoles> GetUserRoleService(int userId, int roleId)
    {
        return await _unitOfWork.UserRoleRepository.GetUserRoleRepository(userId, roleId);
    }

    public async Task<List<UsersRoles>> GetAllUserRolesService()
    {
        return await _unitOfWork.UserRoleRepository.GetAllUserRolesRepository();
    }
}

}
