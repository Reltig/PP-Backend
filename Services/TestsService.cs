using PPBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PPBackend.Settings;

namespace PPBackend.Services;

public class TestService : CRUDService<Test, TestsStorageSettings>
{
    public TestService(IOptions<TestsStorageSettings> storeSettings) : base(storeSettings) {}
}