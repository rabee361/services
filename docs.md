# 🏗️ E-Commerce Microservices Architecture Documentation

Welcome to the internal technical documentation for the **GLOW SHOP** ecosystem.

---

## 📁 Source Code Structure (`/src`)

The root `/src` directory contains all business logic, divided into specialized microservices and web interfaces.

### 🛰️ 1. ApiGateway

The entry point for all external traffic. It handles request routing, load balancing, and authentication abstraction.

- **`ocelot.json`**: The core configuration file. Defines "Upstream" paths (what the user sees) and "Downstream" paths (internal service locations).
- **`Program.cs`**: Initializes the Ocelot middleware and integrates it into the ASP.NET pipeline.
- **`appsettings.json`**: Contains basic hosting settings.
- **`Dockerfile`**: Containerization instructions for the gateway.

### 🛍️ 2. Ecommerce.Web (Customer Storefront)

An ASP.NET Core MVC application providing the main shopping experience.

- **`Controllers/HomeController.cs`**: Manages the product gallery, cart logic, and checkout flows.
- **`Controllers/AccountController.cs`**: Handles user registration, login, and session management.
- **`Services/ProductService.cs`**: An internal client that communicates with the `Product.Api` via the Gateway.
- **`Models/`**: Contains ViewModels for mapping API data to the UI (e.g., `ProductViewModel`, `CartViewModel`).
- **`Views/`**: UI templates using Razor. Includes `Home/Index` (Gallery), `Home/Cart`, and `Shared/_Layout`.
- **`wwwroot/`**: Static assets including custom CSS for the glassmorphism design.

### 👨‍� 3. Ecommerce.Admin (Management Portal)

A restricted portal for Admins and Warehouse Managers.

- **`Program.cs`**: Configured with `UsePathBase("/admin")` to run behind a subpath in production.
- **`Controllers/BaseAdminController.cs`**: A base class that ensures only users with `Admin` or `Manager` roles can access the controllers.
- **`Controllers/UsersController.cs`**: CRUD operations for managing system users and their roles.
- **`Controllers/ProductsController.cs`**: Allows Admins to add/edit products and categories.
- **`Controllers/InventoryController.cs`**: Allows Managers to view and update stock levels for their assigned warehouses.
- **`Views/`**: Specialized layouts for the dashboard sidebar and management tables.

### � 4. Users.Api

Handles user identity, roles, and authentication.

- **`Controllers/UsersController.cs`**: Provides endpoints for `/login`, `/register`, and user management.
- **`Data/UsersDbContext.cs`**: Entity Framework context for the `usersdb`.
- **`Program.cs`**: Sets up the MySQL connection and dependency injection.

### 📦 5. Product.Api

The "Source of Truth" for the product catalog.

- **`Controllers/ProductsController.cs`**: Manages the product list and details.
- **`Controllers/CategoriesController.cs`**: Handles product categories.
- **`Data/ProductDbContext.cs`**: Entity Framework context for the `productdb`.

### 🏭 6. Inventory.Api

Manages warehouses and multi-location stock levels.

- **`Controllers/WarehousesController.cs`**: Manages warehouse entities and manager assignments.
- **`Controllers/InventoryController.cs`**: Manages stock counts.
- **`Consumers/OrderCreatedConsumer.cs`**: **CRITICAL**: Listens to RabbitMQ messages. When an order is placed, this file automatically reduces stock in the database.

### 🧾 7. Order.Api

Orchestrates the order lifecycle.

- **`Controllers/OrdersController.cs`**: Handles order placement.
- **`Program.cs`**: Configures **MassTransit** to publish events to RabbitMQ when a new order is saved.

### 🤝 8. Shared.Contracts

A library shared across all microservices to ensure consistency in communication.

- **`Events.cs`**: Defines the `OrderCreatedEvent` message structure used by RabbitMQ.

---

## 🔄 System Workflow (Example: Placing an Order)

1.  **User** clicks "Checkout" in `Ecommerce.Web`.
2.  `Ecommerce.Web` sends a request to `Order.Api`.
3.  `Order.Api` saves the order to its database and publishes an **`OrderCreatedEvent`** to **RabbitMQ**.
4.  **`Inventory.Api`** (the Consumer) picks up the message instantly.
5.  `Inventory.Api` finds the correct warehouse and reduces the stock count.
6.  The UI is updated, and the user sees their order history reflect the change.

---

## �️ Database Strategy

We use a **Database-per-Service** pattern consolidated into a single MySQL container (`mysql-db`) for resource efficiency. Each service only has access to its own database schema.
