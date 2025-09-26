# Alfa.Backend

Advanced **ASP.NET Core API** following **Clean Architecture** principles, implementing **CQRS** with **MediatR**. This solution is built for maintainability, scalability, and testability using best practices and a modular layered architecture.

---

## 🏗️ Architecture Overview

| Layer / Project | Responsibility |
|------------------|-----------------|
| **Web.API** | HTTP endpoints, routing, controllers, API surface |
| **Application** | CQRS handlers, application logic, DTOs, validation |
| **Domain** | Domain entities, value objects, domain services, business rules |
| **Infrastructure** | Data persistence (EF Core), external integrations, repositories |
| **Shared** | Shared abstractions, common utilities, cross-cutting concerns |
| **RabbitMQConsumerService** | Background message consumer service (if used) |

---

## 🚀 Features & Key Concepts

- Clean Architecture separation of concerns  
- CQRS (Command / Query Responsibility Segregation)  
- MediatR for request/response pipeline & decoupled handlers  
- FluentValidation (or some validation mechanism)  
- Entity Framework Core for database access  
- Extensible, modular structure for adding new features  
- Separation of infrastructure from application logic  
- Support for background services (RabbitMQ consumer, etc)  
- Docker / container support (if configured in repo)  

---

## 📦 Getting Started

### Prerequisites

- .NET SDK (version X.Y or later)  
- SQL Server / PostgreSQL / (whatever DB your project uses)  
- Docker (optional, if docker setup included)  

### Setup

1. Clone the repository  
   ```bash
   git clone https://github.com/ilahinasirov/alfa.backend.git
   cd alfa.backend
