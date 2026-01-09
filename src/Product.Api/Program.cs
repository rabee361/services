using Microsoft.EntityFrameworkCore;
using Product.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    db.Database.EnsureCreated();

    // Assign random images if not present
    var wwwroot = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "images");
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

app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

app.Run();
