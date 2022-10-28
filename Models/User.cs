using System.Security.Cryptography;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Core.Operations;
using PPBackend.Settings;

namespace PPBackend.Models;

public class User
{
    [BsonId]
    //[BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = null!;
    
    [BsonElement("psw_hash")]
    public string Password { get; set; } = null!;

    public static User CreateUser(string name, string password)
    {
        var user = new User();
        user.Name = name;
        var hashBytes = SHA1.HashData(password.ToCharArray().Select(c => (byte)c).ToArray());
        user.Password = System.Text.Encoding.UTF8.GetString(hashBytes);
        user.Id = ObjectId.GenerateNewId((user.Name + user.Password).ToCharArray().Sum(c => c)).ToString();
        return user;
    }
}