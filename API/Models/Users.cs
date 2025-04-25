namespace API.Models
{
public class Users
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public ICollection<UsersRoles> UserRoles { get; set; } = new List<UsersRoles>();
}
}