using MongoDB.Driver;

namespace MsEhrLogger.Infrastructure.Config;

/// <summary>
/// Contexto de MongoDB para el MS-EHRLogger.
/// Espejo de AppDbContext.cs del MS-AgendaHub.
/// Centraliza la configuración de la base de datos exclusiva del servicio.
/// </summary>
public class AppDbContext
{
    public IMongoDatabase Database { get; }

    public AppDbContext(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDB:ConnectionString"]
            ?? "mongodb://ehrlogger_user:ehrlogger_pass@localhost:27019/ehr_db?authSource=ehr_db";
        var databaseName = configuration["MongoDB:DatabaseName"] ?? "ehr_db";

        var client   = new MongoClient(connectionString);
        Database     = client.GetDatabase(databaseName);
    }
}
