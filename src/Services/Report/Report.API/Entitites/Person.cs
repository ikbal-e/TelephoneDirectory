using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Report.API.Entitites;

public class Person
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string PersonIdOnContactService { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string Company { get; set; }
    public List<Location> Locations { get; set; } = new();
    public List<PhoneNumber> PhoneNumbers { get; set; } = new();
}
