using Contact.API.Infrastructure.Exceptions;

namespace Contact.API.Features.People.Exceptions;

public class PersonNotFoundException : NotFoundException
{
    public PersonNotFoundException(string message) : base(message) { }
}
