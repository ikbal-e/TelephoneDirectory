using Contact.API.Features.People.Exceptions;
using Contact.API.Infrastructure.Data;
using EventBus.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Contact.API.Features.People.Commands;

public class DeletePersonCommand : IRequest<Unit>
{
    public Guid PersonId { get; set; }
}

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand>
{
    private readonly ContactContext _context;
    private readonly IBus _bus;

    public DeletePersonCommandHandler(ContactContext context, IBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _context.People.FindAsync(request.PersonId);

        if (person is null) throw new PersonNotFoundException("Person Not Found");

        _context.People.Remove(person);
        await _context.SaveChangesAsync();

        await _bus.Publish(new PersonDeletedEvent()
        {
            PersonIdOnContactService = request.PersonId,
        });

        return Unit.Value;
    }
}