using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PPBackend.Models;
using PPBackend.Services;

namespace PPBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly TestService _testsService;

    public TestController(TestService testsService) =>
        _testsService = testsService;

    // [HttpGet]
    // public async Task<List<Test>> Get() =>
    //     await _testsService.GetAsync();
    //
    [HttpGet("{id}")]
    public async Task<ActionResult<Test>> Get(int id)
    {
        var test = await _testsService.GetAsync(id);
    
        if (test is null)
        {
            return NotFound();
        }
    
        return test;
    }
    //
    // [HttpPost]
    // public async Task<IActionResult> Post(Test newBook)
    // {
    //     await _testsService.CreateAsync(newBook);
    //
    //     return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    // }
    //
    // [HttpPut("{id}")]
    // public async Task<IActionResult> Update(int id, Test updatedTest)
    // {
    //     var test = await _testsService.GetAsync(id);
    //
    //     if (test is null)
    //     {
    //         return NotFound();
    //     }
    //
    //     updatedTest.Id = test.Id;
    //
    //     await _testsService.UpdateAsync(id, updatedTest);
    //
    //     return NoContent();
    // }
    //
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> Delete(int id)
    // {
    //     var test = await _testsService.GetAsync(id);
    //
    //     if (test is null)
    //     {
    //         return NotFound();
    //     }
    //
    //     await _testsService.RemoveAsync(id);
    //
    //     return NoContent();
    // }
    
    [HttpPost()]
    public async Task<IActionResult> CreateTest(TestRegistrationModel trm)
    {
        await _testsService.CreateAsync(new Test(trm));
    
        return CreatedAtAction(nameof(CreateTest), trm);
    }
    
    [HttpGet("{id}/questions")]
    public async Task<ActionResult<Test>> GetQuestions(int id)
    {
        var test = await _testsService.GetAsync(id);

        if (test is null)
        {
            return NotFound();
        }

        return Ok(test.GetQuestions()); //TODO: переделать в _testService.Getquestions()
    }
}