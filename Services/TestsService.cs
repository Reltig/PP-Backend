using PPBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PPBackend.Settings;

namespace PPBackend.Services;

public class TestService : CRUDService<Test, TestsStorageSettings>
{
    public TestService(IOptions<TestsStorageSettings> storeSettings) : base(storeSettings) {}

    public async Task<float> Evaluate(int id, List<string> answers)=>
        await Task.Run(async () =>
        {
            var test = await GetAsync(id);
            if(test is null)
                return 0;
            return await test.Evaluate(answers);
        });
    
}