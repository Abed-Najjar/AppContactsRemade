using API.Data;
using API.DTOs.ContactsDtos;
using API.Response;
using API.Services.contactServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ContactsController(IContactService contactService) : ControllerBase
    {

        [HttpGet("contacts")]
        public async Task<AppResponse<List<ContactsOut>>> GetAllContacts()
        {
            return await contactService.GetAllService();
        }

        [HttpGet("contacts/{id:int}")]
        public async Task<AppResponse<ContactsOut>> GetContact(int id)
        {
            return await contactService.GetContactService(id);
        }

        [HttpPost("add-contact")]
        public async Task<AppResponse<ContactsOut>> AddContact( [FromBody] ContactIn contactDetails)
        {
            return await contactService.AddContactService(contactDetails);
        }

        [HttpPut("update-contact")]
        public async Task<AppResponse<ContactsOut>> UpdateContact(int id, ContactIn updatedContactDetails)
        {
            return await contactService.UpdateContactService(id, updatedContactDetails);
        }


        [HttpGet("delete-contact/{id:int}")]
        public async Task<AppResponse<ContactsOut>> DeleteContact(int id)
        {
            return await contactService.DeleteContactService(id);
        }

    }
}
