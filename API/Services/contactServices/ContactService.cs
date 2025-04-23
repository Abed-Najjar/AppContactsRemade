using API.DTOs.ContactsDtos;
using API.Models;
using API.Response;
using API.Services.contactServices;
using API.UoW;

namespace API.Services.ContactServs
{
    public class ContactService(IUnitOfWork unitOfWork) : IContactService
    {
        private readonly IUnitOfWork UnitOfWork = unitOfWork;
        public async Task<AppResponse<ContactsOut>> AddContactService(ContactIn contactDetails)
        {
            if(contactDetails == null)
                return new AppResponse<ContactsOut>(null, "Your input fields are empty", 404, false);
            
            var contact = new Contacts
            {
                UserName = contactDetails.Name,
                Email = contactDetails.Email
            };

            await UnitOfWork.ContactRepository.AddContactRepository(contact);
            await UnitOfWork.Complete();

            var result = new ContactsOut
            {
                Id = contact.Id,
                Name = contact.UserName,
                Email = contact.Email
            };

            return new AppResponse<ContactsOut>(result, "Added " + contact.UserName + " successfully", 200, true);
        }

        public async Task<AppResponse<ContactsOut>> DeleteContactService(int id)
        {
            var contact = await UnitOfWork.ContactRepository.GetContactByIdRepository(id);
            if(contact == null) return new AppResponse<ContactsOut>(null, "Contact does not exist", 404, false);

            await UnitOfWork.ContactRepository.DeleteContactRepository(contact.Id);
            await UnitOfWork.Complete();

            var result = new ContactsOut
            {
                Id = contact.Id,
                Name = contact.UserName,
                Email = contact.Email
            };

            return new AppResponse<ContactsOut>(result, "Removed " + contact.UserName + " successfully", 200, true);
        }

        public async Task<AppResponse<List<ContactsOut>>> GetAllService()
        {
            var contacts = await UnitOfWork.ContactRepository.GetAllContactsRepository();
            if(contacts.Count == 0) return new AppResponse<List<ContactsOut>>(null,"No records in contacts table", 404, false);

            var resultDto = contacts.Select(contact => new ContactsOut
            {
                Id = contact.Id,
                Name = contact.UserName,
                Email = contact.Email
            }).ToList();

            return new AppResponse<List<ContactsOut>>(resultDto);
        }

        public async Task<AppResponse<ContactsOut>> GetContactService(int id)
        {
            var contact = await UnitOfWork.ContactRepository.GetContactByIdRepository(id);
            if(contact == null) return new AppResponse<ContactsOut>(null,"Contact not found",404,false);

            var result = new ContactsOut
            {
                Id = contact.Id,
                Name = contact.UserName,
                Email = contact.Email,
            };

            return new AppResponse<ContactsOut>(result);
        }

        public async Task<AppResponse<ContactsOut>> UpdateContactService(int id, ContactIn updatedContactDto)
        {
            var contact = await UnitOfWork.ContactRepository.GetContactByIdRepository(id);
            if(contact == null) return new AppResponse<ContactsOut>(null, "Cant update, Contact not found",404,false);

            contact.UserName = updatedContactDto.Name;
            contact.Email = updatedContactDto.Email;

            await UnitOfWork.ContactRepository.UpdateContactRepository(id, contact);
            await UnitOfWork.Complete();

            var result = new ContactsOut
            {
                Id = contact.Id,
                Name = contact.UserName,
                Email = contact.Email
            };

            return new AppResponse<ContactsOut>(result ,"Updated " + contact.UserName + " successfully", 200, true);

        }
    }
}