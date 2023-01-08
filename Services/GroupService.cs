using PPBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PPBackend.Settings;

namespace PPBackend.Services;

public class GroupService : CRUDService<Group, GroupsStorageSettings>
{

    public GroupService(IOptions<GroupsStorageSettings> storeSettings) : base(storeSettings)
    {
    }

    public async Task<bool> TryAddUserToGroupAsync(int groupId, int userId) =>
        await Task.Run(async () =>
        {
            var group = await GetAsync(groupId);
            if(group is null)
                return false;
            group.AddMember(userId);
            await UpdateAsync(groupId, group);
            return true;
        });
    
    public async Task<bool> TryDeleteUserFromGroupAsync(int groupId, int userId) =>
        await Task.Run(async () =>
        {
            var group = await GetAsync(groupId);
            if(group is null)
                return false;
            group.RemoveMember(userId);
            await UpdateAsync(groupId, group);
            return true;
        });

    public async Task<int> CreateAsync(GroupRegistrationModel grm)
    {
        var id = new Random(new DateTime().Millisecond).Next(10000, 99999);
        await CreateAsync(new Group(id, grm));
        return id;
    }

    public async Task<bool> TryAddTest(int groupId, int testId) =>
        await Task.Run(async () =>
        {
            var group = await GetAsync(testId);
            if (group is null)
                return false;
            group.AddTest(testId);
            await UpdateAsync(groupId, group);
            return true;
        });
    
    public async Task<bool> TryDeleteTest(int groupId, int testId) =>
        await Task.Run(async () =>
        {
            var group = await GetAsync(testId);
            if (group is null)
                return false;
            group.DeleteTest(testId);
            await UpdateAsync(groupId, group);
            return true;
        });
}