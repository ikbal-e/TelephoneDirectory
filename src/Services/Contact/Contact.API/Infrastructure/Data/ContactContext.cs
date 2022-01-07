using Contact.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contact.API.Infrastructure.Data;

public class ContactContext : DbContext
{
    public ContactContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Person>()
            .HasMany(x => x.ContactInformations)
            .WithOne(x => x.Person)
            .HasForeignKey(x => x.PersonId);
    }

    public DbSet<ContactInformation> Contacts { get; set; }
    public DbSet<Person> People { get; set; }
}
