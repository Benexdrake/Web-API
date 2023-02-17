using MongoDB.Driver;
using Webscraper_API.Scraper.TCG_Magic.Controller;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder
    .Services
    .AddSingleton<MongoClient>(new MongoClient(builder.Configuration["MongoDb:IP"]))
    .AddSingleton<Browser>()

    .AddScoped<CrunchyrollController>()
    .AddScoped<IMDbController>()
    .AddScoped<Dota2Controller>()
    .AddScoped<InsightDigitalController>()
    .AddScoped<SteamController>()
    .AddScoped<PokemonController>()

    .AddScoped<CrunchyrollDBContext>()
    .AddScoped<IMDbDBContext>()
    .AddScoped<PokemonDBContext>()
    .AddScoped<Dota2DbContext>()
    .AddScoped<MagicDbContext>()
    .AddScoped<InsightDigitalDbContext>()
    .AddScoped<SteamDbContext>();


var app = builder.Build();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("*"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
