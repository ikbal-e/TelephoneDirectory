using Contact.API.ValueObjects;

namespace Contact.API.Features.People.DTOs;

public class ContactInformationRequestDto
{
    public ContactInformationType ContactInformationType { get; set; }
    public string Value { get; set; }
}
