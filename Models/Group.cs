﻿using MongoDB.Bson.Serialization.Attributes;

namespace PPBackend.Models;

public class Group : IDatabaseModel
{
    public Group(int id, GroupRegistrationModel grm)
    {
        Id = id;
        Members = grm.Members;
        Tests = grm.Tests;
    }

    [BsonId]
    public int Id { get; set; }
    
    [BsonElement("members")]
    public List<int> Members { get; set; } = new();
    
    [BsonElement("tests")]
    public List<int> Tests { get; set; } = new();

    public void AddMember(int groupId) => Members.Add(groupId);
    public void RemoveMember(int groupId) => Members.Remove(groupId);
}

public class GroupRegistrationModel
{
    public List<int> Members { get; set; } = new();
    
    public List<int> Tests { get; set; } = new();
}