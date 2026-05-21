using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Adapters.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Configurar Kestrel para escuchar en el puerto HTTP 5002
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5002);
});

// Registrar servicios de la capa de Infraestructura
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "MS-Insurance - Medipass",
        Version = "v1",
        Description = "Microservicio Síncrono para la validación de seguros y cobertura de procedimientos médicos."
    });
});

var app = builder.Build();

// Aplicar migraciones y precargar base de datos automáticamente al iniciar con política de reintentos
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
    int retries = 10;
    while (retries > 0)
    {
        try
        {
            context.Database.Migrate();
            Console.WriteLine("Migraciones aplicadas correctamente y base de datos inicializada.");
            break;
        }
        catch (Exception ex)
        {
            retries--;
            Console.WriteLine($"Base de datos no disponible aún. Reintentando en 5 segundos... ({retries} intentos restantes). Detalle: {ex.Message}");
            if (retries == 0)
            {
                Console.WriteLine("Error crítico: No se pudo establecer conexión con la base de datos tras varios intentos.");
                throw;
            }
            Thread.Sleep(5000);
        }
    }
}

// Configurar la canalización de solicitudes HTTP
if (app.Environment.IsDevelopment() || true) // Permitir swagger siempre para desarrollo y pruebas
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MS-Insurance v1");
    });
}

// Deshabilitamos redirección HTTPS para facilitar pruebas y comunicación interna de microservicios
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
