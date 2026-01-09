INSERT INTO Warehouses (Id, Name, Location, ManagerId)
SELECT 1, 'Main Warehouse', 'Downtown', 2
WHERE NOT EXISTS (SELECT 1 FROM Warehouses WHERE Id = 1);

INSERT INTO Warehouses (Id, Name, Location, ManagerId)
SELECT 2, 'North Branch', 'Uptown', 2
WHERE NOT EXISTS (SELECT 1 FROM Warehouses WHERE Id = 2);

INSERT INTO Stocks (Id, ProductId, WarehouseId, Quantity)
SELECT 1, 1, 1, 50
WHERE NOT EXISTS (SELECT 1 FROM Stocks WHERE Id = 1);

INSERT INTO Stocks (Id, ProductId, WarehouseId, Quantity)
SELECT 2, 2, 1, 20
WHERE NOT EXISTS (SELECT 1 FROM Stocks WHERE Id = 2);

INSERT INTO Stocks (Id, ProductId, WarehouseId, Quantity)
SELECT 3, 3, 2, 100
WHERE NOT EXISTS (SELECT 1 FROM Stocks WHERE Id = 3);
