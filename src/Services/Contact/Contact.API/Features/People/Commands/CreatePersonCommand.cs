using Contact.API.Entities;
using Contact.API.Infrastructure.Data;
using EventBus.IntegrationEvents;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Contact.API.Features.People.Commands;

public class CreatePersonCommand : IRequest<PersonDto>
{
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string CompanyName { get; set; }
}

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, PersonDto>
{
    private readonly ContactContext _context;
    private readonly IBus _bus;

    public CreatePersonCommandHandler(ContactContext context, IBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
    {
        public CreatePersonCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name can not be empty");
            RuleFor(x => x.Lastname).NotEmpty().WithMessage("Lastname can not be empty");
            RuleFor(x => x.CompanyName).NotEmpty().WithMessage("CompanyName can not be empty");
        }
    }

    public async Task<PersonDto> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var personEntity = new Person
        {
            Name = request.Name,
            Lastname = request.Lastname,
            CompanyName = request.CompanyName
        };

        await _context.People.AddAsync(personEntity);
        await _context.SaveChangesAsync();

        await _bus.Publish(new PersonCreatedEvent()
        {
            PersonIdOnContactService = personEntity.Id,
            Name = personEntity.Name,
            Lastname = personEntity.Lastname,
            Company = personEntity.CompanyName
        });


        return new()
        {
            Id = personEntity.Id,
            Name = personEntity.Name,
            Lastname = personEntity.Lastname,
            CompanyName = personEntity.CompanyName
        };
    }
}