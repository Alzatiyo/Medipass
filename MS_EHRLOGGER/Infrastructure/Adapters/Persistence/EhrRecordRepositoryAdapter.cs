using MongoDB.Driver;
using MsEhrLogger.Application.Ports.Out;
using MsEhrLogger.Domain.Enums;
using MsEhrLogger.Domain.Models;
using MsEhrLogger.Infrastructure.Mappers.Interface;

namespace MsEhrLogger.Infrastructure.Adapters.Persistence;

/// <summary>
/// Patrón GoF: ADAPTER
/// Adapta MongoDB al contrato del puerto IEhrRecordRepositoryPort.
/// Espejo exacto de AppointmentRepositoryAdapter.cs del MS-AgendaHub.
///
/// DIP: el dominio no conoce MongoDB; solo conoce IEhrRecordRepositoryPort.
/// </summary>
public class EhrRecordRepositoryAdapter : IEhrRecordRepositoryPort
{
    private readonly IMongoCollection<EhrRecordEntity> _collection;
    private readonly IEhrRecordMapper                  _mapper;

    public EhrRecordRepositoryAdapter(
        IMongoDatabase       database,
        IEhrRecordMapper     mapper)
    {
        _collection = database.GetCollection<EhrRecordEntity>("ehr_records");
        _mapper     = mapper;
        CreateIndexes();
    }

    public async Task<EhrRecord> SaveAsync(EhrRecord record)
    {
        var entity = _mapper.ToEntity(record);
        var filter = Builders<EhrRecordEntity>.Filter.Eq(e => e.Id, entity.Id);
        await _collection.ReplaceOneAsync(filter, entity,
            new ReplaceOptions { IsUpsert = true });
        return record;
    }

    public async Task<EhrRecord?> FindByIdAsync(string id)
    {
        var entity = await _collection
            .Find(e => e.Id == id)
            .FirstOrDefaultAsync();
        return entity is null ? null : _mapper.ToDomain(entity);
    }

    public async Task<EhrRecord?> FindByAppointmentIdAsync(string appointmentId)
    {
        var entity = await _collection
            .Find(e => e.AppointmentId == appointmentId)
            .FirstOrDefaultAsync();
        return entity is null ? null : _mapper.ToDomain(entity);
    }

    public async Task<IEnumerable<EhrRecord>> FindByPatientIdAsync(string patientId)
    {
        var entities = await _collection
            .Find(e => e.PatientId == patientId)
            .SortByDescending(e => e.OccurredAt)
            .ToListAsync();
        return entities.Select(_mapper.ToDomain);
    }

    public async Task<IEnumerable<EhrRecord>> FindByStatusAsync(EhrRecordStatus status)
    {
        var entities = await _collection
            .Find(e => e.Status == status)
            .ToListAsync();
        return entities.Select(_mapper.ToDomain);
    }

    public async Task<IEnumerable<EhrRecord>> FindAllAsync(int page, int size)
    {
        var entities = await _collection
            .Find(_ => true)
            .SortByDescending(e => e.OccurredAt)
            .Skip(page * size)
            .Limit(size)
            .ToListAsync();
        return entities.Select(_mapper.ToDomain);
    }

    public async Task<long> CountByStatusAsync(EhrRecordStatus status) =>
        await _collection.CountDocumentsAsync(e => e.Status == status);

    // ── Índices MongoDB ────────────────────────────────────────────────────

    private void CreateIndexes()
    {
        var indexKeys = Builders<EhrRecordEntity>.IndexKeys;

        var indexes = new[]
        {
            new CreateIndexModel<EhrRecordEntity>(
                indexKeys.Ascending(e => e.AppointmentId),
                new CreateIndexOptions { Unique = true, Name = "idx_appointmentId" }),

            new CreateIndexModel<EhrRecordEntity>(
                indexKeys.Ascending(e => e.PatientId),
                new CreateIndexOptions { Name = "idx_patientId" }),

            new CreateIndexModel<EhrRecordEntity>(
                indexKeys.Ascending(e => e.Status),
                new CreateIndexOptions { Name = "idx_status" }),

            new CreateIndexModel<EhrRecordEntity>(
                indexKeys.Ascending(e => e.CorrelationId),
                new CreateIndexOptions { Name = "idx_correlationId" }),

            new CreateIndexModel<EhrRecordEntity>(
                indexKeys.Descending(e => e.OccurredAt),
                new CreateIndexOptions { Name = "idx_occurredAt" })
        };

        _collection.Indexes.CreateMany(indexes);
    }
}
