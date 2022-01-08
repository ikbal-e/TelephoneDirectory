using Bogus;
using OfficeOpenXml;

namespace Report.API.Services;

public class DocumentService : IDocumentService
{
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
}
