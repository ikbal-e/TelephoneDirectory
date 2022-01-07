using Contact.API.Entities;
using Contact.API.Features.People.DTOs;
using Contact.API.Infrastructure.Data;
using Contact.API.ValueObjects;
using FluentValidation;
using MediatR;

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

    public AddContactInformationCommandHandler(ContactContext context)
    {
        _context = context;
    }

    public async Task<ContactInformationResponseDto> Handle(AddContactInformationCommand request, CancellationToken cancellationToken)
    {
        var contactInfo = new ContactInformation
        {
            PersonId = request.PersonId,
            ContactInformationType = request.ContactInformationType.Value,
            Value = request.Value
        };

        await _context.Contacts.AddAsync(contactInfo);
        await _context.SaveChangesAsync();

        return new()
        {
            ContactInformationId = contactInfo.Id,
            ContactInformationType = contactInfo.ContactInformationType,
            PersonId = request.PersonId,
            Value = contactInfo.Value
        };
    }
}
