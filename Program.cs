using Microsoft.EntityFrameworkCore;
using WPR_project.Data;
using WPR_project.Repositories;
using WPR_project.Services;
using WPR_project.Services.Email;

var builder = WebApplication.CreateBuilder(args);

// React CORS-configuratie
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy.WithOrigins("https://localhost:5173")
                        .AllowAnyHeader()
                        .AllowCredentials());
});

// Database setup
builder.Services.AddDbContext<GegevensContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection voor repositories
builder.Services.AddScoped<IHuurderRegistratieRepository, HuurderRegistratieRepository>();
builder.Services.AddScoped<IZakelijkeHuurderRepository, ZakelijkeHuurderRepository>();
builder.Services.AddScoped<IWagenparkBeheerderRepository, WagenparkBeheerderRepository>();

// Dependency Injection voor services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ParticulierHuurderService>();
builder.Services.AddScoped<ZakelijkeHuurderService>();
builder.Services.AddScoped<WagenparkBeheerderService>();
builder.Services.AddScoped<VoertuigService>();

// Controllers en Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configureer de HTTP-aanvraagpipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Zorgt ervoor dat bestanden in wwwroot worden geserveerd
app.UseRouting();
app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();

// Configureer routes
app.MapControllers();

// Fallback voor React-routering
app.MapFallbackToFile("index.html");

// Start de applicatie op een specifieke poort
app.Run("https://localhost:5033");