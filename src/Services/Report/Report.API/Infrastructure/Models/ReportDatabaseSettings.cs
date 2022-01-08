namespace Report.API.Infrastructure.Models;

public class ReportDatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string PeopleCollectionName { get; set; }
    public string ReportsCollectionName { get; set; }
}
