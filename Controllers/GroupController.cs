using Microsoft.AspNetCore.Authorization;
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

    //[HttpGet]
    // public async Task<List<Group>> Get() =>
    //     await _groupService.GetAsync();
    //
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
    //
    // [HttpPost]
    // public async Task<IActionResult> Post(GroupRegistrationModel grm)
    // {
    //     await _groupService.CreateAsync(grm);
    //
    //     return CreatedAtAction(nameof(Get), grm);
    // }
    //
    // [HttpPut("{id}")]
    // public async Task<IActionResult> Update(int id, Group newGroup)
    // {
    //     var group = await _groupService.GetAsync(id);
    //
    //     if (group is null)
    //     {
    //         return NotFound();
    //     }
    //
    //     newGroup.Id = group.Id;
    //
    //     await _groupService.UpdateAsync(id, newGroup);
    //
    //     return NoContent();
    // }
    //
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> Delete(int id)
    // {
    //     var group = await _groupService.GetAsync(id);
    //
    //     if (group is null)
    //     {
    //         return NotFound();
    //     }
    //
    //     await _groupService.RemoveAsync(id);
    //
    //     return NoContent();
    // }
    //
    // [HttpPost]
    // [Route("{groupId}/{userId}")]
    // public async Task<IActionResult> AddUserToGroup(
    //     UsersService usersService, 
    //     int groupId, 
    //     int userId)
    // {
    //     var status = await _groupService.TryAddUserToGroupAsync(groupId, userId);
    //     return status ? Ok() : NotFound("Неверный id");
    // }
    //
    // [HttpDelete]
    // [Route("{groupId}/{userId}")]
    // public async Task<IActionResult> DeleteUserFromGroup(
    //     UsersService usersService, 
    //     int groupId, 
    //     int userId)
    // {
    //     var user = await usersService.GetAsync(userId);
    //     if (user is null)
    //         return NotFound("Неверный id");
    //     var status = await _groupService.TryDeleteUserFromGroupAsync(groupId, userId);
    //     return status ? Ok() : NotFound("Неверный id");
    // }
    
    [Authorize(Roles = "Teacher")]
    [HttpPost]
    public async Task<IActionResult> CreateGroup(GroupRegistrationModel grm, [FromServices] UsersService usersService)
    {
        var groupId = await _groupService.CreateAsync(grm);
        var userId = int.Parse(User.FindAll("ID").FirstOrDefault()?.Value);
        await usersService.AddManagedGroup(userId, groupId);
        return CreatedAtAction(nameof(CreateGroup), groupId);
    }
    
    [HttpPost("add_test/{groupId}/{testId}")]
    public async Task<IActionResult> AddTest(int groupId,int testId)
    {
        var result = await _groupService.TryAddTest(groupId,testId);
        
        return result ? Ok() : BadRequest();
    }
    
    [HttpDelete("delete_test/{groupId}/{testId}")]
    public async Task<IActionResult> DeleteTest(int groupId,int testId)
    {
        var result = await _groupService.TryDeleteTest(groupId, testId);
        
        return result ? Ok() : BadRequest();
    }
}