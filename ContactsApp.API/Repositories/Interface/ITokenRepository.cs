using ContactsApp.API.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace ContactsApp.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(AppIdentityUser user, List<string> roles);
    }
}
