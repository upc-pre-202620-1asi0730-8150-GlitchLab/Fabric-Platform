# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.1.0] - 2026-11-03

### Added
- **Machinery Bounded Context — Domain Layer:**
  - Added `Machine` aggregate root with `ReportDowntime()`, `ResumeOperation()`, `RegisterMaintenance()`, `UpdateStatus()` and accumulated downtime calculation.
  - Added `MachineDowntime` and `MaintenanceRecord` entities.
  - Added strongly-typed identifiers `MachineId`, `MachineDowntimeId` and `MaintenanceRecordId`.
  - Added `EMachineStatus` (`Operational`, `InMaintenance`, `Stopped`) and `EMaintenanceType` (`Preventive`, `Corrective`, `Calibration`) enumerations.
  - Added commands `RegisterMachineCommand`, `UpdateMachineStatusCommand`, `ReportMachineDowntimeCommand`, `ResumeMachineOperationCommand` and `RegisterMaintenanceRecordCommand`.
  - Added queries `GetAllMachinesQuery`, `GetMachineByIdQuery`, `GetMachineByCodeQuery`, `GetMachineDowntimesByMachineIdQuery` and `GetMaintenanceRecordsByMachineIdQuery`.
  - Added domain events `MachineRegisteredEvent` and `MachineDowntimeReportedEvent`.
  - Added `MachineryError` enumeration and `MachineryErrors` error catalog.
  - Added `IMachineRepository` contract.
- **Machinery Bounded Context — Application Layer:**
  - Added `MachineCommandService` and `MachineQueryService` with their interfaces.
  - Added `MachineRegisteredEventHandler` and `MachineDowntimeReportedEventHandler`.
- **Machinery Bounded Context — Infrastructure Layer:**
  - Added `MachineRepository` over Entity Framework Core.
  - Added `ApplyMachineryConfiguration()` Fluent API mapping for `machines`, `machine_downtimes` and `maintenance_records`.
- **Machinery Bounded Context — Interfaces Layer:**
  - Added `MachinesController` (`POST/GET /api/v1/machines`, `GET /api/v1/machines/{machineId}`, `PATCH /api/v1/machines/{machineId}/status`).
  - Added `MachineDowntimesController` (`POST/GET /api/v1/machines/{machineId}/downtimes`, `PATCH /api/v1/machines/{machineId}/downtimes/{downtimeId}/resumption`).
  - Added `MachineMaintenancesController` (`POST/GET /api/v1/machines/{machineId}/maintenances`).
  - Added REST resources, resource/command assemblers and `MachineryActionResultAssembler` with RFC 9457 `ProblemDetails`.
- **Localization:**
  - Added Machinery error messages in English and Spanish, `CommonMessages` resources and `MachineryMessages` resources.
- **Configuration:**
  - Configured `Program.cs` with kebab-case routing, CORS, MySQL `AppDbContext`, localization, `ProblemDetails`, global exception handler, OpenAPI/Scalar, Cortex Mediator and Machinery dependency injection.
  - Added Machinery requests to `FabricPlatform.API.http`.

### Changed
- `IBaseRepository` and `BaseRepository` now include `FindByIdAsNoTrackingAsync()` and `ListAsNoTrackingAsync()`, and their query methods are `virtual` so repositories can include child entities.
- `BaseRepository.FindByIdAsync()` now supports strongly-typed identifiers.
- `AppDbContext` now applies the Machinery configuration.
- Renamed `ErrorMessages.en.resx` to `ErrorMessages.resx` (neutral resources) and `CommonMessage.resx` to `CommonMessages.resx`.

### Fixed
- Fixed Entity Framework Core package names in `FabricPlatform.API.csproj` (`Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Relational`, `Microsoft.EntityFrameworkCore.Design`).

### Deprecated
- Nothing yet.

### Removed
- Removed the default `weatherforecast` request from `FabricPlatform.API.http`.

### Security
- Nothing yet.

---

## [1.0.0] - 2026-11-03

### Added
- **Project Configuration:**
  - Created the `fabric-platform` solution with the `FabricPlatform.API` project targeting .NET 10 (`net10.0`).
  - Added `global.json` to pin the .NET SDK version.
  - Added `.gitignore` for build artifacts and IDE files.
  - Added `appsettings.Development.json` and `appsettings.Production.json` with database connection and token settings.
  - Added package references: `Microsoft.AspNetCore.OpenApi`, `Scalar.AspNetCore`, Entity Framework Core, `MySql.EntityFrameworkCore`, `Humanizer` and `Cortex.Mediator`.
- **Shared Kernel — Domain Layer:**
  - Added `IAggregateRoot` interface and `AggregateRoot` base class for managing the in-memory domain event lifecycle (`IEvent`).
  - Added `IAuditableEntity` interface for creation and last update timestamps.
  - Added `IEntityId` interface for strongly-typed entity identifiers.
  - Added `IEvent` interface for domain event notification.
  - Added `Error` record and `EErrorType` enumeration for domain error handling.
  - Added `IBaseRepository` and `IUnitOfWork` contracts.
- **Shared Kernel — Application Layer:**
  - Added `Result` model for explicit success/failure operation results.
  - Added `IEventHandler` interface for domain event handlers.
- **Shared Kernel — Infrastructure Layer:**
  - Added `AppDbContext` and `ModelBuilderExtensions` / `StringExtensions` for snake_case table and column naming in Entity Framework Core.
  - Added `BaseRepository` and `UnitOfWork` implementations.
  - Added `StronglyTypedIdConverter` for persisting strongly-typed IDs with Entity Framework Core.
  - Added `AuditableEntityInterceptor` to set entity timestamps automatically.
  - Added `DispatchDomainEventsInterceptor` to publish domain events through `Cortex.Mediator` after `SaveChanges`.
  - Added `TransactionCommandBehavior`, `LoggingCommandBehavior` and `ExceptionHandlingCommandBehavior` mediator pipeline behaviors for command processing.
  - Added `GlobalExceptionHandler` and `GlobalExceptionHandlerMiddleware` with `MiddlewareExtensions` for standardized error responses.
  - Added `KebabCaseRouteNamingConvention` and string extensions for kebab-case REST routes.
  - Added `BearerSecuritySchemeTransformer` and `IamBearerSecurityOperationTransformer` for OpenAPI Bearer security documentation.
- **Shared Kernel — Interfaces Layer:**
  - Added `ProblemDetailsFactory` for RFC 9457 `ProblemDetails` error responses.
- **Localization:**
  - Added `CommonMessages` and `ErrorMessages` resources in English and Spanish.
- **Documentation:**
  - Added `docs/user-stories.md` with the REST API technical stories for the Machine Registry, Production Tracking, Quality Management, Reporting & Analytics and IAM bounded contexts.

### Changed
- Reorganized the `IAuditableEntity` namespace for better organization.
- Added missing `System` using directives across Shared Kernel files.

### Fixed
- Nothing yet.

### Deprecated
- Nothing yet.

### Removed
- Removed obsolete project files and unused files from the initial template.

### Security
- Nothing yet.
