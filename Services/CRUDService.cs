using PPBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PPBackend.Settings;

namespace PPBackend.Services;

public class CRUDService<T, TStorageSettings> 
    where T:IDatabaseModel
    where TStorageSettings:DataBaseSettings
{
    protected readonly IMongoCollection<T> _collection;

    public CRUDService(
        IOptions<TStorageSettings> storeSettings)
    {
        var mongoClient = new MongoClient(
            storeSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            storeSettings.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<T>(
            storeSettings.Value.CollectionName);
    }

    public async Task<List<T>> GetAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<T?> GetAsync(int id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    
    public async Task<List<T>> GetAsync(int[] ids) =>
        await _collection.Find(x => ids.Contains(x.Id)).ToListAsync();

    public async Task CreateAsync(T newItem) =>
        await _collection.InsertOneAsync(newItem);

    public async Task UpdateAsync(int id, T newItem) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, newItem);

    public async Task RemoveAsync(int id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}