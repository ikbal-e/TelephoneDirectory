﻿using Contact.API.Features.People.Exceptions;
using Contact.API.Infrastructure.Data;
using EventBus.IntegrationEvents;
using MassTransit;
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
    private readonly IBus _bus;

    public DeleteContactInformationCommanddHandler(ContactContext context, IBus bus)
    {
        _context = context;
        _bus = bus;
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

        await _bus.Publish(new ContactInfoDeletedEvent()
        {
            ContactIdOnContactService = request.ContactInformationId
        });

        return Unit.Value;
    }
}
