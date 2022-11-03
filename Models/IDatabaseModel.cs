using MongoDB.Bson.Serialization.Attributes;

namespace PPBackend.Models;

public interface IDatabaseModel//TODO: сделать дженериком
{
    [BsonId]
    public int Id { get; set; }
}