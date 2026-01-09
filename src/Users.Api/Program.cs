using Microsoft.EntityFrameworkCore;
using Users.Api.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Automatically apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    db.Database.EnsureCreated();

    var seedFile = Path.Combine(app.Environment.ContentRootPath, "seed.sql");
    if (File.Exists(seedFile))
    {
        var sql = File.ReadAllText(seedFile);
        db.Database.ExecuteSqlRaw(sql);
    }
}

app.UseAuthorization();
app.MapControllers();

app.Run();
