using EventBus.IntegrationEvents;
using MassTransit;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Report.API.Infrastructure.Data;
using Report.API.Models;
using Report.API.Services;

namespace Report.API.Consumers;

public class ReportRequestedEventConsumer : IConsumer<ReportRequestedEvent>
{
    private readonly ReportContext _context;
    private readonly IDocumentService _documentService;

    public ReportRequestedEventConsumer(ReportContext context, IDocumentService documentService)
    {
        _context = context;
        _documentService = documentService;
    }

    public async Task Consume(ConsumeContext<ReportRequestedEvent> context)
    {
        var uniqueLocations =  await _context.People.AsQueryable().SelectMany(x => x.Locations).Select(x => x.Name).Distinct().ToListAsync();

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
        }

        var filePath = await _documentService.CreateExcelFileAsync(locationReport, "LocationReports");

        var filter = Builders<Entities.Report>.Filter.Eq(x => x.Id, context.Message.ReportId);
        var update = Builders<Entities.Report>.Update
            .Set(x => x.Path, filePath)
            .Set(x => x.Status, ValueObjects.ReportStatus.Ready);

        var result = await _context.Reports.UpdateOneAsync(filter, update);
    }
}
