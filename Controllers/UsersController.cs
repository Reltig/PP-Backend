using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PPBackend.Models;
using PPBackend.Services;

namespace PPBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly UsersService _userDbService;

    public UsersController(UsersService userDbService) =>
        _userDbService = userDbService;

    [HttpGet]
    [Route("info/{id}")]
    public async Task<ActionResult> GetInfo([FromRoute] int id, [FromServices] UsersService usersService)
    {
        var user = await usersService.GetAsync(id);
        if (user is null)
            return NotFound();
        return Json(user.GetInfo());
    }
    // [HttpGet]
    // public async Task<List<User>> Get() =>
    //     await _userDbService.GetAsync();
    //
    // [HttpGet("{id}")]
    // public async Task<ActionResult<User>> Get(int id)
    // {
    //     var user = await _userDbService.GetAsync(id);
    //
    //     if (user is null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return user;
    // }
    //
    // [HttpPost]
    // public async Task<IActionResult> Post(UserRegistrationModel urm)
    // {
    //     var newUser = new User(urm);
    //     await _userDbService.CreateAsync(newUser);
    //     
    //     return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    // }
    //
    // [HttpPut("{id}")]
    // public async Task<IActionResult> Update(int id, User updatedUser)
    // {
    //     var user = await _userDbService.GetAsync(id);
    //
    //     if (user is null)
    //     {
    //         return NotFound();
    //     }
    //
    //     updatedUser.Id = user.Id;
    //
    //     await _userDbService.UpdateAsync(id, updatedUser);
    //
    //     return NoContent();
    // }
    //
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> Delete(int id)
    // {
    //     var user = await _userDbService.GetAsync(id);
    //
    //     if (user is null)
    //     {
    //         return NotFound();
    //     }
    //
    //     await _userDbService.RemoveAsync(id);
    //
    //     return NoContent();
    // }
}