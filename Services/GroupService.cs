using PPBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PPBackend.Settings;

namespace PPBackend.Services;

public class GroupService : CRUDService<Group, GroupsStorageSettings>
{
    public GroupService(IOptions<GroupsStorageSettings> storeSettings) : base(storeSettings) {}

    public async Task<bool> TryAddUserToGroupAsync(int id, int userId) =>
        await Task.Run(async () =>
        {
            var group = await GetAsync(id);
            if(group is null)
                return false;
            group.AddMember(userId);
            return true;
        });
}