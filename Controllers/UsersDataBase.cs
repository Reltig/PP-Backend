using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PPBackend.Models;
using PPBackend.Services;

namespace PPBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersDataBaseService _userDbService;

    public UsersController(UsersDataBaseService userDbService) =>
        _userDbService = userDbService;

    [HttpGet]
    public async Task<List<User>> Get() =>
        await _userDbService.GetAsync();
    
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var user = await _userDbService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
        await _userDbService.CreateAsync(newUser);
        
        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, User updatedUser)
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
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userDbService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _userDbService.RemoveAsync(id);

        return NoContent();
    }
}