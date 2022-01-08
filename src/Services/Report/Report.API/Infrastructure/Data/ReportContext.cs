using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Report.API.Entities;
using Report.API.Infrastructure.Models;

namespace Report.API.Infrastructure.Data;

public class ReportContext
{
    public ReportContext(IOptions<ReportDatabaseSettings> reportDatabaseSettings)
    {
        var mongoClient = new MongoClient(reportDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(reportDatabaseSettings.Value.DatabaseName);
        People = mongoDatabase.GetCollection<Person>(reportDatabaseSettings.Value.PeopleCollectionName);
        Reports = mongoDatabase.GetCollection<Entities.Report>(reportDatabaseSettings.Value.ReportsCollectionName);
    }

    public IMongoCollection<Person> People{ get; }
    public IMongoCollection<Entities.Report> Reports{ get; }
}
