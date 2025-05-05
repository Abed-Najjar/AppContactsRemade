using System;
using System.Collections.Generic;

namespace API.DTOs.UserRoleDtos;

public class UsersRolesOut
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
}
