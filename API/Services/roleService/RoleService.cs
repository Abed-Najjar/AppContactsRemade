using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.RolesDtos;
using API.Models;
using API.Response;
using API.UoW;

namespace API.Services.roleService
{
    public class RoleService(IUnitOfWork unitOfWork) : IRoleService
    {
        private readonly IUnitOfWork UnitOfWork = unitOfWork;
        public async Task<AppResponse<RolesOut>> CreateRole(RoleIn roleDto)
        {
            if(roleDto == null) return new AppResponse<RolesOut>(null, "Name of role is empty", 404, false);
            
            var role = new Roles
            {
                RoleName = roleDto.Rolename,
            };

            await UnitOfWork.RolesRepository.AddRole(role);
            await UnitOfWork.Complete();
            
            var result = new RolesOut
            {
                Id = role.Id,
                Rolename = role.RoleName
            };

            return new AppResponse<RolesOut>(result);
        }

        public async Task<AppResponse<List<RolesOut>>> GetAll()
        {
            var roles = await UnitOfWork.RolesRepository.GetAllRoles();
            if(roles.Count == 0) return new AppResponse<List<RolesOut>>(null,"Roles not found",404,false);

            var result = roles.Select(r => new RolesOut{
                Id = r.Id,
                Rolename = r.RoleName
            }).ToList();

            return new AppResponse<List<RolesOut>>(result);
            
        }

        public async Task<AppResponse<RolesOut>> GetById(int id)
        {
            var role = await UnitOfWork.RolesRepository.GetRoleById(id);
            if(role == null) return new AppResponse<RolesOut>(null, "Role doesnt exist", 404, false);
            var result = new RolesOut
            {
                Id = role.Id,
                Rolename = role.RoleName
            };
            return new AppResponse<RolesOut>(result);
        }

        public async Task<AppResponse<RoleDeleteOut>> RemoveRole(RoleDeleteIn role)
        {
            var removedRole = await UnitOfWork.RolesRepository.GetRoleById(role.Id);
            if(removedRole == null) return new AppResponse<RoleDeleteOut>(null, "Cant remove, Role not found or doesnt exist",404,false);

            await UnitOfWork.RolesRepository.DeleteRole(removedRole.Id);
            await UnitOfWork.Complete();

            var result = new RoleDeleteOut
            {
                Id = removedRole.Id,
                Rolename = removedRole.RoleName
            };

            return new AppResponse<RoleDeleteOut>(result);
        }

        public async Task<AppResponse<RolesOut>> UpdateRole(int id, RoleIn updatedRoleDto)
        {
            var role = await UnitOfWork.RolesRepository.GetRoleById(id);
            if(role == null) return new AppResponse<RolesOut>(null,"Cant update, Role not found or doesnt exist",404,false);
            
            role.RoleName = updatedRoleDto.Rolename;

            await UnitOfWork.RolesRepository.UpdateRole(id, role);
            await UnitOfWork.Complete();

            var result = new RolesOut
            {
                Id = role.Id,
                Rolename = role.RoleName
            };

            return new AppResponse<RolesOut>(result);
        }
    }
}