using Application;
using Infrastructure;
using Infrastructure.DataAccess;
using Shared;
using Shared.Settings;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://careers.yelo.az")  /*http://recruitment-test.yelo.az*/
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
//connectionString>>
var canvas = SkiaSharp.SKColorType.Alpha16;
Environment.SetEnvironmentVariable("DOTNET_SYSTEM_DRAWING_ENABLESKIA", "1");

var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
//connectionString<<
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddFastEndpoints().SwaggerDocument(o => o.ShortSchemaNames = true);
builder.Services.AddHttpClient();

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

builder.Services
    .AddPortalSettings(builder.Configuration)
    .AddRedis(builder.Configuration)
    .AddApplicationCore()
    .AddInfrastructureService(builder.Configuration)
    .AddMinioClient(builder.Configuration);


builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

var app = builder.Build();
app.UseRouting();
app.UseExceptionHandling();
app.UseFastEndpoints(c => c.Endpoints.ShortNames = true);
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

//if (!app.Environment.IsProduction())
//{
//    app.UseSwaggerGen();

//}
app.UseSwaggerGen();
app.UseSwagger();
app.UseSwaggerUI();


app.Run();

