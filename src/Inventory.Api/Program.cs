using Microsoft.EntityFrameworkCore;
using Inventory.Api.Data;
using Inventory.Api.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0))));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

for (int i = 0; i < 10; i++)
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
            db.Database.EnsureCreated();

            var seedFile = Path.Combine(app.Environment.ContentRootPath, "seed.sql");
            if (File.Exists(seedFile))
            {
                var sql = File.ReadAllText(seedFile);
                db.Database.ExecuteSqlRaw(sql);
            }
        }
        break;
    }
    catch (Exception)
    {
        if (i == 9) throw;
        Console.WriteLine($"Waiting for MySQL... attempt {i + 1}");
        Thread.Sleep(5000);
    }
}

app.UseAuthorization();
app.MapControllers();

app.Run();
