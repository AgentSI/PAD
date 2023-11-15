using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SyncNode.Services;
using SyncNode.Settings;

var builder = WebApplication.CreateBuilder(args);

// Adăugați configurarea
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<MovieAPISettings>(configuration.GetSection("MovieAPISettings"));
builder.Services.AddSingleton<IMovieAPISettings>(provider =>
     provider.GetRequiredService<IOptions<MovieAPISettings>>().Value);

builder.Services.AddSingleton<SyncWorkJobService>();
builder.Services.AddHostedService(provider => provider.GetService<SyncWorkJobService>());

builder.Services.AddControllers();

// Adăugați serviciul Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sync", Version = "v1" });
});

var app = builder.Build();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
     app.UseExceptionHandler("/Error");
}

app.UseSwagger();
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();
