using Common.Models;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MovieAPI.Repositories;
using MovieAPI.Services;
using MovieAPI.Settings;

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

builder.Services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
builder.Services.Configure<SyncServiceSettings>(configuration.GetSection("SyncServiceSettings"));

builder.Services.AddSingleton<IMongoDbSettings>(provider =>
     provider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
     
builder.Services.AddSingleton<ISyncServiceSettings>(provider =>
     provider.GetRequiredService<IOptions<SyncServiceSettings>>().Value);

builder.Services.AddScoped<IMongoRepository<Movie>, MongoRepository<Movie>>();
builder.Services.AddScoped<ISyncService<Movie>, SyncService<Movie>>();

builder.Services.AddControllers();

// Adăugați serviciul Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Proxy", Version = "v1" });
});

var app = builder.Build();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();
