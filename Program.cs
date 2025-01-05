using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using WPR_project.Data;
using WPR_project.Repositories;
using WPR_project.Services;
using WPR_project.Services.Email;

var builder = WebApplication.CreateBuilder(args);

// React CORS-configuratie
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
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
builder.Services.AddScoped<IVoertuigRepository, VoertuigRepository>();
builder.Services.AddScoped<IAbonnementRepository, AbonnementRepository>();
builder.Services.AddScoped<IBedrijfsMedewerkersRepository, BedrijfsMedewerkersRepository>();
builder.Services.AddScoped<IHuurVerzoekRepository, HuurVerzoekRepository>();
builder.Services.AddScoped<ISchademeldingRepository, SchademeldingRepository>();
builder.Services.AddScoped<IVoertuigStatusRepository, VoertuigStatusRepository>();

// Dependency Injection voor services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ParticulierHuurderService>();
builder.Services.AddScoped<ZakelijkeHuurderService>();
builder.Services.AddScoped<WagenparkBeheerderService>();
builder.Services.AddScoped<AbonnementService>();
builder.Services.AddScoped<VoertuigService>();
builder.Services.AddScoped<BedrijfsMedewerkersService>();
builder.Services.AddScoped<HuurverzoekService>();
builder.Services.AddScoped<SchademeldingService>();
builder.Services.AddScoped<VoertuigStatusService>();

// Voor de 24-uurs reminder service
builder.Services.AddHostedService<HuurverzoekReminderService>();

// Controllers en Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // Prevent circular references
        options.JsonSerializerOptions.MaxDepth = 64; // Adjust depth as needed
    });

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
