using Contact.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contact.API.Infrastructure.Data;

public class ContactContext : DbContext
{
    public ContactContext(DbContextOptions options) : base(options) { }

    public DbSet<ContactInformation> Contacts { get; set; }
    public DbSet<Person> People { get; set; }
}
