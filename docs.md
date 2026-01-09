# 🏗️ E-Commerce Microservices Architecture Documentation

Welcome to the documentation for the **GLOW SHOP** ecosystem! This system is a distributed microservices architecture built with **.NET 8**, **MySQL**, **RabbitMQ**, and **Ocelot**, featuring a premium customer storefront and a robust admin dashboard.

---

## 🛰️ System Overview

The architecture consists of 6 main components:

### 🌐 Web Applications

1.  **Ecommerce.Web (Port 5174)**: The customer-facing storefront. Allows users to browse products, sign up, log in, and shop.
2.  **Ecommerce.Admin (Port 5257)**: The management portal. Features role-based access for Admins and Managers to control inventory, users, and products.

### ⚙️ Microservices (Backend)

1.  **Users.Api (Port 5260)**: Manages authentication, user profiles, and roles (Client, Manager, Admin).
2.  **Product.Api (Port 5025)**: Handles the product catalog and categories.
3.  **Inventory.Api (Port 5098)**: Manages multiple warehouses and stock levels.
4.  **Order.Api (Port 5053)**: Orchestrates order placement and history.
5.  **ApiGateway (Port 5000)**: Ocelot gateway that routes all external requests to the correct internal service.

---

## 👤 Authentication & Roles

The system uses a session-based authentication layer that integrates with the **Users.Api**.

| Role        | Access Level | Description                                                                                     |
| :---------- | :----------- | :---------------------------------------------------------------------------------------------- |
| **Client**  | Storefront   | Can shop, view history, and manage their profile in `Ecommerce.Web`.                            |
| **Manager** | Admin Portal | Access to `Ecommerce.Admin`. Restricted to managing stocks for their assigned warehouse only.   |
| **Admin**   | Full Master  | Access to `Ecommerce.Admin`. Full control over Users, Products, Categories, and all Warehouses. |

---

## 📡 API Endpoints

### 1. Users Service (`/users/api/...`)

- `POST /login`: Authenticates a user and returns their profile + role.
- `POST /register`: Creates a new Client account.
- `GET /`: Returns all users (Admin only).
- `PUT /{id}`: Updates user details/roles.

### 2. Product Service (`/products/api/...`)

- `GET /products`: List all products.
- `POST /products`: Add new product (Admin only).
- `GET /categories`: List all available product categories.

### 3. Inventory Service (`/inventory/api/...`)

- `GET /inventory`: List all stock levels across warehouses.
- `GET /inventory/warehouse/{id}`: Filter stock by warehouse (Manager use-case).
- `GET /warehouses`: List all warehouses.
- `GET /warehouses/manager/{id}`: Find which warehouse a manager is assigned to.

### 4. Order Service (`/orders/api/...`)

- `POST /orders`: Places an order (Triggers async stock reduction via RabbitMQ).
- `GET /orders`: View order history.

---

## 🔄 Integration Logic

### Synchronous (REST)

Web applications communicate with services via the **ApiGateway** or directly in development.

- **Example**: `Ecommerce.Admin` calls `Users.Api` to verify a login request.

### Asynchronous (Event-Driven)

We use **RabbitMQ** with **MassTransit** for cross-service consistency:

1.  **Order Placed**: `Order.Api` publishes `OrderCreatedEvent`.
2.  **Stock Reduced**: `Inventory.Api` consumes the event and updates the stock for the relevant warehouse automatically.

---

## 🚀 How to Run the System (Master Command)

You can now start the **entire ecosystem** (all 4 APIs, the Gateway, and both Web Portals) with a single command!

### Prerequisites

- Docker & Docker Compose installed.

### The "One Command" Start

From the project root, simply run:

```bash
docker-compose up --build
```

This will:

1.  Spin up all **MySQL** databases and **RabbitMQ**.
2.  Build and start all 4 **Microservices**.
3.  Build and start the **Ocelot Gateway**.
4.  Build and start the **Customer Storefront** (`Ecommerce.Web`) and **Admin Portal** (`Ecommerce.Admin`).

### 🔗 Access Points

Once everything is up, you can access the system at:

- **Customer Storefront**: [http://localhost:5174](http://localhost:5174) 🛒
- **Admin Portal**: [http://localhost:5257](http://localhost:5257) 🛠️
- **API Gateway**: [http://localhost:5000](http://localhost:5000) 🛰️
- **RabbitMQ Management**: [http://localhost:15672](http://localhost:15672) (guest/guest) 🐰

### 🔑 Test Accounts

- **Master Admin**: User: `admin` | Pass: `admin`
- **Warehouse Manager**: User: `manager` | Pass: `manager`

---

## 🛠️ Technology Stack

- **Backend**: .NET 8, ASP.NET Core Web API
- **Frontend**: ASP.NET Core MVC (Inter font, Glassmorphism CSS)
- **Database**: MySQL (Per-service database pattern)
- **Messaging**: RabbitMQ + MassTransit
- **Gateway**: Ocelot
- **Hosting**: Docker & Docker Compose
