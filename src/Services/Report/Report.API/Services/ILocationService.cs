
namespace Report.API.Services;

public interface ILocationService
{
    Task<string> MakeReportRequestAsync();
}