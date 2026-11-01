using Microsoft.EntityFrameworkCore;
using ApiEmpresa.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<Conexiones>(options =>
    options.UseAzureSql(
        builder.Configuration.GetConnectionString("ConexionSQLAzure")
        ?? throw new InvalidOperationException(
            "No se encontró la conexión ConexionSQLAzure."
        )
    ));

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirBlazor", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// HTTPS termina en Azure App Service; los contenedores usan el puerto HTTP 8080.

app.UseCors("PermitirBlazor");

app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapControllers();

app.Run();
