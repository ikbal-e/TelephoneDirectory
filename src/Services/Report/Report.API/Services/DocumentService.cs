using Bogus;
using MongoDB.Driver;
using OfficeOpenXml;
using Report.API.Infrastructure.Data;
using MongoDB.Driver.Linq;
using Report.API.Models;
using MassTransit;
using EventBus.IntegrationEvents;

namespace Report.API.Services;

public class DocumentService : IDocumentService
{
    private readonly ReportContext _context;
    private readonly IBus _bus;

    public DocumentService(ReportContext context, IBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task<string> MakeReportRequestAsync()
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

    public async Task<string> CreateExcelFileAsync<T>(IEnumerable<T> data, string fileDirectory)
    {
        var randomName = $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}-{string.Join("-", new Faker().Lorem.Words(3))}";
        var filePath = $"{Path.Combine("StaticFiles", fileDirectory, randomName)}.xlsx";

        using var p = new ExcelPackage();
        var ws = p.Workbook.Worksheets.Add("Sheet1");
        ws.Cells.LoadFromCollection(data, true);
        var file = new FileInfo(filePath);
        await p.SaveAsAsync(file);

        return filePath;
    }

    public async Task<IEnumerable<Entities.Report>> GetLocationReportsAsync()
    {
        var reports = await _context.Reports.AsQueryable().ToListAsync();

        return reports;
    }

    public async Task<FileStream> GetGeportFileAsync(string id)
    {
        var report = await (await _context.Reports.FindAsync(x => x.Id == id)).FirstOrDefaultAsync();

        return File.OpenRead(report.Path);
    }
}
