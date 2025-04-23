namespace API.DTOs.ContactsDtos
{
    public class ContactDeleteOut
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public string? Email { get; set; }
    }
}