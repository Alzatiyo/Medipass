# MS-EHRLogger — Microservicio Asíncrono EHR (.NET 8 / C#)

> **Medipass: Agendamiento Especializado** · Ecosistema de Microservicios  
> Misma arquitectura, lenguaje y convenciones de nombres que **MS-AgendaHub**

---

## 📁 Estructura del Proyecto

```
MsEhrLogger
│
├── Application
│   ├── Ports
│   │   ├── In
│   │   │       IEhrLoggerUseCasePort.cs
│   │   │
│   │   └── Out
│   │           IEhrRecordRepositoryPort.cs
│   │           IEhrEventPublisherPort.cs
│   │
│   └── UseCases
│           EhrLoggerUseCase.cs
│
├── Domain
│   ├── Builders
│   │       EhrRecordBuilder.cs
│   │
│   ├── Enums
│   │       EhrRecordStatus.cs
│   │       EhrEventType.cs
│   │
│   ├── Exceptions
│   │       DomainException.cs
│   │
│   ├── Models
│   │       EhrRecord.cs
│   │       AppointmentSummary.cs
│   │
│   └── Services
│           EhrRecordService.cs
│
├── Infrastructure
│   ├── Adapters
│   │   ├── Persistence
│   │   │       EhrRecordEntity.cs
│   │   │       EhrRecordRepositoryAdapter.cs
│   │   │
│   │   └── Rest
│   │           AppointmentController.cs
│   │           AppointmentConsumerAdapter.cs
│   │           EhrEventPublisherAdapter.cs
│   │
│   ├── Config
│   │       AppDbContext.cs
│   │       InfrastructureServiceExtensions.cs
│   │
│   ├── Dtos
│   │       AppointmentConfirmedEvent.cs
│   │       EhrRecordResponse.cs
│   │
│   └── Mappers
│       │   EhrRecordMapper.cs
│       │
│       └── Interface
│               IEhrRecordMapper.cs
│
├── Tests
│   └── Domain
│           EhrRecordTests.cs
│
├── docker/
│   ├── mongo-init.js
│   ├── prometheus.yml
│   └── grafana/provisioning/datasources/datasources.yml
│
├── appsettings.json
├── appsettings.Docker.json
├── docker-compose.yml
├── Dockerfile
└── MsEhrLogger.csproj
```

---

## 🔗 Compatibilidad con MS-AgendaHub

| MS-AgendaHub (C#) | MS-EHRLogger (C#) | Rol |
|---|---|---|
| `IAppointmentUseCasePort.cs` | `IEhrLoggerUseCasePort.cs` | Puerto de entrada |
| `IAppointmentRepositoryPort.cs` | `IEhrRecordRepositoryPort.cs` | Puerto de persistencia |
| `IEhrEventPublisherPort.cs` | `IEhrEventPublisherPort.cs` | **Nombre idéntico** — simetría intencional |
| `AppointmentUseCase.cs` | `EhrLoggerUseCase.cs` | Caso de uso |
| `AppointmentBuilder.cs` | `EhrRecordBuilder.cs` | Builder GoF |
| `AppointmentService.cs` | `EhrRecordService.cs` | Servicio de dominio |
| `AppointmentRepositoryAdapter.cs` | `EhrRecordRepositoryAdapter.cs` | Adapter GoF |
| `AppointmentController.cs` | `AppointmentController.cs` | **Nombre idéntico** — convención REST |
| `EhrEventPublisherAdapter.cs` | `EhrEventPublisherAdapter.cs` | **Nombre idéntico** — simetría intencional |
| `AppointmentMapper.cs` | `EhrRecordMapper.cs` | Mapper |
| `IAppointmentMapper.cs` | `IEhrRecordMapper.cs` | Interfaz mapper |
| `AppDbContext.cs` | `AppDbContext.cs` | **Nombre idéntico** — configuración DB |
| `InfrastructureServiceExtensions.cs` | `InfrastructureServiceExtensions.cs` | **Nombre idéntico** — DI |

### Contrato del Evento (cola RabbitMQ)

`EhrEventPublisherAdapter.cs` del **MS-AgendaHub** publica hacia `medipass.events` con routing key `appointment.confirmed`.  
`AppointmentConsumerAdapter.cs` del **MS-EHRLogger** consume esa misma cola deserializando `AppointmentConfirmedEvent.cs`.

Los campos del DTO son idénticos en ambos servicios:

```csharp
// Publicado por MS-AgendaHub → Consumido por MS-EHRLogger
public class AppointmentConfirmedEvent {
    public string   EventId            { get; set; }
    public string   AppointmentId      { get; set; }
    public string   PatientId          { get; set; }
    public string   DoctorId           { get; set; }
    public string   Specialty          { get; set; }
    public DateTime ScheduledAt        { get; set; }
    public string   ConsultationRoom   { get; set; }
    public string   InsuranceCode      { get; set; }
    public string   ProcedureCode      { get; set; }
    public bool     InsuranceValidated { get; set; }
    public string   Observations       { get; set; }
    public string   CorrelationId      { get; set; }
    public string   SourceService      { get; set; }
    public DateTime PublishedAt        { get; set; }
}
```

---

## 🚀 Inicio Rápido

```bash
# Clonar y levantar todo el stack
git clone https://github.com/medipass/ms-ehrlogger.git
cd ms-ehrlogger
docker compose up -d --build
```

| Servicio | URL |
|---------|-----|
| **Swagger UI** | http://localhost:8083/swagger |
| **Health Check** | http://localhost:8083/health |
| **Métricas Prometheus** | http://localhost:8083/metrics |
| **RabbitMQ Management** | http://localhost:15672 (`medipass` / `medipass_pass`) |
| **Prometheus** | http://localhost:9090 |
| **Grafana** | http://localhost:3000 (`admin` / `medipass123`) |
| **Jaeger UI** | http://localhost:16686 |

---

## 🧪 Tests

```bash
dotnet test
```

---

## 📦 Variables de Entorno

| Variable | Default |
|----------|---------|
| `MongoDB__ConnectionString` | `mongodb://...@localhost:27019/ehr_db` |
| `MongoDB__DatabaseName` | `ehr_db` |
| `RabbitMQ__Host` | `localhost` |
| `RabbitMQ__Port` | `5672` |
| `RabbitMQ__Username` | `medipass` |
| `RabbitMQ__Password` | `medipass_pass` |
| `Jaeger__Host` | `localhost` |
| `Jaeger__Port` | `6831` |

---

## 🔗 Entregables del Proyecto Medipass

| Entregable | Enlace |
|-----------|--------|
| MS-AgendaHub (Core, C#) | [ms-agendahub](../ms-agendahub/) |
| MS-Insurance (Síncrono) | [ms-insurance](../ms-insurance/) |
| **MS-EHRLogger (Asíncrono, C#)** | ← Este repositorio |
| Docker Compose Ecosistema | [docker-compose.yml](../docker-compose.yml) |
| Documento RFC | [RFC-Medipass.pdf](../docs/RFC-Medipass.pdf) |
| Modelo C4 | [C4-Medipass.png](../docs/C4-Medipass.png) |
