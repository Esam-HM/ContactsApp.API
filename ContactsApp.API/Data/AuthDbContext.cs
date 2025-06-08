using ContactsApp.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Data
{
    public class AuthDbContext : IdentityDbContext<AppIdentityUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "cc36ff5e-d67f-4b38-958d-05f4fbc79052";
            var writerRoleId = "c046cac2-15d8-4181-965c-66f772c84a81";

            var roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = readerRoleId,          // reader role
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,          // writer role
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
