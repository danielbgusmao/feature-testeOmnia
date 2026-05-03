# Ambev Developer Evaluation - Sales API

## 📌 Overview

This project implements a Sales API following Clean Architecture principles using .NET 8, Entity Framework Core, and PostgreSQL.

It supports full CRUD operations for sales and includes business rules for quantity-based discounts.

---

## 🚀 Technologies

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* xUnit (for tests)

---

## ⚙️ How to run the project

### 1. Clone the repository

```bash
git clone <repository-url>
cd <repository-folder>
```

---

### 2. Configure the database

Update the `appsettings.json` file in:

```
Ambev.DeveloperEvaluation.WebApi
```

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=omnia_db;Username=postgres;Password=postgres123"
}
```

---

### 3. Apply database migrations

```bash
dotnet ef migrations add InitialSales -p Ambev.DeveloperEvaluation.ORM -s Ambev.DeveloperEvaluation.WebApi
dotnet ef database update -p Ambev.DeveloperEvaluation.ORM -s Ambev.DeveloperEvaluation.WebApi
```

---

### 4. Run the API

```bash
dotnet run --project Ambev.DeveloperEvaluation.WebApi
```

---

## 📦 Features

* Create, update, delete and retrieve sales
* Add and manage sale items
* Cancel sales and individual items
* Automatic discount calculation based on quantity:

  * 4–9 items: 10%
  * 10–20 items: 20%
* Validation rules enforced in the domain layer

---

## 🧪 Running tests

```bash
dotnet test
```

---

## 📚 Notes

* External Identities pattern is used for Customer, Product, and Branch.
* Discounts and business rules are implemented in the domain layer.
* Logging simulates event publishing (SaleCreated, SaleModified, etc).

---

## 🔥 Future improvements

* Add message broker integration (RabbitMQ / Rebus)
* Add pagination and filtering for queries
* Improve test coverage
* Docker support
