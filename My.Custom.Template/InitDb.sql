-- Create Database (if not already existing)
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'MyDatabase')
    BEGIN
        CREATE DATABASE {MyDb};
    END
GO

USE {MyDb};
GO

-- Create Examples Table
CREATE TABLE Example (
                       Id INT IDENTITY(1,1) PRIMARY KEY,
                       Name NVARCHAR(MAX) NOT NULL
);
