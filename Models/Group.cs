using MongoDB.Bson.Serialization.Attributes;

namespace PPBackend.Models;

public class Group : IDatabaseModel
{
    public Group(int id, GroupRegistrationModel grm)
    {
        Id = id;
        Name = grm.Name;
        Members = grm.Members;
        Tests = grm.Tests;
    }

    [BsonId]
    public int Id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; } = null!;
    
    [BsonElement("members")]
    public List<int> Members { get; set; } = new();
    
    [BsonElement("tests")]
    public List<int> Tests { get; set; } = new();

    public void AddMember(int groupId) => Members.Add(groupId);
    public void RemoveMember(int groupId) => Members.Remove(groupId);

    public void AddTest(int testId) => Tests.Add(testId);

    public void DeleteTest(int testId) => Tests.Remove(testId);
}

public class GroupRegistrationModel
{
    public string Name { get; set; } = null!;
    public List<int> Members { get; set; } = new();
    
    public List<int> Tests { get; set; } = new();
}