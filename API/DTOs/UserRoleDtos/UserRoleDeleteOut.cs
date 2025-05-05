using System;
using System.Collections.Generic;

namespace API.DTOs.UserRoleDtos;

public class UserRoleDeleteOut
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
}
