using Contact.API.Features.People.Commands;
using Contact.API.Features.People.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PeopleController : ControllerBase
{
    private readonly IMediator _mediator;

    public PeopleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<PersonDto>>> GetPeopleAsync(CancellationToken cancellationToken)
    {
        var people = await _mediator.Send(new GetPeopleQuery(), cancellationToken);
        return Ok(people);
    }

    [HttpPost()]
    public async Task<ActionResult<PersonDto>> CreatePersonAsync([FromBody] CreatePersonCommand createPersonCommand)
    {
        var person = await _mediator.Send(createPersonCommand);
        return CreatedAtRoute("", person);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePersonAsync([FromRoute] Guid Id)
    {
        await _mediator.Send(new DeletePersonCommand
        {
            PersonId = Id
        });

        return NoContent();
    }

    [HttpPost("{id}/ContactInformations")]
    public async Task<ActionResult<ContactInformationResponseDto>> AddContactInformation([FromRoute] Guid id, [FromBody] ContactInformationRequestDto contactInformationDto)
    {
        var contactInformation = await _mediator.Send(new AddContactInformationCommand 
        { 
            PersonId = id,
            ContactInformationType = contactInformationDto.ContactInformationType,
            Value = contactInformationDto.Value
        });

        return CreatedAtRoute("", contactInformation);
    }

    [HttpDelete("{personId}/ContactInformations/{contactInformationId}")]
    public async Task<ActionResult> AddContactInformation([FromRoute] Guid personId, [FromRoute] Guid contactInformationId)
    {
        await _mediator.Send(new DeleteContactInformationCommand
        {
            PersonId = personId,
            ContactInformationId = contactInformationId
        });

        return NoContent();
    }
}
