using Contact.API.Features.People.DTOs;
using Contact.API.Features.People.Exceptions;
using Contact.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contact.API.Features.People.Commands;

public class GetPersonDetailQuery : IRequest<PersonDetailDto>
{
    public Guid PersonId { get; set; }
}

public class GetPersonDetailQuerydHandler : IRequestHandler<GetPersonDetailQuery, PersonDetailDto>
{
    private readonly ContactContext _context;

    public GetPersonDetailQuerydHandler(ContactContext context)
    {
        _context = context;
    }

    public async Task<PersonDetailDto> Handle(GetPersonDetailQuery request, CancellationToken cancellationToken)
    {
        var person = await _context
            .People
            .Include(x => x.ContactInformations)
            .AsNoTracking()
            .Where(x => x.Id == request.PersonId)
            .FirstOrDefaultAsync(cancellationToken);

        if (person == null) throw new PersonNotFoundException("Person not found");

        return new()
        {
            Id = person.Id,
            Name = person.Name,
            Lastname = person.Lastname,
            CompanyName = person.CompanyName,
            ContactInformations = person.ContactInformations.Select(x => new ContactInformationResponseDto
            {
                ContactInformationId = x.Id,
                Value = x.Value,
                ContactInformationType = x.ContactInformationType,
                PersonId = x.PersonId
            }).ToList()
        };
    }
}
