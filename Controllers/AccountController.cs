using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PPBackend.Models;
using PPBackend.Services;

namespace PPBackend.Controllers
{
    public class AccountController : Controller
    {
        // тестовые данные вместо использования базы данных


        [HttpPost("/token")]
        public IActionResult Token(string username, string password, UsersService service)
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

        private ClaimsIdentity GetIdentity(string username, string password, UsersService services)
        {
            var user =  services.GetUserAsync(username, password).Result;
            
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("Name", user.Name),
                    new Claim("ID", user.Id.ToString()),
                    // new Claim(ClaimsIdentity.DefaultRoleClaimType, user.) //TODO: добавить роли
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