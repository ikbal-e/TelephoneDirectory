using Contact.API.ValueObjects;

namespace Contact.API.Entities;

public class ContactInformation
{
    public Guid Id { get; set; }
    public ContactInformationType ContactInformationType { get; set; }
    public string Value { get; set; }
    public Guid PersonId { get; set; }
    public Person Person { get; set; }
}
