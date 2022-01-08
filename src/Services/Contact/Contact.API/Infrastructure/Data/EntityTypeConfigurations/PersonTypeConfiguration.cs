using Contact.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contact.API.Infrastructure.Data.EntityTypeConfigurations;

public class PersonTypeConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasMany(x => x.ContactInformations)
            .WithOne(x => x.Person)
            .HasForeignKey(x => x.PersonId);
    }
}
