using NETDAY1;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();
app.UseMyCustomMiddleware();

app.MapGet("/", () => "Hello World!");

app.Run();
