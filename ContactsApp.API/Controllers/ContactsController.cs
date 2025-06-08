using ContactsApp.API.Models.Domain;
using ContactsApp.API.Models.DTO;
using ContactsApp.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.API.Controllers
{
    // URL: https://localhost:7195/api/contacts
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IGenericRepository<Contact> contactsRepository;

        public ContactsController(IGenericRepository<Contact> contactsRepository)
        {
            this.contactsRepository = contactsRepository;
        }

        // URL: https://localhost:7195/api/contacts
        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] CreateContactRequestDto request)
        {
            var contact = new Contact
            {
                Name = request.Name,
                Surname = request.Surname,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                IsFavorite = request.IsFavorite
            };

            await contactsRepository.CreateEntityAsync(contact);

            var response = new ContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Surname = contact.Surname,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                IsFavorite = contact.IsFavorite
            };

            return Ok(response);
        }

        // URL: https://localhost:7195/api/contacts
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await contactsRepository.GetAllEntitiesAsync();
            var response = new List<ContactDto>();

            foreach (var contact in contacts)
            {
                response.Add(new ContactDto
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Surname = contact.Surname,
                    PhoneNumber = contact.PhoneNumber,
                    Email = contact.Email,
                    IsFavorite = contact.IsFavorite
                });
            }

            return Ok(response);
        }

        // URL: https://localhost:7195/api/contacts/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var contact = await contactsRepository.GetEntityByIdAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            var contactDto = new ContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Surname = contact.Surname,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                IsFavorite = contact.IsFavorite
            };

            return Ok(contactDto);
        }

        // URL: https://localhost:7195/api/contacts
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateContact([FromRoute] int id, [FromBody] UpdateContactRequestDTO request)
        {
            var contact = new Contact()
            {
                Id = id,
                Name = request.Name,
                Surname = request.Surname,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                IsFavorite = request.IsFavorite
            };

            var updatedContact = await contactsRepository.UpdateEntityAsync(contact);

            if (updatedContact == null)
            {
                return NotFound();
            }

            var updatedContactDto = new ContactDto()
            {
                Id = updatedContact.Id,
                Name = updatedContact.Name,
                Surname = updatedContact.Surname,
                PhoneNumber = updatedContact.PhoneNumber,
                Email = updatedContact.Email,
                IsFavorite = updatedContact.IsFavorite
            };

            return Ok(updatedContactDto);
        }

        // URL: https://localhost:7195/api/contacts/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteContact([FromRoute] int id)
        {
            var deletedContact = await contactsRepository.DeleteEntityAsync(id);

            if(deletedContact == null)
            {
                return NotFound();
            }

            var deletedContactDto = new ContactDto()
            {
                Id = deletedContact.Id,
                Name = deletedContact.Name,
                Surname = deletedContact.Surname,
                PhoneNumber = deletedContact.PhoneNumber,
                Email = deletedContact.Email,
                IsFavorite = deletedContact.IsFavorite
            };

            return Ok(deletedContactDto);
        }

    }
}
