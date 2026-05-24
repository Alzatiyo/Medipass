Aplication
│
├── Ports
│   ├── In
│   │       IAppointmentUseCasePort.cs
│   │
│   └── Out
│           IAppointmentRepositoryPort.cs
│           IInsuranceServicePort.cs
│           IEhrEventPublisherPort.cs
│           IDoctorAvailabilityPort.cs
│
└── UseCases
        AppointmentUseCase.cs

Domain
│
├── Builders
│       AppointmentBuilder.cs
│
├── Enums
│       AppointmentStatus.cs
│       InsuranceStatus.cs
│       MedicalSpecialty.cs
│
├── Exceptions
│       DomainException.cs
│
├── Models
│       Appointment.cs
│       Doctor.cs
│       Patient.cs
│
└── Services
        AppointmentService.cs

Infrastructure
│
├── Adapters
│   ├── Persistence
│   │       AppointmentEntity.cs
│   │       DoctorEntity.cs
│   │       PatientEntity.cs
│   │       AppointmentRepositoryAdapter.cs
│   │
│   └── Rest
│           AppointmentController.cs
│           InsuranceServiceAdapter.cs
│           DoctorAvailabilityAdapter.cs
│           EhrEventPublisherAdapter.cs
│
├── Config
│       AppDbContext.cs
│       InfrastructureServiceExtensions.cs
│
├── Dtos
│       CreateAppointmentRequest.cs
│       UpdateAppointmentStatusRequest.cs
│
├── Mappers
│   │   AppointmentMapper.cs
│   │
│   └── Interface
│           IAppointmentMapper.cs
│
└── Migrations
        InitialCreate.cs