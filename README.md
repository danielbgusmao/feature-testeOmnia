# Ambev Developer Evaluation - Sales API

## 📌 Overview

This project implements a Sales API following Clean Architecture principles using .NET 8, Entity Framework Core, and PostgreSQL.

The API allows full CRUD operations for sales and enforces business rules for quantity-based discounts.

---

## 🚀 Technologies

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* MediatR
* AutoMapper
* xUnit

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
src/Ambev.DeveloperEvaluation.WebApi
```

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=omnia_db;Username=postgres;Password=postgres123"
}
```

---

### 3. Apply database migrations

```bash
dotnet ef database update -p src/Ambev.DeveloperEvaluation.ORM -s src/Ambev.DeveloperEvaluation.WebApi
```

---

### 4. Run the API

```bash
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

---

### 5. Access Swagger

```
https://localhost:<port>/swagger
```

---

## 📦 Main Features

* Create sales with multiple items
* Automatic discount calculation:

  * 4–9 items: 10%
  * 10–20 items: 20%
* Prevent selling more than 20 identical items
* Cancel sales and items (domain ready)
* Clean Architecture (Domain, Application, Infrastructure, API)

---

## 🧪 Example Request

### POST /api/sales

```json
{
  "saleNumber": "SALE-001",
  "customerId": "11111111-1111-1111-1111-111111111111",
  "customerName": "Daniel Customer",
  "branchId": "22222222-2222-2222-2222-222222222222",
  "branchName": "Main Branch",
  "items": [
    {
      "productId": "33333333-3333-3333-3333-333333333333",
      "productName": "Product A",
      "quantity": 4,
      "unitPrice": 100
    }
  ]
}
```

---

## 📊 Expected Behavior

| Quantity | Discount    |
| -------- | ----------- |
| 1–3      | 0%          |
| 4–9      | 10%         |
| 10–20    | 20%         |
| >20      | Not allowed |

---

## 🧠 Architecture

* **Domain**: Business rules and entities
* **Application**: Commands, Queries, Handlers
* **ORM**: Entity Framework Core mappings
* **WebApi**: Controllers and endpoints

---

## 🔥 Notes

* External Identities pattern used (Customer, Product, Branch)
* Discounts are calculated in the domain layer
* Logging simulates event publishing (SaleCreated)

---

## 🚧 Future Improvements

* Implement GET endpoints
* Add cancel endpoints
* Add pagination and filtering
* Improve test coverage
* Add Docker support
