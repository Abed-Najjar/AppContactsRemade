using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.UserRoleDtos;

public class UserRoleIn
{   
    [Required]
    public required string Username { get; set; }
    [Required][IsValidEmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
    [Required]
    public required List<int> RoleIds { get; set; } = new List<int>();
}
