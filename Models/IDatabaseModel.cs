using MongoDB.Bson.Serialization.Attributes;

namespace PPBackend.Models;

public interface IDatabaseModel
{
    [BsonId]
    public int Id { get; set; }
}