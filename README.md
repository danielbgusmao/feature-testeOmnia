# Ambev Developer Evaluation - Sales API

## 📌 Overview

This project implements a Sales API following Clean Architecture principles using .NET 8, Entity Framework Core, and PostgreSQL.

The API supports full sales lifecycle management, including creation, retrieval, update, cancellation, and listing of sales, with business rules applied at the domain level.

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

Update the connection string in:

```text
src/Ambev.DeveloperEvaluation.WebApi/appsettings.json
```

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=omnia_db;Username=postgres;Password=postgres123"
}
```

---

### 3. Apply migrations

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
http://localhost:<port>/swagger
```

---

## 📦 Endpoints

### ➕ Create Sale

```http
POST /api/sales
```

---

### 🔍 Get Sale by ID

```http
GET /api/sales/{id}
```

---

### ✏️ Update Sale

```http
PATCH /api/sales/{id}
```

---

### ❌ Cancel Sale

```http
PATCH /api/sales/{id}/cancel
```

---

### 📄 List Sales (Pagination & Filters)

```http
GET /api/sales
```

#### Query Parameters

* page (default: 1)
* pageSize (default: 10, max: 100)
* saleNumber
* customerId
* customerName
* branchId
* branchName
* isCancelled
* startDate
* endDate
* orderBy (saleDate, totalAmount, saleNumber, customerName, branchName)
* orderDirection (asc, desc)

---

### 🔎 Example - List Sales

```http
GET /api/sales?page=1&pageSize=10&customerName=Daniel&orderBy=totalAmount&orderDirection=desc
```

---

## 🧪 Example Request

### Create Sale

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

## 📬 Postman Collection

You can import the Postman collection to test all endpoints easily:

👉 [Download Postman Collection](docs/Localhost.postman_collection.json)

After importing, update the base URL if needed and run the requests.

---

### ⚠️ Notes

* The collection includes:

  * Create Sale
  * Get Sale by Id
  * Update Sale
  * Cancel Sale
  * List Sales (with filters and pagination)



---

## 📊 Business Rules

### Discount Tiers

| Quantity | Discount |
| -------- | -------- |
| 1–3      | 0%       |
| 4–9      | 10%      |
| 10–20    | 20%      |

---

### Restrictions

* ❌ Maximum 20 items per product
* ❌ No discount for less than 4 items

---

## 🧠 Architecture

The project follows Clean Architecture:

* **Domain** → Business rules and entities
* **Application** → Commands, Queries, Handlers
* **ORM** → Entity Framework Core mappings
* **WebApi** → Controllers and endpoints

---

## 🔥 Additional Features

* Domain-driven design (DDD)
* Automatic discount calculation
* Event simulation using logging:

  * SaleCreated
  * SaleModified
  * SaleCancelled
* Validation at domain level
* Pagination and filtering for listing sales

---

## 🚧 Future Improvements

* Implement item cancellation
* Add integration tests
* Add authentication to secure endpoints
* Docker support

---

## 💡 Notes

* External Identities pattern is used (Customer, Product, Branch)
* Business rules are enforced in the domain layer
* The API uses MediatR to decouple application logic
