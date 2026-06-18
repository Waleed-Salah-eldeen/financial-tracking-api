# 💰 Expense Tracker API

A production-ready, highly maintainable RESTful API built to track daily expenses, manage categories, and provide clean financial insights. The project is strictly built following **Onion Architecture** (Clean Architecture) and DDD (Domain-Driven Design) principles to ensure total decoupling of business logic from external frameworks.

---

## 🏗️ Architecture & Design Patterns

The system is split into 4 distinct layers adhering to the **Onion Architecture** rules, where dependencies flow inward:

* **Domain:** Contains Entities, Value Objects, and core business rules (Zero dependencies).
* **Application:** Defines Interfaces, DTOs, and the core business logic/use-cases.
* **Infrastructure:** Implements database context (EF Core), migrations, and external services.
* **Presentation (Web API):** Handles HTTP requests, JWT authentication, and API Controllers.

### Key Architectural Highlights:

* **Result Pattern:** Custom implementation for fluent business workflow and unified error handling without throwing costly exceptions.
* **Data Projection:** Optimized database queries utilizing EF Core `AsNoTracking()` and LINQ projections directly into DTOs to maximize performance.
* **Strongly-Typed API Responses:** Consistent JSON response structure across all endpoints.

---

## 🛠️ Tech Stack & Tools

* **Framework:** .NET 10 Web API
* **ORM:** Entity Framework Core
* **Database:** MS SQL Server
* **Security:** JWT Authentication & Role-based Authorization
* **Validation:** Fluent Data Annotations (Server-side input validation)