using BSC_0909.Controller;
using BSC_0909.Domain.Services.Bot;
DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.WebHost.UseUrls("http://0.0.0.0:6969");
// ------- DI --------
builder.Services.AddInjection(builder.Configuration);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.Run();


