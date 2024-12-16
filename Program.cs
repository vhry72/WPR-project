using Microsoft.EntityFrameworkCore;
using System.Text.Json;
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



// Dependency Injection voor services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ParticulierHuurderService>();
builder.Services.AddScoped<ZakelijkeHuurderService>();
builder.Services.AddScoped<WagenparkBeheerderService>();
builder.Services.AddScoped<AbonnementService>();
builder.Services.AddScoped<VoertuigService>();
builder.Services.AddScoped<BedrijfsMedewerkersService>();
builder.Services.AddScoped<HuurverzoekService>();

//Voor de 24-uurs reminder service
builder.Services.AddHostedService<HuurverzoekReminderService>();



// Controllers en Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
 void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Je kunt hier globale JSON-opties instellen als dat nodig is
            });
}

 void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}


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

app.UseHttpsRedirection();

// Start de applicatie op een specifieke poort
app.Run("https://localhost:5033");