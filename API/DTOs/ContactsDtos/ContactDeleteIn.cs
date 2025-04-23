using System.ComponentModel.DataAnnotations;

namespace API.DTOs.ContactsDtos
{
    public class ContactDeleteIn
    {
        [Required]
        public required int Id { get; set; }
    }
}