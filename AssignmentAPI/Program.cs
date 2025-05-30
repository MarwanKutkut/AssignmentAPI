using AssignmentAPI.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services
    .AddSwaggerExplorer()
    .InjectDbContext(builder.Configuration)
    .AddAppConfig(builder.Configuration)
    .ConfigureIdentityOptions()
    .AddIdentityAuth(builder.Configuration)
    .InjectValidators()
    ;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrationAsync();
}

app.ConfigureSwaggerExplorer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
