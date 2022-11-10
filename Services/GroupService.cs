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

    public async Task<bool> TryAddUserToGroupAsync(int id, int userId) =>
        await Task.Run(async () =>
        {
            var group = await GetAsync(id);
            if(group is null)
                return false;
            group.AddMember(userId);
            return true;
        });
    
    public async Task<bool> TryDeleteUserFromGroupAsync(int groupId, int userId) =>
        await Task.Run(async () =>
        {
            var group = await GetAsync(groupId);
            if(group is null)
                return false;
            group.RemoveMember(userId);
            return true;
        });

    public async Task CreateAsync(GroupRegistrationModel grm)
    {
        await CreateAsync(new Group(globalId, grm));
    }
}