using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Report.API.ValueObjects;

namespace Report.API.Entitites;

public class Report
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Path { get; set; }
    public DateTime RequestedAt { get; set; }
    public ReportStatus Status{ get; set; }
}
