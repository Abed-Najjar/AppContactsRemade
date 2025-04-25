namespace API.Models
{
    public class Roles
    {
        public int RolesId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public ICollection<UsersRoles> UserRoles { get; set; } = new List<UsersRoles>();
    }
}