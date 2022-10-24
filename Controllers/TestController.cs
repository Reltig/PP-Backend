using Microsoft.AspNetCore.Mvc;

namespace PPBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    public TestController(){}

    [HttpGet(Name = "GetTest")]
    public void GetTest()
    {
        
    }
}