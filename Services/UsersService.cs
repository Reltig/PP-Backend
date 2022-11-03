using PPBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PPBackend.Settings;

namespace PPBackend.Services;

public class UsersService: CRUDService<User, UserStorageSettings>
{
    public UsersService(IOptions<UserStorageSettings> storeSettings) : base(storeSettings) {}
}