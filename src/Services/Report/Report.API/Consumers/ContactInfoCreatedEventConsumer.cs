using EventBus.IntegrationEvents;
using MassTransit;

namespace Report.API.Consumers;

public class ContactInfoCreatedEventConsumer : IConsumer<ContactInfoCreatedEvent>
{
    public Task Consume(ConsumeContext<ContactInfoCreatedEvent> context)
    {
        throw new NotImplementedException();
    }
}
