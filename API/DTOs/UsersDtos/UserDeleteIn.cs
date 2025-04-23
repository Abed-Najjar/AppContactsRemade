using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.UsersDtos
{
    public class UserDeleteIn
    {
        [Required]
        public required int Id { get; set; }
    }
}