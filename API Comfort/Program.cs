using API_Comfort.Models;

ComfortDBContext comfortDBContext = new ComfortDBContext();


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();






app.MapGet("/GetProducts", () => comfortDBContext.Products);



app.Run();
