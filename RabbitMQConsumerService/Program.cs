using Candidate.Application;
using Candidate.Application.UseCases.Portal.General;
using Candidate.Infrastructure;
using Candidate.Infrastructure.DataAccess;
using Candidate.Shared;
using Candidate.Shared.Settings;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RabbitMQConsumerService.Consumer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://cc-web-test.7hrm.az")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
//connectionString>>
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
//connectionString<<
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();


var settings = builder.Configuration.GetSection(AuthSettings.SectionName)
            .Get<AuthSettings>()!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateVacancyCommand).Assembly));
builder.Services.AddScoped(typeof(IRequestHandler<CreateVacancyCommand>), typeof(CreateVacancyCommandHandler));




builder.Services.AddSingleton<CreateVacancyConsumer>();
builder.Services.AddHostedService<CreateVacancyConsumer>();

builder.Services
    .AddPortalSettings(builder.Configuration)
    .AddRedis(builder.Configuration)
    .AddApplicationCore()
    .AddInfrastructureService(builder.Configuration);


var app = builder.Build();
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();



app.Run();
