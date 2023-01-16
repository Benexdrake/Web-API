var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CrunchyrollController>()
                .AddScoped<IMDbController>()
                .AddScoped<Pokemon>();

builder.Services.AddDbContext<CrunchyrollDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Crunchyroll")));
builder.Services.AddDbContext<IMDbDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IMDB")));
builder.Services.AddDbContext<PokemonDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Pokemon")));

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
