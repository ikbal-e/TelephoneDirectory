using Contact.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contact.API.Infrastructure.Data.EntityTypeConfigurations;

public class ContactInformationTypeConfiguration : IEntityTypeConfiguration<ContactInformation>
{
    public void Configure(EntityTypeBuilder<ContactInformation> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
