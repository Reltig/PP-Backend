﻿using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver.Core.Operations;
using PPBackend.Settings;

namespace PPBackend.Models;

public class User:IDatabaseModel
{
    public User(){}
    public User(UserRegistrationModel urm)
    {
        Id = new Random().Next();//TODO: переделать
        Name = urm.Name;
        Password = urm.Password;
        Role = urm.Role;
    }
    [BsonId]
    public int Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = null!;
    
    [BsonElement("psw_hash")]
    public string Password { get; set; } = null!;
    
    [BsonElement("role")]
    [BsonRepresentation(BsonType.String)] 
    public Role Role { get; set; }
    
    [BsonElement("managed_groups")]
    public List<int> ManagedGroups { get; set; } = new();
    
    [BsonElement("managed_test")]
    public List<int> ManagedTests { get; set; } = new();
    
    [BsonElement("groups")]
    public List<int> Groups { get; set; } = new();
    
    [BsonElement("avaible_tests_list")]
    public List<int> AvaibleTestsIdList { get; set; } = new();
    
    [BsonElement("complited_tests")]
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
    public Dictionary<int, float> ComplitedTests { get; set; } = new();

    public void CompleteTest(int testId, float rating){
        if(!ComplitedTests.ContainsKey(testId))
            ComplitedTests.Add(testId, rating);
        if (rating > ComplitedTests[testId])
            ComplitedTests[testId] = rating;
    }

    public object GetInfo() =>
        new
        {
            Id = this.Id,
            Name = this.Name,
            Groups = this.Groups,
            ComplitedTests = this.ComplitedTests,
            ManagedGroups = this.ManagedGroups,
            AvaibleTestsIdList = this.AvaibleTestsIdList,
            ManagedTests = this.ManagedTests,
            Role = this.Role.ToString()
        };

    public void AddManagedTest(int testId)
    {
        if(!ManagedTests.Contains(testId))
            ManagedTests.Add(testId);
    }
    
    public void DeleteTest(int testId)=>AvaibleTestsIdList.Remove(testId);

    public void AddToGroup(int groupId)
    {
        if(!Groups.Contains(groupId))
            Groups.Add(groupId);
    }
    public void DeleteFromGroup(int groupId) => Groups.Remove(groupId);
    
    public void AddTest(int testId)
    {
        if(!AvaibleTestsIdList.Contains(testId))
            AvaibleTestsIdList.Add(testId);
    }
    public void AddManagedGroups(int groupId)
    {
        if(!ManagedGroups.Contains(groupId))
            ManagedGroups.Add(groupId);
    }
}

public class UserRegistrationModel
{
    public string Name { get; set; } = null!;
    
    public string Password { get; set; } = null!;
    public Role Role { get; set; }
}