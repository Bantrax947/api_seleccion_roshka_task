using Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AgregarCore();
builder.Services.AgregarInfrastructura();

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

app.UseAuthorization();
app.MapControllers();
app.Run();