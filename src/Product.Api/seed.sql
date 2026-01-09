INSERT INTO Categories (Id, Name, Description, CreatedAt)
SELECT 1, 'Electronics', 'Electronic devices and gadgets', NOW()
WHERE NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 1);

INSERT INTO Categories (Id, Name, Description, CreatedAt)
SELECT 2, 'Home', 'Home and living products', NOW()
WHERE NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 2);

INSERT INTO Categories (Id, Name, Description, CreatedAt)
SELECT 3, 'Accessories', 'Fashion and lifestyle accessories', NOW()
WHERE NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 3);

INSERT INTO Products (Id, Name, Price, CategoryId, ImageUrl)
SELECT 1, 'Smartphone X', 999.99, 1, ''
WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Id = 1);

INSERT INTO Products (Id, Name, Price, CategoryId, ImageUrl)
SELECT 2, 'Laptop Pro', 1499.50, 1, ''
WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Id = 2);

INSERT INTO Products (Id, Name, Price, CategoryId, ImageUrl)
SELECT 3, 'Coffee Maker', 79.00, 2, ''
WHERE NOT EXISTS (SELECT 1 FROM Products WHERE Id = 3);
