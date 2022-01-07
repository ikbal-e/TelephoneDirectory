using EventBus.IntegrationEvents;
using MassTransit;
using MongoDB.Bson;
using MongoDB.Driver;
using Report.API.Entitites;
using Report.API.Infrastructure.Data;

namespace Report.API.Consumers;

public class PersonCreatedEventConsumer : IConsumer<PersonCreatedEvent>
{
    private readonly ReportContext _context;

    public PersonCreatedEventConsumer(ReportContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<PersonCreatedEvent> context)
    {
        var person = new Person
        {
            Name = context.Message.Name,
            Lastname = context.Message.Lastname,
            Company = context.Message.Company,
            PersonIdOnContactService = context.Message.PersonIdOnContactService.ToString()
        };

        await _context.People.InsertOneAsync(person);
    }
}
