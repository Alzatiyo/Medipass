using Infrastructure.Config;

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
