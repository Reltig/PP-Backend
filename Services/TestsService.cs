using PPBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PPBackend.Settings;

namespace PPBackend.Services;

public class TestService
{
    private readonly IMongoCollection<Test> _testsCollection;

    public TestService(
        IOptions<TestsStorageSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        _testsCollection = mongoDatabase.GetCollection<Test>(
            bookStoreDatabaseSettings.Value.BooksCollectionName);
    }

    public async Task<List<Test>> GetAsync() =>
        await _testsCollection.Find(_ => true).ToListAsync();

    public async Task<Test?> GetAsync(string id) =>
        await _testsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Test newBook) =>
        await _testsCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, Test updatedBook) =>
        await _testsCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveAsync(string id) =>
        await _testsCollection.DeleteOneAsync(x => x.Id == id);
}