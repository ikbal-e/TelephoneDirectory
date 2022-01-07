using Contact.API.Features.People.Commands;
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
    public async Task<ActionResult> GetPeopleAsync(CancellationToken cancellationToken)
    {
        var people = await _mediator.Send(new GetPeopleQuery(), cancellationToken);
        return Ok(people);
    }

    [HttpPost()]
    public async Task<ActionResult> CreatePersonAsync([FromBody] CreatePersonCommand createPersonCommand)
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
}
