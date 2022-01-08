using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Report.API.Entities;

public class PhoneNumber
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ContactInformationIdOnContactService { get; set; }
    public string Value { get; set; }
}
