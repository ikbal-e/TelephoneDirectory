
namespace Report.API.Services;

public interface IDocumentService
{
    Task<string> CreateExcelFileAsync<T>(IEnumerable<T> data, string fileDirectory);
    Task<IEnumerable<Entities.Report>> GetLocationReportsAsync();
}