using EventBus.IntegrationEvents;
using MassTransit;

namespace Report.API.Consumers;

public class PersonCreatedEventConsumer : IConsumer<PersonCreatedEvent>
{
    public Task Consume(ConsumeContext<PersonCreatedEvent> context)
    {
        throw new NotImplementedException();
    }
}
