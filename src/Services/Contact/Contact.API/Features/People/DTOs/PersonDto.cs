namespace Contact.API.Features.People.Commands;

public class PersonDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string CompanyName { get; set; }
}
