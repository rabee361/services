using Microsoft.EntityFrameworkCore;
using Users.Api.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0))));



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Automatically apply migrations and seed data with retry logic
for (int i = 0; i < 10; i++)
{
    try
    {
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
        break; // Success!
    }
    catch (Exception ex)
    {
        if (i == 9) throw; // Re-throw if all retries failed
        Console.WriteLine($"Waiting for MySQL... attempt {i + 1}");
        Thread.Sleep(5000); // Wait 5 seconds before retry
    }
}

app.UseAuthorization();
app.MapControllers();

app.Run();
