using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyApi
{
    /*
    public class MyUser : IdentityUser<Guid>
    {
        public string? MyCustomProp { get; set; }
    }

    public class MyRole : IdentityRole<Guid>
    {
        public string? MyDescription { get; set; }
    }

    public class IdentityEFCoreDbContext : IdentityDbContext<MyUser, MyRole, Guid>
    {
        public IdentityEFCoreDbContext(DbContextOptions<IdentityEFCoreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MyUser>().ToTable("MyUsers");

            builder.Entity<MyRole>().ToTable("MyRoles");
        }
    }
*/
}