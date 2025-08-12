using BSC_0909.Controller;
using BSC_0909.Domain.Services.Bot;
using BSC_0909.Infrastructure.Services;
DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.WebHost.UseUrls("http://0.0.0.0:6969");
// ------- DI --------
await builder.Services.AddInjection(builder.Configuration);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.Run();


