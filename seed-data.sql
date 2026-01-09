-- Seed Users (usersdb)
USE usersdb;
INSERT INTO Users (Id, Username, Email, Password, Role)
SELECT 1, 'admin', 'admin@glow.com', 'admin', 2
WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Id = 1);

INSERT INTO Users (Id, Username, Email, Password, Role)
SELECT 2, 'manager', 'manager@glow.com', 'manager', 1
WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Id = 2);

INSERT INTO Users (Id, Username, Email, Password, Role)
SELECT 3, 'john_doe', 'john@example.com', 'password123', 0
WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Id = 3);

-- Seed Categories (productdb)
USE productdb;
INSERT INTO Categories (Id, Name, Description, CreatedAt)
SELECT 1, 'Electronics', 'Electronic devices and gadgets', NOW()
WHERE NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 1);

INSERT INTO Categories (Id, Name, Description, CreatedAt)
SELECT 2, 'Home', 'Home and living products', NOW()
WHERE NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 2);

INSERT INTO Categories (Id, Name, Description, CreatedAt)
SELECT 3, 'Accessories', 'Fashion and lifestyle accessories', NOW()
WHERE NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 3);

-- Seed Products (productdb)
INSERT INTO Products (Id, Name, Price, CategoryId, ImageUrl)
SELECT 1, 'Smartphone X', 999.99, 1, ''
WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Id = 1);

INSERT INTO Products (Id, Name, Price, CategoryId, ImageUrl)
SELECT 2, 'Laptop Pro', 1499.50, 1, ''
WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Id = 2);

INSERT INTO Products (Id, Name, Price, CategoryId, ImageUrl)
SELECT 3, 'Coffee Maker', 79.00, 2, ''
WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Id = 3);

-- Seed Warehouses (inventorydb)
USE inventorydb;
INSERT INTO Warehouses (Id, Name, Location, ManagerId)
SELECT 1, 'Main Warehouse', 'Downtown', 2
WHERE NOT EXISTS (SELECT 1 FROM Warehouses WHERE Id = 1);

INSERT INTO Warehouses (Id, Name, Location, ManagerId)
SELECT 2, 'North Branch', 'Uptown', 2
WHERE NOT EXISTS (SELECT 1 FROM Warehouses WHERE Id = 2);

-- Seed Stocks (inventorydb)
INSERT INTO Stocks (Id, ProductId, WarehouseId, Quantity)
SELECT 1, 1, 1, 50
WHERE NOT EXISTS (SELECT 1 FROM Stocks WHERE Id = 1);

INSERT INTO Stocks (Id, ProductId, WarehouseId, Quantity)
SELECT 2, 2, 1, 20
WHERE NOT EXISTS (SELECT 1 FROM Stocks WHERE Id = 2);

INSERT INTO Stocks (Id, ProductId, WarehouseId, Quantity)
SELECT 3, 3, 2, 100
WHERE NOT EXISTS (SELECT 1 FROM Stocks WHERE Id = 3);

-- Seed Orders (orderdb)
USE orderdb;
-- Note: Order table uses GUID for ID based on the C# model.
-- We will insert a fixed GUID for testing idempotency.
INSERT INTO Orders (Id, ProductId, Quantity, UserId, Status)
SELECT '550e8400-e29b-41d4-a716-446655440000', 1, 2, '3', 'Completed'
WHERE NOT EXISTS (SELECT 1 FROM Orders WHERE Id = '550e8400-e29b-41d4-a716-446655440000');

INSERT INTO Orders (Id, ProductId, Quantity, UserId, Status)
SELECT '6712705b-801c-4235-8664-58a3683f124c', 2, 1, '3', 'Pending'
WHERE NOT EXISTS (SELECT 1 FROM Orders WHERE Id = '6712705b-801c-4235-8664-58a3683f124c');
