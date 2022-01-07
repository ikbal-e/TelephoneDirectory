using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Report.API.Entitites;
using Report.API.Infrastructure.Models;

namespace Report.API.Infrastructure.Data;

public class ReportContext
{
    public ReportContext(IOptions<ReportDatabaseSettings> reportDatabaseSettings)
    {
        var mongoClient = new MongoClient(reportDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(reportDatabaseSettings.Value.DatabaseName);
        People = mongoDatabase.GetCollection<Person>(reportDatabaseSettings.Value.PeopleCollectionName);
    }

    public IMongoCollection<Person> People{ get; }
}
