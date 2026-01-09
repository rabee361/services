using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Product.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProductDbContext>(options =>
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
            var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
            db.Database.EnsureCreated();

            var seedFile = Path.Combine(app.Environment.ContentRootPath, "seed.sql");
            if (File.Exists(seedFile))
            {
                var sql = File.ReadAllText(seedFile);
                db.Database.ExecuteSqlRaw(sql);
            }

            // Assign random images if not present
            var wwwroot = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "images");
            if (Directory.Exists(wwwroot))
            {
                var images = Directory.GetFiles(wwwroot, "*.png")
                    .Select(f => "/images/" + Path.GetFileName(f))
                    .ToList();

                if (images.Any())
                {
                    var products = db.Products.Where(p => string.IsNullOrEmpty(p.ImageUrl)).ToList();
                    if (products.Any())
                    {
                        var random = new Random();
                        foreach (var product in products)
                        {
                            product.ImageUrl = images[random.Next(images.Count)];
                        }
                        db.SaveChanges();
                    }
                }
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

app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

app.Run();
