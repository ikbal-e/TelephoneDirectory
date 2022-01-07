using EventBus.IntegrationEvents;
using MassTransit;

namespace Report.API.Consumers;

public class ContactInfoDeletedEventConsumer : IConsumer<ContactInfoDeletedEvent>
{
    public Task Consume(ConsumeContext<ContactInfoDeletedEvent> context)
    {
        throw new NotImplementedException();
    }
}
