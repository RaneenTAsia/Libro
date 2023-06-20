using Application.Profiles;
using AutoDependencyRegistration;
using Domain.Enums;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Persistence.Settings;
using Infrastructure.Validators;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog();

var logger = new LoggerConfiguration()
        .WriteTo.Console(theme: AnsiConsoleTheme.Code)
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();

//allows us to inject a content type provider in other parts of our code
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

//Because AddFluentValidator is depracated
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>(); // register validators

builder.Services.AddFluentValidationAutoValidation(); // the same old MVC pipeline behavior
builder.Services.AddFluentValidationClientsideAdapters(); // for client side

builder.Services.AddDbContext<LibroDbContext>(DbContextOptions => DbContextOptions.UseSqlServer(builder.Configuration["ConnectionStrings:LibroDbConnectionString"]));

builder.Services.AddAutoMapper(
    typeof(Program).GetTypeInfo().Assembly,
    typeof(UserProfile).GetTypeInfo().Assembly
);
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AutoRegisterDependencies();

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;

        options.TokenValidationParameters = new()
        {
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };
    }
    );

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeAdministrator", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Administrator");
    });

    options.AddPolicy("MustBeLibrarian", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Librarian");
    });

    options.AddPolicy("MustBePatron", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Patron");
    });
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();