using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Users
    {
        public int Id{ get; set; }
        public required string UserName { get; set; }
        public required string PasswordHash { get; set; } = string.Empty;
        public string? Email { get; set; }
        public ICollection<UsersRoles>? UserRoles { get; set; }
    }
}