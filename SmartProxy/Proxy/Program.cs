using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json");

// Configure logging.
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddOcelot()
     .AddCacheManager(x =>
     {
          x.WithDictionaryHandle();
     });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
     app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
     endpoints.MapGet("/", async context =>
     {
          await context.Response.WriteAsync("Smart Proxy");
     });
});

app.MapRazorPages();
app.UseOcelot().Wait();

app.Run();
