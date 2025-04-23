using API.Models;

namespace API.Repositories.ContactsRepos
{
    public interface IContactRepository
    {
        Task<Contacts> AddContactRepository(Contacts contact);
        Task<List<Contacts>> GetAllContactsRepository();
        Task<Contacts> GetContactByIdRepository(int id);
        Task<Contacts> UpdateContactRepository(int id, Contacts updatedContactDetails);
        Task<Contacts> DeleteContactRepository(int id);
    }
    
}