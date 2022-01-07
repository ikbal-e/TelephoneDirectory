using Contact.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contact.API.Features.People.Commands;

public class GetPeopleQuery : IRequest<IEnumerable<PersonDto>> { }

public class GetPeopleQuerydHandler : IRequestHandler<GetPeopleQuery, IEnumerable<PersonDto>>
{
    private readonly ContactContext _context;

    public GetPeopleQuerydHandler(ContactContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PersonDto>> Handle(GetPeopleQuery query, CancellationToken cancellationToken)
    {
        var people = await _context.People.Select(x => new PersonDto
        {
            Id = x.Id,
            Name = x.Name,
            Lastname = x.Lastname,
            CompanyName = x.CompanyName
        }).ToListAsync(cancellationToken: cancellationToken);

        return people;
    }
}