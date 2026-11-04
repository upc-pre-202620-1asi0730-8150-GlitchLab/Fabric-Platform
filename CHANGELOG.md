# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
