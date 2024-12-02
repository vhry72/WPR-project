using Microsoft.EntityFrameworkCore;
using WPR_project.Data;


 var builder = WebApplication.CreateBuilder(args);

//react : 

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder => builder.WithOrigins("http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader());
});


    // Database setup
    builder.Services.AddDbContext<GegevensContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Add services to the container
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles(); // Zorgt ervoor dat wwwroot-bestanden geserveerd worden
    app.UseRouting();

    // Route API-aanroepen naar controllers
    app.MapControllers();

    // Alle andere verzoeken naar React's index.html sturen
    app.MapFallbackToFile("index.html");
// alsjeblieft vhry
    app.Run("https://localhost:5033");
 