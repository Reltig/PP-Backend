using PPBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PPBackend.Settings;

namespace PPBackend.Services;

public class UsersService: CRUDService<User, UserStorageSettings>
{
    public UsersService(IOptions<UserStorageSettings> storeSettings) : base(storeSettings) {}
    
    public async Task<bool> TryAddTestToUserAsync(int userId, int testId) =>
        await Task.Run(async () =>
        {
            var user = await GetAsync(userId);
            if(user is null)
                return false;
            user.AddTest(testId);
            return true;
        });

    public async Task<int> GetIdAsync(string name, string password)
    {
        var user = await _collection.Find(user => user.Name == name && user.Password == password).FirstOrDefaultAsync();
        return user.Id;
    }
    public async Task<User> GetUserAsync(string name, string password)
    {
        var user = await _collection.Find(user => user.Name == name && user.Password == password).FirstOrDefaultAsync();
        return user;
    }
    
    public async Task<List<int>> GetUserGroups(int id)
    {
        var user = await GetAsync(id);
        if (user is null)
            return null;
        return user.Groups;
    }
    
    public async Task<bool> TryDeleteTestToUserAsync(int userId, int testId) =>
        await Task.Run(async () =>
        {
            var user = await GetAsync(userId);
            if(user is null)
                return false;
            user.DeleteTest(testId);
            await UpdateAsync(userId, user);
            return true;
        });

    public async Task<bool> TryAddGroupToUserAsync(int id, int groupId) =>
        await Task.Run(async () =>
        {
            var user = await GetAsync(id);
            if(user is null)
                return false;
            user.AddToGroup(groupId);
            await UpdateAsync(id, user);
            return true;
        });

    public async Task AddManagedGroup(int userId, int groupId) =>
        await Task.Run(async () =>
        {
            var user = await GetAsync(userId);
            user.AddManagedGroups(groupId);
            await UpdateAsync(userId, user);
        });

    public async Task AddManagedTest(int userId, Test test)
    {
        await Task.Run(async () =>
        {
            var user = await GetAsync(userId);
            user.AddManagedTest(test.Id);
            await UpdateAsync(userId, user);
        });
    }
}