using Infrastructure.Database;
using WebApi.Dependencies;
using FluentValidation.AspNetCore;
using FluentValidation;
using WebApi.Validator;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Core.Interfaces;
using Core.Services;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SqlServerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ISubTaskRepository, SubTaskRepository>();
builder.Services.AddScoped<ISubTaskService, SubTaskService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<TareaRequestValidator>();
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

        logger.LogWarning("Validaci�n fallida en {Controller}: {@Errores}",
            context.ActionDescriptor.DisplayName, errores);

        return new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState));
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api Roshka",
        Version = "v1",
        Description = "Api Rest para gestionar tareas y subtareas para entrevista t�cnica ROSHKA",
        Contact = new OpenApiContact
        {
            Name = "Fabian Franco",
            Email = "francofabian947@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/franco-fabian")
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();