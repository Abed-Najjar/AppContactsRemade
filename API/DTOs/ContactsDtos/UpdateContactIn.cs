using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.ContactsDtos
{
    public class UpdateContactIn
    {
        public int Id { get; set; }
        [Required]
        public required string Username { get; set; }
        public string? Email { get; set; }
    }
}