using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.UserRoleDtos;

public class UserRoleDeleteIn
{
    [Required]
    public required int UserId { get; set; }
    [Required]
    public required int RoleId { get; set; }
}
