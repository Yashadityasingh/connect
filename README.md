# 📝 Todo API — ASP.NET Core Backend

A secure, production-ready RESTful Todo API built with **ASP.NET Core Web API**, featuring JWT authentication, middleware, rate limiting, activity logging, and full CRUD operations.

---

## 🚀 Hosted Backend

🔗 **Live API URL:**
https://connect-2.onrender.com

🔗 **Swagger Documentation:**
https://connect-2.onrender.com/swagger

---

## 📌 Features

### 🔐 Authentication & Authorization

* JWT-based authentication
* Secure login endpoint
* Protected routes using `[Authorize]`
* User-specific task management

### 📋 Task Management (CRUD)

Authenticated users can:

* ➕ Create tasks
* 📄 View their tasks
* ✏️ Update tasks
* ❌ Delete tasks

Each task belongs only to the logged-in user.

---

## 🧩 Middleware Implementation

The application uses custom and built-in middleware to handle cross-cutting concerns:

* ✅ Authentication Middleware (JWT)
* ✅ Authorization Enforcement
* ✅ Input Validation (Data Annotations)
* ✅ Rate Limiting
* ✅ Global Error Handling Middleware

---

## ⏱️ Rate Limiting

To prevent abuse and ensure stability:

* Strict limits on authentication endpoints
* Throttling applied to general API endpoints
* Implemented using ASP.NET Core Rate Limiting

---

## 📊 Activity Logging

User and system activities are stored in the database for auditing:

### Security Events

* Login attempts (success/failure)
* Authentication activity

### Task Operations

* Task creation
* Task updates
* Task deletions

### API Usage

* General endpoint access patterns

---

## 🏗️ Technology Stack

* ASP.NET Core Web API (.NET 8)
* Entity Framework Core
* JWT Authentication
* SQL Database
* Docker (for deployment)
* Swagger / OpenAPI

---

## 📂 Project Structure

```
TodoApi/
│
├── Controllers/
├── Models/
├── DTOs/
├── Middleware/
├── Data/
├── Services/
├── Dockerfile
├── Program.cs
└── appsettings.json
```

---

## 🛠️ Running Locally

### 1️⃣ Clone Repository

```
git clone https://github.com/Yashadityasingh/connect.git
cd connect
```

---

### 2️⃣ Restore Dependencies

```
dotnet restore
```

---

### 3️⃣ Configure Environment Variables

Create an `.env` file or use system variables.

Example:

```
JWT_SECRET=YourSuperSecretKey
DB_CONNECTION=YourDatabaseConnectionString
```

---

### 4️⃣ Run Application

```
dotnet run
```

The API will start at:

```
https://localhost:5001
```

Swagger:

```
https://localhost:5001/swagger
```

---

## 🐳 Running with Docker

### Build Image

```
docker build -t todo-api .
```

### Run Container

```
docker run -p 8080:8080 todo-api
```

Access API:

```
http://localhost:8080
```

---

## 📖 API Documentation

Interactive documentation available via Swagger UI:

* View endpoints
* Test requests directly
* Understand request/response schemas

---

## 🧠 Design Decisions & Architecture

* RESTful API design principles
* Layered architecture for separation of concerns
* Entity Framework Core for data access
* DTOs for request/response handling
* Middleware pipeline for centralized handling of errors and security
* User-based task ownership model
* Dockerized for portability and cloud deployment

---

## ⚠️ Assumptions

* Each user manages only their own tasks
* Authentication is required for all task operations
* Database is pre-configured and accessible
* Environment variables are properly set

---

## 📬 Deliverables

✔ GitHub Repository
✔ Hosted Backend URL
✔ Swagger Documentation
✔ README with setup instructions

---

## 👨‍💻 Author

**Yashaditya Singh**

