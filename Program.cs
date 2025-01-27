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
using NuGet.Protocol.Resources;
using Hangfire.Dashboard;
using System;

var builder = WebApplication.CreateBuilder(args);

// cors policy ingevoerd
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

var hangfireUsername = builder.Configuration["Hangfire:Username"];
var hangfirePassword = builder.Configuration["Hangfire:Password"];


// Database setup
builder.Services.AddDbContext<GegevensContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Voeg Identity DbContext en configuratie toe
builder.Services.AddDbContext<GegevensContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<GegevensContext>()
    .AddDefaultTokenProviders();


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
builder.Services.AddScoped<IFactuurRepository, FactuurRepository>();
builder.Services.AddScoped<IPrivacyVerklaringRepository, PrivacyVerklaringRepository>();
builder.Services.AddScoped<IBackOfficeMedewerkerRepository, BackOfficeMedewerkerRepository>();



// Dependency Injection voor services
builder.Services.AddScoped<IFrontOfficeMedewerkerRepository, FrontOfficeMedewerkerRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<EmailService>();
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
builder.Services.AddScoped<FactuurService>();
builder.Services.AddScoped<PrivacyVerklaringService>();
builder.Services.AddScoped<BackOfficeService>();
builder.Services.AddScoped<FrontOfficeService>();



// Controllers en Swagger
builder.Services.AddControllers();

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
                Console.WriteLine($"JWT ontvangen: {jwtCookie}");
                context.Token = jwtCookie;
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            // Log de claims
            var claims = context.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
            Console.WriteLine($"Claims gevalideerd: {string.Join(", ", claims)}");
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authenticatie mislukt: {context.Exception.Message}");
            return Task.CompletedTask;
        }
    };


    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Zorg ervoor dat de juiste claimnamen worden gebruikt
        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", // userId
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",         // role
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();




if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Controleer of de inloggegevens correct zijn
    if (string.IsNullOrEmpty(hangfireUsername) || string.IsNullOrEmpty(hangfirePassword))
    {
        throw new InvalidOperationException("Hangfire username or password is not configured.");
    }

    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization =
        [
            new HangfireBasicAuthenticationFilter
            {
                Username = hangfireUsername,
                Password = hangfirePassword
            }
        ]
    });
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


