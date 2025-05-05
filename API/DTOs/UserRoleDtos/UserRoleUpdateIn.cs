using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.UserRoleDtos;

public class UserRoleUpdateIn
{    
    [Required]
    public int UserId { get; set; }
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string PasswordHash { get; set; }
    [Required]
    public required List<int> RoleIds { get; set; } = new List<int>();
}
