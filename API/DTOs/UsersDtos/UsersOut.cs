using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.UsersDtos
{
    public class UsersOut
    {
        public int Id{ get; set; }
        public string Username { get; set; } = string.Empty;
        public string Passwordhash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}