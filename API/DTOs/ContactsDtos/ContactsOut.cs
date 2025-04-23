namespace API.DTOs.ContactsDtos
{
    public class ContactsOut
    {
        public int Id { get; set; }
        public  string Name { get; set; } = null!;
        public string? Email { get; set; }
    }
}