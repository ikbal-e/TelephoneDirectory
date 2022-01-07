using Contact.API.Features.People.Exceptions;
using Contact.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contact.API.Features.People.Commands;

public class DeleteContactInformationCommand : IRequest<Unit>
{
    public Guid PersonId { get; set; }
    public Guid ContactInformationId { get; set; }
}

public class DeleteContactInformationCommanddHandler : IRequestHandler<DeleteContactInformationCommand>
{
    private readonly ContactContext _context;

    public DeleteContactInformationCommanddHandler(ContactContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteContactInformationCommand request, CancellationToken cancellationToken)
    {
        var contactInfo = await _context
            .Contacts
            .Where(x => x.PersonId == request.PersonId && x.Id == request.ContactInformationId)
            .FirstOrDefaultAsync();

        if (contactInfo is null) throw new ContactNotFoundException("Contact Not Found");

        _context.Contacts.Remove(contactInfo);
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
