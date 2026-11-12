# Fabric Platform — REST API Technical Stories

## Overview

This document contains API-focused technical stories intended for frontend developers integrating the Fabric web application (Vue 3) with the Fabric Platform REST API (ASP.NET Core / .NET 10).

The stories are traced to the Technical Stories (TS) defined in the Fabric project report (section 3.1, Epic EP008) and to the related User Stories of each bounded context.

Common conventions:
- Base path: `/api/v1`
- Content-Type: `application/json`
- Routes use kebab-case and plural resource names.
- Error responses follow standard RFC 9457 `ProblemDetails` format (`type`, `title`, `status`, `detail`, `instance`).
- Error messages are localized (`en`, `es`) through the `Accept-Language` header.
- Authentication: Standard HTTP Bearer token via `Authorization: Bearer <token>` header (once the IAM context is implemented).

---

## Bounded Context: Machine Registry

The Machine Registry context manages the sewing machines of the workshop, their operational status, their downtimes and their maintenance history.

Related user stories: US007, US008, US017, US018 (EP004 — Sewing Machinery Tracking).

### TS004 — Machine Management API
As a frontend developer, I want to register, query and update the status of machines through the API so that I can implement the Machinery screens and show the current operational condition of each machine.

Acceptance criteria:
- **Scenario: Successful machine registration**
  - Given a `POST` request to `/api/v1/machines` is received with a body containing: `code`, `type`, `location`, `acquiredDate`
  - When the API validates and persists the machine aggregate
  - Then the API responds `201 Created` with a `Location` header (`/api/v1/machines/{machineId}`) and returns `MachineResource` (`id`, `code`, `type`, `status`, `location`, `acquiredDate`, `accumulatedDowntimeMinutes`)
  - And the new machine starts with status `Operational`
- **Scenario: Duplicate machine code**
  - Given a `POST` request to `/api/v1/machines` with a `code` that already exists
  - Then the API responds `409 Conflict` with a problem details payload
- **Scenario: Validation error**
  - Given a `POST` request with a blank `code` or `type`
  - Then the API responds `400 Bad Request`
- **Scenario: Get all machines**
  - Given machines are registered
  - When a `GET` request to `/api/v1/machines` is received
  - Then the API responds `200 OK` with an array of `MachineResource` items and their current status (empty array if no machines exist)
- **Scenario: Get machine by id**
  - Given a `GET` request to `/api/v1/machines/{machineId}` is received
  - When the machine exists, the API responds `200 OK` with `MachineResource`
  - When the machine does not exist, the API responds `404 Not Found`
- **Scenario: Get machine by code**
  - Given a `GET` request to `/api/v1/machines?code={code}` is received
  - When a machine with that code exists, the API responds `200 OK` with an array containing that `MachineResource`
  - When no machine has that code, the API responds `200 OK` with an empty array
- **Scenario: Update machine status**
  - Given a machine exists and a `PUT` request to `/api/v1/machines/{machineId}/status` is received with a body containing a valid `status` (`Operational`, `InMaintenance`, `Stopped`)
  - When the API applies the status change
  - Then the API responds `200 OK` with the updated `MachineResource`
  - If the `status` value is invalid, the API responds `400 Bad Request`
  - If the machine does not exist, the API responds `404 Not Found`

---

### TS010 — Machine Downtime and Dead-Time API
As a frontend developer, I want to register machine stops and their resumption through the API so that I can implement the breakdown report and resume flows, and display the accumulated dead time of each machine.

Acceptance criteria:
- **Scenario: Register a downtime**
  - Given a machine exists and a `POST` request to `/api/v1/machines/{machineId}/downtimes` is received with a body containing: `reason`, `startTime`
  - When the API registers the downtime in the machine aggregate
  - Then the machine status changes to `Stopped` and the API responds `201 Created` with `MachineDowntimeResource` (`id`, `machineId`, `reason`, `startTime`, `endTime`, `durationMinutes`)
- **Scenario: Downtime without reason**
  - Given a `POST` request to `/api/v1/machines/{machineId}/downtimes` with a blank `reason`
  - Then the API responds `400 Bad Request` and nothing is stored
- **Scenario: Machine already stopped**
  - Given the machine already has an open downtime
  - When a new `POST` request to `/api/v1/machines/{machineId}/downtimes` is received
  - Then the API responds `409 Conflict`
- **Scenario: Resume operation**
  - Given an open downtime exists and a `PUT` request to `/api/v1/machines/{machineId}/downtimes/{downtimeId}/resumption` is received with a body containing: `endTime`, `solution`
  - When the API closes the downtime
  - Then the API calculates `durationMinutes`, restores the machine status to `Operational` and responds `200 OK` with the updated `MachineDowntimeResource`
  - If `endTime` is earlier than `startTime`, the API responds `400 Bad Request`
  - If the downtime is already closed, the API responds `409 Conflict`
  - If the machine or downtime does not exist, the API responds `404 Not Found`
- **Scenario: Get downtimes of a machine**
  - Given a `GET` request to `/api/v1/machines/{machineId}/downtimes` is received
  - Then the API responds `200 OK` with the downtimes ordered chronologically
  - If the machine does not exist, the API responds `200 OK` with an empty array

---

### TS013 — Maintenance History API
As a frontend developer, I want to register and query the maintenance records of each machine through the API so that I can implement the maintenance history view and support preventive maintenance planning.

Acceptance criteria:
- **Scenario: Register a maintenance**
  - Given a machine exists and a `POST` request to `/api/v1/machines/{machineId}/maintenances` is received with a body containing: `technician`, `maintenanceType`, `description`, `performedDate`
  - When the API stores the maintenance record in the machine aggregate
  - Then the API responds `201 Created` with `MaintenanceRecordResource` (`id`, `machineId`, `technician`, `maintenanceType`, `description`, `performedDate`)
- **Scenario: Incomplete maintenance record**
  - Given a `POST` request without `performedDate` or with a blank `technician`
  - Then the API responds `400 Bad Request` and nothing is stored
- **Scenario: Machine not found**
  - Given a `POST` request to `/api/v1/machines/{machineId}/maintenances` for a non-existent machine
  - Then the API responds `404 Not Found`
- **Scenario: Get maintenance history**
  - Given a `GET` request to `/api/v1/machines/{machineId}/maintenances` is received
  - Then the API responds `200 OK` with the maintenance records ordered chronologically by `performedDate`

---

## Bounded Context: Production Tracking

The Production Tracking context manages production batches, their stages, movements, operators and observations.

### TS001 — Production Batch Management API
As a frontend developer, I want to register and query production batches through the API so that I can implement the Production Batches screens.

Acceptance criteria:
- **Scenario: Successful create**
  - Given a `POST` request to `/api/v1/production-batches` is received with a body containing: `model`, `quantity`, `technicalSheetId`
  - When the API validates and persists the batch aggregate
  - Then the API responds `201 Created` with a `Location` header (`/api/v1/production-batches/{batchId}`) and returns `ProductionBatchResource` (`id`, `model`, `quantity`, `stage`, `status`, `technicalSheetId`, `createdDate`)
  - If `quantity` is not greater than zero or `model` is blank, the API responds `400 Bad Request`
- **Scenario: Get all batches**
  - Given a `GET` request to `/api/v1/production-batches` is received
  - Then the API responds `200 OK` with an array of `ProductionBatchResource` items
- **Scenario: Get batch by id**
  - Given a `GET` request to `/api/v1/production-batches/{batchId}` is received
  - When found, the API responds `200 OK`; otherwise `404 Not Found`

---

### TS007 — Operator Assignment API
As a frontend developer, I want to assign operators to a stage of a production batch so that I can implement personnel traceability in the workshop.

Acceptance criteria:
- **Scenario: Successful assignment**
  - Given a batch exists and a `POST` request to `/api/v1/production-batches/{batchId}/operator-assignments` is received with a body containing: `operatorName`, `stage`
  - Then the API registers the assignment and responds `201 Created` with `OperatorAssignmentResource`
- **Scenario: Duplicate assignment**
  - Given the operator is already assigned to the same stage and batch
  - When the same `POST` request is received
  - Then the API responds `400 Bad Request`
- **Scenario: Get assignments**
  - Given a `GET` request to `/api/v1/production-batches/{batchId}/operator-assignments` is received
  - Then the API responds `200 OK` with the assignments of the batch

---

### TS008 — Batch Observations API
As a frontend developer, I want to register and query operational notes of a batch so that I can document incidents and agreements without altering the formal defect flow.

Acceptance criteria:
- **Scenario: Successful create**
  - Given a `POST` request to `/api/v1/production-batches/{batchId}/observations` is received with a body containing: `content`, `responsible`
  - Then the API stores the observation and responds `201 Created` with `BatchObservationResource`
- **Scenario: Blank content**
  - Given `content` is empty or only whitespace
  - Then the API responds `400 Bad Request`
- **Scenario: Get observations**
  - Given a `GET` request to `/api/v1/production-batches/{batchId}/observations` is received
  - Then the API responds `200 OK` with the observations ordered from newest to oldest

---

### TS009 — Batch Stage Transition and Traceability API
As a frontend developer, I want to update the stage of a batch and query its movements so that I can show batch progress and control garment differences between stages.

Acceptance criteria:
- **Scenario: Successful stage update**
  - Given a batch exists and a `PUT` request to `/api/v1/production-batches/{batchId}/stage` is received with a body containing: `stage`, `inputQuantity`, `outputQuantity`, `responsible`
  - Then the API updates the stage, registers a `LotMovement`, recalculates the progress percentage and responds `200 OK` with `ProductionBatchResource`
- **Scenario: Batch not found**
  - Given the `batchId` does not exist
  - Then the API responds `404 Not Found`
- **Scenario: Get movements**
  - Given a `GET` request to `/api/v1/production-batches/{batchId}/movements` is received
  - Then the API responds `200 OK` with the movements ordered chronologically

---

## Bounded Context: Quality Management

The Quality Management context manages fabric roll inspections, quality inspections of garments and defect records.

### TS002 — Quality Inspection API
As a frontend developer, I want to register and query quality inspections so that I can implement the Quality screens.

Acceptance criteria:
- **Scenario: Successful create**
  - Given a `POST` request to `/api/v1/quality-inspections` is received with a body containing: `batchId`, `machineId`, `inspectedDate`, `totalInspected`
  - Then the API registers the inspection and responds `201 Created` with `QualityInspectionResource`
  - If `totalInspected` is not greater than zero, the API responds `400 Bad Request`
- **Scenario: Get inspections**
  - Given a `GET` request to `/api/v1/quality-inspections` is received
  - Then the API responds `200 OK` with an array of `QualityInspectionResource` items

---

### TS003 — Quality Defects API
As a frontend developer, I want to register and query the defects found in garments so that I can link quality incidents to batches.

Acceptance criteria:
- **Scenario: Successful create**
  - Given a `POST` request to `/api/v1/quality-inspections/{inspectionId}/defects` is received with a body containing: `defectType`, `quantity`, `origin`, `description`
  - Then the API registers the defect and responds `201 Created` with `DefectRecordResource`
- **Scenario: Get defects by batch**
  - Given a `GET` request to `/api/v1/defects?batchId={batchId}` is received
  - Then the API responds `200 OK` with the defects associated with that batch

---

### TS011 — Defect Evidence and Disposition API
As a frontend developer, I want to attach evidences and register the disposition of a defect so that the quality team can classify observed garments with photographic support.

Acceptance criteria:
- **Scenario: Register defect with machine**
  - Given a `POST` request to `/api/v1/defects` is received with a body containing: `batchId`, `defectType`, `quantity`, `machineId`
  - Then the API generates a unique defect identifier and responds `201 Created`
- **Scenario: Attach evidences**
  - Given a defect exists and a `PUT` request to `/api/v1/defects/{defectId}/evidences` is received with a list of evidences
  - Then the API stores the evidences and responds `200 OK`
- **Scenario: Register disposition**
  - Given a defect in evaluation and a `PUT` request to `/api/v1/defects/{defectId}/disposition` is received with a body containing: `disposition`
  - Then the API updates the status of the defect and responds `200 OK`

---

## Bounded Context: Reporting & Analytics

The Reporting & Analytics context consolidates production, quality and machinery data into indicators, alerts and reports.

### TS005 — Indicators and Alerts API
As a frontend developer, I want to query production indicators and alerts so that I can display up-to-date operational information.

Acceptance criteria:
- **Scenario: Get indicators**
  - Given production data exists
  - When a `GET` request to `/api/v1/indicators` is received
  - Then the API responds `200 OK` with the calculated indicators
- **Scenario: Get alerts**
  - Given alerts generated by deviations in production, quality or machinery exist
  - When a `GET` request to `/api/v1/alerts` is received
  - Then the API responds `200 OK` with the alerts and their status

---

### TS012 — Dashboard Metrics API
As a frontend developer, I want to query aggregated metrics and filtered alerts so that I can feed the cards and charts of the Dashboard.

Acceptance criteria:
- **Scenario: Metrics by date**
  - Given a `GET` request to `/api/v1/dashboard-metrics?date={date}` is received
  - Then the API responds `200 OK` with the consolidated production and compliance indicators
  - If there are no records for that date, the API responds `200 OK` with all indicators in zero
- **Scenario: Alerts by severity**
  - Given a `GET` request to `/api/v1/alerts?severity={severity}` is received
  - Then the API responds `200 OK` with only the alerts of that severity

---

## Bounded Context: IAM (Identity & Access Management)

The IAM context manages user accounts and credentials.

### TS006 — User Management API
As a frontend developer, I want to register, query and update users through the API so that I can implement registration and profile flows.

Acceptance criteria:
- **Scenario: Successful registration**
  - Given a `POST` request to `/api/v1/users` is received with valid user data
  - Then the API registers the user and responds `201 Created` with a `Location` header (`/api/v1/users/{userId}`) and `UserResource`
  - If the username already exists, the API responds `409 Conflict`
- **Scenario: Update user data**
  - Given a user exists and a `PUT` request to `/api/v1/users/{userId}` is received with valid data
  - Then the API updates the user and responds `200 OK` with the updated `UserResource`
  - If the user does not exist, the API responds `404 Not Found`
