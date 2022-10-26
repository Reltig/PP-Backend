using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PPBackend.Models;

public class Test
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("text")]
    public string Text { get; set; } = null!;
}