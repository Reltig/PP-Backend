using System.Security.Cryptography;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Core.Operations;
using PPBackend.Settings;

namespace PPBackend.Models;

public class User
{
    public User(){}
    public User(UserRegistrationModel urm)
    {
        Name = urm.Name;
        Password = urm.Password;
        Id = new ObjectId(
                Convert.ToHexString(
                SHA1.Create().ComputeHash(
                        (Name+Password)
                        .ToCharArray()
                        .Select(c => (byte)c)
                        .ToArray())))
            .ToString();
    }
    [BsonId]
    //[BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = null!;
    
    [BsonElement("psw_hash")]
    public string Password { get; set; } = null!;
    
    [BsonElement("avaible_tests_list")]
    public List<string> AvaibleTestsIdList { get; set; } = new();
    
    [BsonElement("complited_tests")]
    public Dictionary<string, float> ComplitedTests { get; set; } = new();

    public void AddTest(string testId) => AvaibleTestsIdList.Add(testId);

    public void CompleteTest(string testId, float rating) =>
        ComplitedTests.Add(testId, rating); //TODO: функцию обновления результата на лучший
    
    #region InDevelopment

    public static User CreateUser(string name, string password)
    {
        var user = new User();
        user.Name = name;
        var hashBytes = SHA1.HashData(password.ToCharArray().Select(c => (byte)c).ToArray());
        user.Password = System.Text.Encoding.UTF8.GetString(hashBytes);
        user.Id = ObjectId.GenerateNewId((user.Name + user.Password).ToCharArray().Sum(c => c)).ToString();
        return user;
    }

    #endregion
}

public class UserRegistrationModel
{
    public string Name { get; set; } = null!;
    
    public string Password { get; set; } = null!;
}