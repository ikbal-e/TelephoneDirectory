using EventBus.IntegrationEvents;
using MassTransit;
using MongoDB.Driver;
using Report.API.Entitites;
using Report.API.Infrastructure.Data;

namespace Report.API.Consumers;

public class ContactInfoDeletedEventConsumer : IConsumer<ContactInfoDeletedEvent>
{
    private readonly ReportContext _context;

    public ContactInfoDeletedEventConsumer(ReportContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<ContactInfoDeletedEvent> context)
    {
        var filter = Builders<Person>.Filter.Where(_ => true);
        var phoneNumbersUpdate = Builders<Person>
            .Update.PullFilter(x => x.PhoneNumbers, 
            builder => builder.ContactInformationIdOnContactService == context.Message.ContactIdOnContactService.ToString());

        var phoneResult = await _context.People.UpdateOneAsync(filter, phoneNumbersUpdate);

        if (phoneResult.MatchedCount > 0) return;

        var locationsUpdate = Builders<Person>
            .Update.PullFilter(x => x.Locations,
            builder => builder.ContactInformationIdOnContactService == context.Message.ContactIdOnContactService.ToString());

        var locationResult = await _context.People.UpdateOneAsync(filter, locationsUpdate);

    }
}
