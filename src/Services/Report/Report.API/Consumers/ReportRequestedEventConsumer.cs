using EventBus.IntegrationEvents;
using MassTransit;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Report.API.Entitites;
using Report.API.Infrastructure.Data;
using Report.API.Models;

namespace Report.API.Consumers;

public class ReportRequestedEventConsumer : IConsumer<ReportRequestedEvent>
{
    private readonly ReportContext _context;

    public ReportRequestedEventConsumer(ReportContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<ReportRequestedEvent> context)
    {
        var uniqueLocations = await (await _context.People.DistinctAsync<string>("Locations.Name", new BsonDocument())).ToListAsync();

        var locationReport = new List<LocationReportData>();
        foreach (var uniqueLocation in uniqueLocations)
        {
            var location = new LocationReportData
            {
                Location = uniqueLocation,
                PeopleCount = await _context.People.AsQueryable().SelectMany(x => x.Locations).Where(x => x.Name == uniqueLocation).CountAsync(),
                PhoneCount = await _context.People.AsQueryable().SelectMany(x => x.PhoneNumbers).Where(x => x.Value == uniqueLocation).CountAsync()
            };

            locationReport.Add(location);

            //TODO:
        }
    }
}
