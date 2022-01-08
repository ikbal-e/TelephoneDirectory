using Contact.API.Infrastructure.Exceptions;

namespace Contact.API.Features.People.Exceptions;

public class ContactNotFoundException : NotFoundException
{
    public ContactNotFoundException(string message) : base(message) { }
}
