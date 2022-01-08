
namespace Report.API.Services;

public interface IDocumentService
{
    Task<string> CreateExcelFileAsync<T>(IEnumerable<T> data, string fileDirectory);
    Task<FileStream> GetGeportFileAsync(string id);
    Task<IEnumerable<Entities.Report>> GetLocationReportsAsync();
}