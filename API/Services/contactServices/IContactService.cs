using API.DTOs.ContactsDtos;
using API.Response;

namespace API.Services.contactServices
{
    public interface IContactService
    {
        Task<AppResponse<List<ContactsOut>>> GetAllService();
        Task<AppResponse<ContactsOut>> GetContactService(int id);
        Task<AppResponse<ContactsOut>> AddContactService(ContactIn contactDetails);
        Task<AppResponse<ContactsOut>> UpdateContactService(int id, ContactIn updatedContactDto);
        Task<AppResponse<ContactsOut>> DeleteContactService(int id);
    }
}