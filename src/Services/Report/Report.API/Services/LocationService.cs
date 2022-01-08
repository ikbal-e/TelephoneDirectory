using EventBus.IntegrationEvents;
using MassTransit;
using Report.API.Infrastructure.Data;

namespace Report.API.Services;

public class LocationService : ILocationService
{
    private readonly ReportContext _context;
    private readonly IBus _bus;

    public LocationService(ReportContext context, IBus bus)
    {
        _context = context;
        _bus = bus;
    }


    public async Task<string> GenerateReportAsync()
    {
        var requestedAt = DateTime.UtcNow;

        var report = new Entities.Report
        {
            RequestedAt = requestedAt,
            Status = ValueObjects.ReportStatus.InProgress
        };

        await _context.Reports.InsertOneAsync(report);

        await _bus.Publish(new ReportRequestedEvent()
        {
            ReportId = report.Id,
            RequestedAt = requestedAt,
        });

        return report.Id;
    }
}
