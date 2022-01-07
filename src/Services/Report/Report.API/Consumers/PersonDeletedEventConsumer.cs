using EventBus.IntegrationEvents;
using MassTransit;

namespace Report.API.Consumers;

public class PersonDeletedEventConsumer : IConsumer<PersonDeletedEvent>
{
    public Task Consume(ConsumeContext<PersonDeletedEvent> context)
    {
        throw new NotImplementedException();
    }
}
