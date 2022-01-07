using EventBus.IntegrationEvents;
using MassTransit;
using MongoDB.Bson;
using MongoDB.Driver;
using Report.API.Entitites;
using Report.API.Infrastructure.Data;
using Report.API.ValueObjects;

namespace Report.API.Consumers;

public class ContactInfoCreatedEventConsumer : IConsumer<ContactInfoCreatedEvent>
{
    private readonly ReportContext _context;

    public ContactInfoCreatedEventConsumer(ReportContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<ContactInfoCreatedEvent> context)
    {
        var contactInformationType = (ContactInformationType)context.Message.ContactInformationType;

        var filter = Builders<Person>
                 .Filter.Eq(e => e.PersonIdOnContactService, context.Message.PersonIdOnContactService.ToString());

        if (contactInformationType is ContactInformationType.PhoneNumber)
        {
            var phoneNumber = new PhoneNumber
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Value = context.Message.Value,
                ContactInformationIdOnContactService = context.Message.ContactIdOnContactService.ToString()
            };

            var update = Builders<Person>.Update
                    .Push(e => e.PhoneNumbers, phoneNumber);

            await _context.People.FindOneAndUpdateAsync(filter, update, new() { IsUpsert = true });

        }
        else if (contactInformationType is ContactInformationType.Location)
        {
            var location = new Location
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = context.Message.Value,
                ContactInformationIdOnContactService = context.Message.ContactIdOnContactService.ToString()
            };       

            var update = Builders<Person>.Update
                    .Push(e => e.Locations, location);

            await _context.People.FindOneAndUpdateAsync(filter, update, new() { IsUpsert = true });
        }
    }
}
