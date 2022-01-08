using Contact.API.Entities;
using Contact.API.Features.People.DTOs;
using Contact.API.Features.People.Exceptions;
using Contact.API.Infrastructure.Data;
using Contact.API.ValueObjects;
using EventBus.IntegrationEvents;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contact.API.Features.People.Commands;

public class AddContactInformationCommand : IRequest<ContactInformationResponseDto>
{
    public Guid PersonId { get; set; }
    public ContactInformationType? ContactInformationType { get; set; }
    public string Value { get; set; }
}

public class AddContactInformationCommandValidator : AbstractValidator<AddContactInformationCommand>
{
    public AddContactInformationCommandValidator()
    {
        RuleFor(x => x.ContactInformationType).NotNull();
        RuleFor(x => x.PersonId).NotEmpty();
        RuleFor(x => x.Value).NotEmpty();
    }
}

public class AddContactInformationCommandHandler : IRequestHandler<AddContactInformationCommand, ContactInformationResponseDto>
{
    private readonly ContactContext _context;
    private readonly IBus _bus;

    public AddContactInformationCommandHandler(ContactContext context, IBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task<ContactInformationResponseDto> Handle(AddContactInformationCommand request, CancellationToken cancellationToken)
    {
        var personExists = await _context
            .People
            .AnyAsync(x => x.Id == request.PersonId);

        if (!personExists) throw new PersonNotFoundException("Person Not Found");

        var contactInfo = new ContactInformation
        {
            PersonId = request.PersonId,
            ContactInformationType = request.ContactInformationType.Value,
            Value = request.Value
        };

        await _context.Contacts.AddAsync(contactInfo);
        await _context.SaveChangesAsync();

        await _bus.Publish(new ContactInfoCreatedEvent()
        {
            PersonIdOnContactService = request.PersonId,
            ContactIdOnContactService = contactInfo.Id,
            ContactInformationType = (int)request.ContactInformationType.Value,
            Value = request.Value
        });

        return new()
        {
            ContactInformationId = contactInfo.Id,
            ContactInformationType = contactInfo.ContactInformationType,
            PersonId = request.PersonId,
            Value = contactInfo.Value
        };
    }
}
