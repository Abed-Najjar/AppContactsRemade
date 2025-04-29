using System;

namespace API.DTOs.UserRoleDtos;

public class UserRoleDeleteOut
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? RoleName { get; set; }
}
