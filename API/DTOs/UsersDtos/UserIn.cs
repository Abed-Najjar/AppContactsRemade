using System.ComponentModel.DataAnnotations;

namespace API.DTOs.UsersDtos
{
    public class UserIn
    {
        [Required]
        public required string Username { get; set; }
        [Required][IsValidEmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Passwordhash { get; set; }
    }
}