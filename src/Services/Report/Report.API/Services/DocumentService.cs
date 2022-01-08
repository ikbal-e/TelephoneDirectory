using Bogus;
using MongoDB.Driver;
using OfficeOpenXml;
using Report.API.Infrastructure.Data;
using MongoDB.Driver.Linq;
using Report.API.Models;

namespace Report.API.Services;

public class DocumentService : IDocumentService
{
    private readonly ReportContext _context;

    public DocumentService(ReportContext context)
    {
        _context = context;
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
