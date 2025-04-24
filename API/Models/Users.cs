using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Users
    {
        public int UserId{ get; set; }
        public string UserName { get; set; }  = null!;
        public string PasswordHash { get; set; } = null!;
        public string? Email { get; set; }
        public ICollection<UsersRoles>? UserRoles { get; set; }
    }
}