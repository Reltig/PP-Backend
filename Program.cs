using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using PPBackend.Settings;
using PPBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<TestsStorageSettings>(
    builder.Configuration.GetSection("TestsStorageSettings"));
builder.Services.Configure<UserStorageSettings>(
    builder.Configuration.GetSection("UsersStorageSettings"));
builder.Services.Configure<GroupsStorageSettings>(
    builder.Configuration.GetSection("GroupsStorageSettings"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // укзывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = TokenService.AuthOptions.ISSUER,
            
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = TokenService.AuthOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            
            // установка ключа безопасности
            IssuerSigningKey = TokenService.AuthOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });
//builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<TestService>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<GroupService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
