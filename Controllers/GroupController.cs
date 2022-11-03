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
    
    [HttpGet("{id:length(24)}")]
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
        var newGroup = new Group(grm);
        await _groupService.CreateAsync(newGroup);

        return CreatedAtAction(nameof(Get), new { id = newGroup.Id }, newGroup);
    }

    [HttpPut("{id:length(24)}")]
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

    [HttpDelete("{id:length(24)}")]
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
    [Route("add/{groupId}/{userId}")]
    public async Task<IActionResult> AddUserToGroup(
        UsersService usersService, 
        int groupId, 
        int userId)
    {
        var status = await _groupService.TryAddUserToGroupAsync(groupId, userId);
        return status ? Ok() : NotFound("Неверный id");
    }
}