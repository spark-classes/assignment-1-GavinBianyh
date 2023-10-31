using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using WEB.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

public class ApiController : Controller
{
    private readonly IConfiguration _configuration;

    public ApiController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("/api")]
    public IActionResult GetSecret()
    {
        var secretValue = _configuration["keybyh"];
        return Content($"Hello, world! This is : {secretValue}");
    }
}
