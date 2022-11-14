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
}