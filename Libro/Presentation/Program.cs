using Application.Profiles;
using AutoDependencyRegistration;
using Domain.Enums;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Validators;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
        options.TokenValidationParameters = new()
        {
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
        policy.RequireClaim("role", Role.Administrator.ToString());
    });

    options.AddPolicy("MustBeLibrarian", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("role", Role.Librarian.ToString());
    });

    options.AddPolicy("MustBePatron", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("role", Role.Patron.ToString());
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();