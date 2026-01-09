INSERT INTO Users (Id, Username, Email, Password, Role)
SELECT 1, 'admin', 'admin@glow.com', 'admin', 2
WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Id = 1);

INSERT INTO Users (Id, Username, Email, Password, Role)
SELECT 2, 'manager', 'manager@glow.com', 'manager', 1
WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Id = 2);

INSERT INTO Users (Id, Username, Email, Password, Role)
SELECT 3, 'john_doe', 'john@example.com', 'password123', 0
WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Id = 3);
