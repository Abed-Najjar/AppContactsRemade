using System.ComponentModel.DataAnnotations;

namespace API.DTOs.RolesDtos
{
    public class RoleDeleteIn
    {
        [Required]
        public required int Roleid { get; set; }
    }
}