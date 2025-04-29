namespace API.Models;

public class UsersRoles
{

    public int UserId { get; set; }
    public Users User { get; set; } = null!;
    public int RoleId { get; set; }
    public Roles Role { get; set; } = null!;
}
