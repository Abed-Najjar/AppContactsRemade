using System.ComponentModel.DataAnnotations;

namespace API.DTOs.RolesDtos
{
    public class RoleIn
    {
        [Required]
        public required string Rolename { get; set; }
    }
}