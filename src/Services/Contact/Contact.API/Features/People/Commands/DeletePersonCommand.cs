using Contact.API.Features.People.Exceptions;
using Contact.API.Infrastructure.Data;
using MediatR;

namespace Contact.API.Features.People.Commands;

public class DeletePersonCommand
{
    public Guid PersonId { get; set; }
}

//TODO:
public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, Unit>
{
    private readonly ContactContext _context;

    public DeletePersonCommandHandler(ContactContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _context.People.FindAsync(request.PersonId);

        if (person is null) throw new PersonNotFoundException("Person Not Found");

        _context.People.Remove(person);
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}