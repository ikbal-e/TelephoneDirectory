using Contact.API.ValueObjects;

namespace Contact.API.Features.People.DTOs;

public class ContactInformationResponseDto
{
    public Guid ContactInformationId { get; set; }
    public Guid PersonId { get; set; }
    public ContactInformationType ContactInformationType { get; set; }
    public string Value { get; set; }
}
