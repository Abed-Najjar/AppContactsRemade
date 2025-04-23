using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.ContactsRepos
{
    public class ContactsRepository(AppDbContext context) : IContactRepository
    {
        private readonly AppDbContext _context = context;
        public async Task<Contacts> AddContactRepository(Contacts contact)
        {
            await _context.Contacts.AddAsync(contact);
            return contact;
        }

        public async Task<Contacts> DeleteContactRepository(int id)
        {
            var contact = await _context.Contacts.FindAsync(id) ?? throw new Exception("Contact not found in database");
            _context.Contacts.Remove(contact);
            return contact;
        }

        public async Task<List<Contacts>> GetAllContactsRepository()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task<Contacts> GetContactByIdRepository(int id)
        {
            var contact = await _context.Contacts.FindAsync(id) ?? throw new Exception("Contact not found in database");
            return contact;
        }

        public async Task<Contacts> UpdateContactRepository(int id, Contacts updatedContactDetails)
        {
            var contact = await _context.Contacts.FindAsync(updatedContactDetails.Id) ?? throw new Exception("Contact not found in database");

            contact.UserName = updatedContactDetails.UserName;
            contact.Email = updatedContactDetails.Email;

            _context.Contacts.Update(contact);
            return contact;
        }
    }
}