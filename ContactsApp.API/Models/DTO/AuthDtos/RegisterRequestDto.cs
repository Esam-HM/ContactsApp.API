﻿namespace ContactsApp.API.Models.DTO.AuthDtos
{
    public class RegisterRequestDto
    {
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
