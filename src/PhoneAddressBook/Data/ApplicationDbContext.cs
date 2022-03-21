namespace PhoneAddressBook.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using PhoneAddressBook.Data.Models;

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Record> Records { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<CountriesDataHash> CountriesDataHash { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Record>()
                .HasIndex(x => x.FirstName);
            builder.Entity<Record>()
                .HasIndex(x => x.LastName);
            builder.Entity<Record>()
                .HasIndex(x => x.PhoneNumber);

            builder.Entity<Country>()
            .HasIndex(x => x.PhoneCode);

            base.OnModelCreating(builder);
        }
    }
}
