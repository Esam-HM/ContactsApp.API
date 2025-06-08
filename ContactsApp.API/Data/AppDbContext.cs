using ContactsApp.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
