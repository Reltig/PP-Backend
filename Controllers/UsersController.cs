using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PPBackend.Models;
using PPBackend.Services;

namespace PPBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService _userDbService;

    public UsersController(UsersService userDbService) =>
        _userDbService = userDbService;

    [HttpGet]
    public async Task<List<User>> Get() =>
        await _userDbService.GetAsync();
    
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        var user = await _userDbService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post(UserRegistrationModel urm)
    {
        var newUser = new User(urm);
        await _userDbService.CreateAsync(newUser);
        
        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User updatedUser)
    {
        var user = await _userDbService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        updatedUser.Id = user.Id;

        await _userDbService.UpdateAsync(id, updatedUser);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userDbService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _userDbService.RemoveAsync(id);

        return NoContent();
    }
    
    [HttpPost]
    [Route("/add-test/{testId}/{userId}")]
    public async Task<IActionResult> AddTestToUser(
        [FromServices] TestService testService, 
        int userId, 
        int testId)
    {
        var test = await testService.GetAsync(testId);
        if (test is null)
            return NotFound("Нет теста с таким id");
        var result = await _userDbService.TryAddTestToUserAsync(userId, testId);

        return result ? Ok() : NotFound("Нет пользователя с таким id");
    }
    
    [HttpPost("test/evaluate/{testId}")]
    public async Task<ActionResult> CompleteTest(
        [FromServices] TestService testsService,
        [FromQuery] int userId,
        [FromBody] List<string> answers,
        int testId)
    {
        var test = await testsService.GetAsync(testId);

        if (test is null)
        {
            return NotFound();
        }

        var rating = await test.Evaluate(answers);

        var user = await _userDbService.GetAsync(userId);
        if (user is null)
        {
            return NotFound();
        }
        
        user.CompleteTest(testId, rating);
        await _userDbService.UpdateAsync(userId, user);

        return NoContent();
    }

    [HttpGet]
    [Route("auth")]
    public async Task<ActionResult> Authentication([FromQuery] string name, [FromQuery] string password)
    {
        return Ok(_userDbService.GetIdAsync(name, password).Result);
    }
    
    [Authorize]
    [HttpPost]
    [Route("add/{groupId}")]
    public async Task<ActionResult> AddToGroup([FromQuery] int groupId, [FromServices] GroupService groupService)
    {
        var id = int.Parse(User.FindAll("ID").FirstOrDefault()?.Value);
        var ok = await groupService.TryAddUserToGroupAsync(groupId, id);
        return ok ? Ok() : BadRequest();
    }
}