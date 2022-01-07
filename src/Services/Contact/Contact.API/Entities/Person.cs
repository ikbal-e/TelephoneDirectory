namespace Contact.API.Entities;

public class Person
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string CompanyName { get; set; }
    public Guid ContactInformationId { get; set; }
    public ContactInformation ContactInformation { get; set; }
}
