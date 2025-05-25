using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using recipedia.Models;

namespace recipedia.Database
{
    public class DBContext:IdentityDbContext<User>
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            
        }
        public DbSet<User> users { get; set; }
        public DbSet<Recipe> recipe { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Rename default Identity tables
            builder.Entity<User>(b => { b.ToTable("Users"); });
            builder.Entity<IdentityRole>(b => { b.ToTable("Roles"); });
            builder.Entity<IdentityUserRole<string>>(b => { b.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(b => { b.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(b => { b.ToTable("UserLogins"); });
            builder.Entity<IdentityRoleClaim<string>>(b => { b.ToTable("RoleClaims"); });
            builder.Entity<IdentityUserToken<string>>(b => { b.ToTable("UserTokens"); });
        }

    }
}
