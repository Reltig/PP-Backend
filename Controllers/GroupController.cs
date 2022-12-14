using Microsoft.AspNetCore.Mvc;
using PPBackend.Models;
using PPBackend.Services;

namespace PPBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    private readonly GroupService _groupService;

    public GroupController(GroupService groupService) =>
        _groupService = groupService;

    [HttpGet]
    public async Task<List<Group>> Get() =>
        await _groupService.GetAsync();
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> Get(int id)
    {
        var group = await _groupService.GetAsync(id);

        if (group is null)
        {
            return NotFound();
        }

        return group;
    }

    [HttpPost]
    public async Task<IActionResult> Post(GroupRegistrationModel grm)
    {
        await _groupService.CreateAsync(grm);

        return CreatedAtAction(nameof(Get), grm);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Group newGroup)
    {
        var group = await _groupService.GetAsync(id);

        if (group is null)
        {
            return NotFound();
        }

        newGroup.Id = group.Id;

        await _groupService.UpdateAsync(id, newGroup);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var group = await _groupService.GetAsync(id);

        if (group is null)
        {
            return NotFound();
        }

        await _groupService.RemoveAsync(id);

        return NoContent();
    }
    
    [HttpPost]
    [Route("{groupId}/{userId}")]
    public async Task<IActionResult> AddUserToGroup(
        UsersService usersService, 
        int groupId, 
        int userId)
    {
        var status = await _groupService.TryAddUserToGroupAsync(groupId, userId);
        return status ? Ok() : NotFound("Неверный id");
    }
    
    [HttpDelete]
    [Route("{groupId}/{userId}")]
    public async Task<IActionResult> DeleteUserFromGroup(
        UsersService usersService, 
        int groupId, 
        int userId)
    {
        var user = await usersService.GetAsync(userId);
        if (user is null)
            return NotFound("Неверный id");
        var status = await _groupService.TryDeleteUserFromGroupAsync(groupId, userId);
        return status ? Ok() : NotFound("Неверный id");
    }
}