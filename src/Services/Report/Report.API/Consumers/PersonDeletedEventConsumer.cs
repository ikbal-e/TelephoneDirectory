using EventBus.IntegrationEvents;
using MassTransit;
using MongoDB.Driver;
using Report.API.Entitites;
using Report.API.Infrastructure.Data;

namespace Report.API.Consumers;

public class PersonDeletedEventConsumer : IConsumer<PersonDeletedEvent>
{
    private readonly ReportContext _context;

    public PersonDeletedEventConsumer(ReportContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<PersonDeletedEvent> context)
    {
        await _context.People.DeleteOneAsync(x => x.PersonIdOnContactService == context.Message.PersonIdOnContactService.ToString());
    }
}
