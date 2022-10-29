using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PPBackend.Models;

public class Test
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("test_name")]
    public string TestName { get; set; } = null!;
    
    [BsonElement("questions_list")]
    public List<Question> QuestionsList { get; set; } = new();

    public List<string> GetAnswers() =>
        QuestionsList.Select(q => q.RightAnswer).ToList();

    public async Task<float> Evaluate(List<string> answers) => //TODO: проверить
        await Task.Run(() => (float)answers.Intersect(GetAnswers()).ToList().Count / GetAnswers().Count);
}