using PPBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PPBackend.Settings;

namespace PPBackend.Services;

public class GroupService : CRUDService<Group, GroupsStorageSettings>
{
    private int globalId = 0;

    public GroupService(IOptions<GroupsStorageSettings> storeSettings) : base(storeSettings)
    {
        globalId = new Random(new DateTime().Millisecond).Next(10000, 99999);
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

    public async Task CreateAsync(GroupRegistrationModel grm)
    {
        await CreateAsync(new Group(globalId, grm));
    }

    public async Task<bool> TryAddTest(int testId) =>
        await Task.Run(async () =>
        {
            var test = await GetAsync(testId);
            if (test is null)
                return false;
            test.AddTest(testId);
            await UpdateAsync(testId, test);
            return true;
        });
    
    public async Task<bool> TryDeleteTest(int testId) =>
        await Task.Run(async () =>
        {
            var test = await GetAsync(testId);
            if (test is null)
                return false;
            test.DeleteTest(testId);
            await UpdateAsync(testId, test);
            return true;
        });
}