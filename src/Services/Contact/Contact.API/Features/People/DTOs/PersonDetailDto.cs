namespace Contact.API.Features.People.DTOs;

public class PersonDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string CompanyName { get; set; }
    public List<ContactInformationResponseDto> ContactInformations { get; set; } = new();
}
