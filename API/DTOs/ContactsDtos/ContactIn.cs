using System.ComponentModel.DataAnnotations;

namespace API.DTOs.ContactsDtos
{
    public class ContactIn
    {

        [Required]
        public required string Name { get; set; }
        public string? Email { get; set; }
    }
}