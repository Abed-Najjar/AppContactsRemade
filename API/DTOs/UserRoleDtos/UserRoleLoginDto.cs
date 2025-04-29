using System.ComponentModel.DataAnnotations;

namespace API.DTOs.UserRoleDtos
{
    public class UserRoleLoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}