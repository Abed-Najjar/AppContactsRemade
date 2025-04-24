using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.UserRoleDtos;

public class UserRoleIn
{   
    [Required]
    public int Id { get; set; }
    public required string Username { get; set; }
    [Required]
    public required string PasswordHash { get; set; }
    [Required]
    public required int RoleId { get; set; }
}
