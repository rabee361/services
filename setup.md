# 🚀 Project Setup Guide

This guide provides the steps required to build and run the **GLOW SHOP** microservices project locally and in production using Docker.

---

## 📋 Prerequisites

Before you start, ensure you have the following installed:

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (Windows/Mac) or [Docker Engine](https://docs.docker.com/engine/install/) (Linux)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (optional, for local development without Docker)
- [Git](https://git-scm.com/)

---

## ⚡ Quick Start (Docker Compose)

The easiest way to run the entire project is using Docker Compose. This starts all databases, RabbitMQ, the API Gateway, all backend services, and both frontend portals.

### 1. Clone the repository

```bash
git clone <repository-url>
cd micro-services
```

### 2. Consolidate and Initialize MySQL

The project is configured to use a single MySQL container (`mysql-db`) for all services.

- Make sure `init.sql` exists in the root folder.
- This script will automatically create `usersdb`, `productdb`, `inventorydb`, and `orderdb`.

### 3. Build and Run

```bash
docker-compose up --build
```

### 4. Verify Services

Once the containers are running, you can access:

- **Customer Storefront**: [http://localhost:5174](http://localhost:5174) 🛍️
- **Admin Dashboard**: [http://localhost:5257](http://localhost:5257) 👨‍💼
- **API Gateway**: [http://localhost:5000](http://localhost:5000) 🛰️
- **RabbitMQ Management**: [http://localhost:15672](http://localhost:15672) (User: `guest` | Pass: `guest`) 🐰

---

## 🛠️ Production Setup (Nginx)

To run the project in production behind an Nginx reverse proxy:

### 1. Update Nginx Configuration

Use the provided `nginx.conf` and update the `server_name` to your domain.

```nginx
server {
    listen 80;
    server_name yourdomain.com;

    # Root redirects to Ecommerce Web
    location / {
        proxy_pass http://localhost:5174;
        # ... other headers
    }

    # Admin subpath
    location /admin/ {
        proxy_pass http://localhost:5257;
        # ... other headers
    }
}
```

### 2. Update ASP.NET PathBase

Ensure `Ecommerce.Admin/Program.cs` includes `app.UsePathBase("/admin")` to handle the subpath correctly.

### 3. SSL (Certbot)

If using HTTPS, run Certbot to generate certificates:

```bash
sudo certbot --nginx -d yourdomain.com
```

---

## 🧪 Local Development (Without Docker)

If you wish to run a specific service for debugging:

1.  **Start Dependencies**: You still need MySQL and RabbitMQ running (standard ports).
2.  **Update Connection Strings**: Update `appsettings.json` in the respective service folder to point to `localhost`.
3.  **Run Service**:
    ```bash
    dotnet run --project src/Users.Api/Users.Api.csproj
    ```

---

## 🔑 Test Credentials

- **Admin**: `admin` / `admin`
- **Manager**: `manager` / `manager`
- **Customer**: `client` / `client` (or register a new one)
