using MongoDB.Bson.Serialization.Attributes;

namespace PPBackend.Models;

public class Group
{
    public Group(GroupRegistrationModel grm)
    {
        Id = grm.Id;
        Members = grm.Members;
        Tests = grm.Tests;
    }

    [BsonId]
    //[BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("members")]
    public List<string> Members { get; set; } = new();
    
    [BsonElement("tests")]
    public List<string> Tests { get; set; } = new();

    public void AddMember(string userId) => Members.Add(userId);
}

public class GroupRegistrationModel
{
    public string? Id { get; set; }
    
    public List<string> Members { get; set; } = new();
    
    public List<string> Tests { get; set; } = new();
}