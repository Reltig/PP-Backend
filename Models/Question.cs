using MongoDB.Bson.Serialization.Attributes;

namespace PPBackend.Models;

public class Question
{
    [BsonElement("text")]
    public string Text { get; set; } = null!;
    
    [BsonElement("right_answer")]
    public string RightAnswer { get; set; } = null!;
    
    [BsonElement("possible_answers")]
    public List<string> PossibleAnswers { get; set; } = new();
}