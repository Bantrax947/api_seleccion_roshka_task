using Infrastructure.Database;
using WebApi.Dependencies;
using FluentValidation.AspNetCore;
using FluentValidation;
using WebApi.Validator;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Core.Interfaces;
using Core.Services;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos
builder.Services.AddDbContext<SqlServerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Añade los servicios al contenedor de inyección de dependencias
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Agregar FluentValidation y los validadores
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<TareaRequestValidator>();

// Agregar tu lógica de autenticación
builder.Services.AddApiKeyAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<BaseApiController>>();
        var errores = context.ModelState
            .Where(ms => ms.Value.Errors.Count > 0)
            .Select(ms => new { Campo = ms.Key, Errores = ms.Value.Errors.Select(e => e.ErrorMessage) });

        logger.LogWarning("Validación fallida en {Controller}: {@Errores}",
            context.ActionDescriptor.DisplayName, errores);

        return new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState));
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Gestión de Tareas",
        Version = "v1",
        Description = "Una API REST para gestionar tareas y subtareas, incluyendo operaciones CRUD y paginación.",
        Contact = new OpenApiContact
        {
            Name = "Fabian Franco",
            Email = "francofabian947@gmail.com",
            Url = new Uri("https://www.tu-sitio.com")
        }
    });

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingresa tu API Key en el campo de valor",
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    };
    c.AddSecurityRequirement(securityRequirement);

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Tareas v1");
    });
}
app.UseHttpsRedirection();

// Asegúrate de que UseAuthentication esté antes de UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
