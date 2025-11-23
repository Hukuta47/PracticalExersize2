using API_Comfort.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;



ComfortDBContext comfortDBContext = new ComfortDBContext();
var builder = WebApplication.CreateBuilder(args);

string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other\\ComfortDatabase.db");

builder.Services.AddDbContext<ComfortDBContext>(options => options.UseSqlite("Data Source={dbPath}"));


var app = builder.Build();


// ----------------------
//    ¿¬“Œ–»«¿÷»ﬂ
// ----------------------

app.MapPost("/auth/login", async (ComfortDBContext db, LoginDto dto) =>
{
    var user = await db.Employees
        .FirstOrDefaultAsync(u => u.Login == dto.Login && u.Password == dto.Password);

    if (user == null)
        return Results.BadRequest("ÕÂ‚ÂÌ˚È ÎÓ„ËÌ ËÎË Ô‡ÓÎ¸");

    return Results.Ok(new
    {
        user.Id,
        user.FullName,
        user.RoleId
    });
});


// ----------------------
//    œ–Œƒ” ÷»ﬂ
// ----------------------

app.MapGet("/products", async (ComfortDBContext db) =>
{
    var list = await db.Products
        .Include(p => p.ProductType)
        .Include(p => p.ProductMaterial)
        .ToListAsync();

    return Results.Ok(list);
});

app.MapGet("/products/{id:int}", async (ComfortDBContext db, int id) =>
{
    var product = await db.Products
        .Include(p => p.ProductType)
        .Include(p => p.ProductMaterial)
        .FirstOrDefaultAsync(p => p.Id == id);

    return product is null ? Results.NotFound() : Results.Ok(product);
});

app.MapPost("/products", async (ComfortDBContext db, Product product) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Ok(product);
});

app.MapPut("/products/{id:int}", async (ComfortDBContext db, int id, Product dto) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();

    product.Name = dto.Name;
    product.Article = dto.Article;
    product.ProductTypeId = dto.ProductTypeId;
    product.MinimumCostPartner = dto.MinimumCostPartner;
    product.ProductMaterialId = dto.ProductMaterialId;

    await db.SaveChangesAsync();
    return Results.Ok(product);
});

app.MapDelete("/products/{id:int}", async (ComfortDBContext db, int id) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();

    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.Ok();
});


// ----------------------
//     ÷≈’¿ ƒÀﬂ œ–Œƒ” “¿
// ----------------------

app.MapGet("/products/{id:int}/workspaces", async (ComfortDBContext db, int id) =>
{
    var list = await db.ProductsWorkSpaces
        .Where(p => p.ProductId == id)
        .Include(p => p.Workspace)
        .ToListAsync();

    return Results.Ok(list);
});


// ----------------------
//   –¿—◊®“ ¬–≈Ã≈Õ» œ–Œ»«¬Œƒ—“¬¿
// ----------------------

app.MapGet("/products/{id:int}/manufacturing-time", async (ComfortDBContext db, int id) =>
{
    var workspaces = await db.ProductsWorkSpaces
        .Where(p => p.ProductId == id)
        .ToListAsync();

    double sum = workspaces.Sum(x => x.ProductionTimePerHour);
    return Results.Ok(new { TotalHours = sum });
});


// ----------------------
//     Ã¿“≈–»¿À€
// ----------------------

app.MapGet("/materials", async (ComfortDBContext db) =>
{
    return Results.Ok(await db.ProductMaterials.ToListAsync());
});

app.MapGet("/materials/{id:int}", async (ComfortDBContext db, int id) =>
{
    var item = await db.ProductMaterials.FindAsync(id);
    return item is null ? Results.NotFound() : Results.Ok(item);
});


// ----------------------
//   –¿—◊®“ —€–‹ﬂ
// ----------------------

app.MapGet("/calc-material", async (ComfortDBContext db, int productTypeId, int materialTypeId, int count, double p1, double p2) =>
{
    var productType = await db.ProductTypes.FindAsync(productTypeId);
    var material = await db.ProductMaterials.FindAsync(materialTypeId);

    if (productType is null || material is null || p1 <= 0 || p2 <= 0 || count <= 0)
        return Results.Ok(new { required = -1 });

    double baseAmount = p1 * p2 * productType.ProductTypeCoefficient;
    double total = baseAmount * count;
    double loss = material.PercentageMaterialLosses;

    double result = total + (total * loss);

    return Results.Ok(new { required = Math.Ceiling(result) });
});


// ----------------------
//    œ¿–“Õ®–€
// ----------------------

app.MapGet("/partners", async (ComfortDBContext db) =>
{
    return Results.Ok(await db.Partners.Include(p => p.Type).ToListAsync());
});

app.MapGet("/partners/{id:int}", async (ComfortDBContext db, int id) =>
{
    var p = await db.Partners
        .Include(p => p.Type)
        .FirstOrDefaultAsync(p => p.Id == id);

    return p is null ? Results.NotFound() : Results.Ok(p);
});

app.MapPost("/partners", async (ComfortDBContext db, Partner partner) =>
{
    db.Partners.Add(partner);
    await db.SaveChangesAsync();
    return Results.Ok(partner);
});

app.MapPut("/partners/{id:int}", async (ComfortDBContext db, int id, Partner dto) =>
{
    var p = await db.Partners.FindAsync(id);
    if (p is null) return Results.NotFound();

    p.Name = dto.Name;
    p.TypeId = dto.TypeId;
    p.Email = dto.Email;
    p.Inn = dto.Inn;
    p.NumberPhone = dto.NumberPhone;
    p.DirectorFullName = dto.DirectorFullName;
    p.Rating = dto.Rating;

    await db.SaveChangesAsync();
    return Results.Ok(p);
});


// ----------------------
//     «¿œ–Œ—€ («¿ﬂ¬ »)
// ----------------------

app.MapGet("/requests", async (ComfortDBContext db) =>
{
    return Results.Ok(await db.Requests.ToListAsync());
});

app.MapPost("/requests", async (ComfortDBContext db, Request req) =>
{
    db.Requests.Add(req);
    await db.SaveChangesAsync();
    return Results.Ok(req);
});


app.Run();


// ----------------------
//   DTO  À¿——€
// ----------------------

public record LoginDto(string Login, string Password);
