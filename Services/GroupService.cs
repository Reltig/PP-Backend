using PPBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PPBackend.Settings;

namespace PPBackend.Services;

public class GroupService
{
    private readonly IMongoCollection<Group> _groupsCollection;

    public GroupService(
        IOptions<GroupsStorageSettings> groupsStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            groupsStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            groupsStoreDatabaseSettings.Value.DatabaseName);

        _groupsCollection = mongoDatabase.GetCollection<Group>(
            groupsStoreDatabaseSettings.Value.BooksCollectionName);
    }

    public async Task<List<Group>> GetAsync() =>
        await _groupsCollection.Find(_ => true).ToListAsync();

    public async Task<Group?> GetAsync(string id) =>
        await _groupsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Group newGroup) =>
        await _groupsCollection.InsertOneAsync(newGroup);

    public async Task UpdateAsync(string id, Group updatedGroup) =>
        await _groupsCollection.ReplaceOneAsync(x => x.Id == id, updatedGroup);

    public async Task RemoveAsync(string id) =>
        await _groupsCollection.DeleteOneAsync(x => x.Id == id);

    public async Task<bool> TryAddUserToGroupAsync(string id, string userId) =>
        await Task.Run(async () =>
        {
            var group = await GetAsync(id);
            if(group is null)
                return false;
            group.AddMember(userId);
            return true;
        });
}