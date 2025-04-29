using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.UsersDtos;

public class loginIn
{
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Passwordhash { get; set; }
}
