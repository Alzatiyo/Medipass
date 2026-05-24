using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Forzar a MS-AgendaHub a correr en el puerto HTTP 5001
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "MS-AgendaHub - Medipass",
        Version = "v1",
        Description = "Microservicio Principal de Agendamiento de Citas Médicas Especializadas."
    });
});

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Aplicar migraciones automáticamente al iniciar con política de reintentos (necesario para Docker)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    int retries = 10;
    while (retries > 0)
    {
        try
        {
            context.Database.Migrate();
            Console.WriteLine("Migraciones aplicadas correctamente y base de datos inicializada en AgendaHub.");
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

if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MS-AgendaHub v1");
    });
}

// Deshabilitar redirección HTTPS para entorno local
// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
