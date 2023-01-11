using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PPBackend.Models;
using PPBackend.Services;

namespace PPBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        // тестовые данные вместо использования базы данных


        [HttpGet("token")]
        public async Task<IActionResult> Token([FromQuery]string username, [FromQuery]string password, [FromServices]UsersService service)
        {
            
            var identity = GetIdentity(username, password, service);
            if (identity == null)
            {
                return BadRequest(new {errorText = "Invalid username or password."});
            }
            

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: TokenService.AuthOptions.ISSUER,
                audience: TokenService.AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(TokenService.AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(TokenService.AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }
        
        [HttpPost]
        [Route("join_group/{groupId}")]
        [Authorize]
        public async Task<ActionResult> AddToGroup(int groupId, [FromServices] GroupService groupService, [FromServices] UsersService usersService)
        {
            var id = int.Parse(User.FindAll("ID").FirstOrDefault()?.Value);
            var ok = await groupService.TryAddUserToGroupAsync(groupId, id) 
                     && await usersService.TryAddGroupToUserAsync(id, groupId);
            return ok ? Ok() : BadRequest();
        }
        
        [HttpPost]
        [Route("new")]
        public async Task<ActionResult> CreateUser([FromBody] UserRegistrationModel urm, [FromServices] UsersService service)
        {
            var user = new User(urm);
            await service.CreateAsync(user);
            return Ok();
        }
        
        [HttpDelete]
        [Route("leave_group/{groupId}")]
        [Authorize]
        public async Task<ActionResult> LeaveFromGroup(int groupId, [FromServices] GroupService groupService)
        {
            var id = int.Parse(User.FindAll("ID").FirstOrDefault()?.Value);
            var ok = await groupService.TryDeleteUserFromGroupAsync(groupId, id);
            return ok ? Ok() : BadRequest();
        }
        
        [HttpGet]
        [Route("get_groups")]
        [Authorize]
        public async Task<ActionResult> GetGroups([FromServices] UsersService usersService)
        {
            var id = int.Parse(User.FindAll("ID").FirstOrDefault()?.Value);
            var result = await usersService.GetUserGroups(id);
            return result.Count > 0 ? Json(result) : BadRequest();
        }
        
        [HttpGet]
        [Route("info")]
        [Authorize]
        public async Task<ActionResult> GetInfo([FromServices] UsersService usersService)
        {
            var id = int.Parse(User.FindAll("ID").FirstOrDefault()?.Value);
            var user = await usersService.GetAsync(id);
            if (user is null)
                return NotFound();
            return Json(user.GetInfo());
        }
        
        [Authorize]
        [HttpPost("complete_test/{testId}")]
        public async Task<ActionResult> CompleteTest(
            int testId,
            [FromServices] TestService testsService,
            [FromServices] UsersService userService,
            [FromBody] List<string> answers)
        {
            var userId  = int.Parse(User.FindAll("ID").FirstOrDefault()?.Value);
            var exist = await testsService.Exist(testId);

            if (!exist)
            {
                return NotFound();
            }

            var rating = await testsService.Evaluate(testId, answers);

            var user = await userService.GetAsync(userId);
            if (user is null)
            {
                return NotFound();
            }
            user.CompleteTest(testId, rating);
            await userService.UpdateAsync(userId, user);

            return NoContent();
        }
        
        [Authorize]
        [HttpPost("add_test/{testId}")]
        public async Task<IActionResult> AddTestToUser(
            [FromServices] TestService testService,
            [FromServices] UsersService userService,
            int testId)
        {
            var userId  = int.Parse(User.FindAll("ID").FirstOrDefault()?.Value);
            var testExist = await testService.Exist(testId);
            if (!testExist)
                return NotFound("Нет теста с таким id");
            var result = await userService.TryAddTestToUserAsync(userId, testId);

            return result ? Ok() : NotFound("Нет пользователя с таким id");
        }
        
        [Authorize]
        [HttpDelete]
        [Route("delete_test/{testId}")]
        public async Task<IActionResult> DeleteTestFromUser(
            [FromServices] TestService testService,
            [FromServices] UsersService userService,
            int testId)
        {
            var userId  = int.Parse(User.FindAll("ID").FirstOrDefault()?.Value);
            var testExist = await testService.Exist(testId);
            if (!testExist)
                return NotFound("Нет теста с таким id");
            var result = await userService.TryAddTestToUserAsync(userId, testId);

            return result ? Ok() : NotFound("Нет пользователя с таким id");
        }

        private ClaimsIdentity GetIdentity(string username, string password, UsersService services)
        {
            var user =  services.GetUserAsync(username, password).Result;
            
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                    new Claim("ID", user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()) //TODO: добавить роли
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}