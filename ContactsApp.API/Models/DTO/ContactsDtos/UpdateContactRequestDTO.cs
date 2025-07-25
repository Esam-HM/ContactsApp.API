﻿namespace ContactsApp.API.Models.DTO.ContactsDtos
{
    public class UpdateContactRequestDTO
    {
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool IsFavorite { get; set; }
    }
}
