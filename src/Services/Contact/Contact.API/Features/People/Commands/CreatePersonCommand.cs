using Contact.API.Entities;
using Contact.API.Infrastructure.Data;
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

    public CreatePersonCommandHandler(ContactContext context)
    {
        _context = context;
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

        return new()
        {
            Id = personEntity.Id,
            Name = personEntity.Name,
            Lastname = personEntity.Lastname,
            CompanyName = personEntity.CompanyName
        };
    }
}