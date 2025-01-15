using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WPR_project.Data;
using WPR_project.Repositories;
using WPR_project.Services;
using WPR_project.Services.Email;
using System.Text.Json.Serialization;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",

        policy => policy.WithOrigins("https://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });


// Database setup
builder.Services.AddDbContext<GegevensContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Voeg Identity DbContext en configuratie toe
builder.Services.AddDbContext<GegevensContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<GegevensContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Haal de JWT-token uit de cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var jwtCookie = context.HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(jwtCookie))
            {
                context.Token = jwtCookie; // Stel de JWT-token in vanuit de cookie
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            // Optionele logging voor debugging van mislukte authenticatie
            Console.WriteLine("JWT authenticatie mislukt: " + context.Exception.Message);
            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Validatie van de JWT-inhoud
        ValidateIssuer = true, // Controleer de 'issuer' claim
        ValidateAudience = true, // Controleer de 'audience' claim
        ValidateLifetime = true, // Controleer of de token verlopen is
        ValidateIssuerSigningKey = true, // Controleer de handtekening van de token

        // Instellingen voor validatie
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Uit je appsettings.json
        ValidAudience = builder.Configuration["Jwt:Audience"], // Uit je appsettings.json
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

        // Tolerantie voor tijdssynchronisatie (optioneel)
        ClockSkew = TimeSpan.Zero // Verwijdert standaard tolerantie van 5 minuten
    };
});


builder.Services.AddHangfire(configuration => configuration
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();


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
builder.Services.AddScoped<IVoertuigNotitiesRepository, VoertuigNotitiesRepository>();



// Dependency Injection voor services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ParticulierHuurderService>();
builder.Services.AddScoped<ZakelijkeHuurderService>();
builder.Services.AddScoped<WagenparkBeheerderService>();
builder.Services.AddScoped<AbonnementService>();
builder.Services.AddScoped<VoertuigService>();
builder.Services.AddScoped<BedrijfsMedewerkersService>();
builder.Services.AddScoped<HuurverzoekService>();
builder.Services.AddScoped<VoertuigNotitiesService>();

builder.Services.AddScoped<SchademeldingService>();
builder.Services.AddScoped<VoertuigStatusService>();

builder.Services.AddScoped<UserManagerService>();


// Voor de 24-uurs reminder service
builder.Services.AddHostedService<HuurverzoekReminderService>();




// Controllers en Swagger
builder.Services.AddControllers();
    

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHangfireDashboard();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Fallback voor React-routering
app.MapFallbackToFile("index.html");


using (var scope = app.Services.CreateScope())
{
    await RoleSeeder.SeedRoles(scope.ServiceProvider);
}

// Start de applicatie op een specifieke poort
app.Run("https://localhost:5033");


