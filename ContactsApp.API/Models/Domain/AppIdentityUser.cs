using Microsoft.AspNetCore.Identity;

namespace ContactsApp.API.Models.Domain
{
    public class AppIdentityUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
