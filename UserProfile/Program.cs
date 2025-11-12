using UserProfile.Infrastructure.Persistance;
using UserProfile.Infrastructure.Repositories;
using UserProfile.Application.Services;
using MediatR;
using FluentValidation;
using Identity.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UserProfile.Application.Commands;
using UserProfile.Application.Validators;
using UserProfile.Application.DTOs;
using UserProfile.Domain.Entities;




var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<UserProfileDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Add services to the container.

builder.Services.AddScoped<ProfileService>();

builder.Services.AddMediatR(typeof(CompleteProfileCommand).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CompleteProfileCommandValidator).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserProfile.Api", Version = "v1" });
});

var app = builder.Build();

app.UseMiddleware<UserProfile.Api.Middlewares.ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<UserProfileDbContext>();
        var retries = 10;
        for (int i = 0; i < retries; i++)
        {
            try
            {
                db.Database.Migrate();
                logger.LogInformation("Database migration applied.");
                break;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Database not ready yet - retrying in 5s ({attempt}/{retries})", i + 1, retries);
                Thread.Sleep(5000);
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}



app.Run();
