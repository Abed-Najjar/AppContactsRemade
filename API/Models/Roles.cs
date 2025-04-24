using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Roles
    {
        public int RolesId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public ICollection<Users> Users { get; set; } = [];
        public ICollection<UsersRoles>? UserRoles { get; set; }
    }
}