using PPBackend.Settings;
using PPBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<TestsStorageSettings>(
    builder.Configuration.GetSection("TestsStorageSettings"));
builder.Services.Configure<UserStorageSettings>(
    builder.Configuration.GetSection("UsersStorageSettings"));
builder.Services.Configure<TestsStorageSettings>(
    builder.Configuration.GetSection("GroupsStorageSettings"));

builder.Services.AddSingleton<TestService>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<GroupsStorageSettings>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
