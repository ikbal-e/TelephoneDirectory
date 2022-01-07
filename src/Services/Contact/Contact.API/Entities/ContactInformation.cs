using Contact.API.ValueObjects;

namespace Contact.API.Entities;

public class ContactInformation
{
    public ContactInformationType ContactInformationType { get; set; }
    public string Value { get; set; }
}
