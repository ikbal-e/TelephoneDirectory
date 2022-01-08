using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Report.API.Entitites;

public class Report
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Path { get; set; }
    public DateTime RequestedAt { get; set; }
}
