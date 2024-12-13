create database Taller
go

use Taller
go

CREATE TABLE Cliente (
    ClienteID INT IDENTITY(1,1) PRIMARY KEY,     -- ID auto-incremental
    Nombre NVARCHAR(100) NOT NULL,               -- Nombre del cliente
    Apellido NVARCHAR(100) NOT NULL,             -- Apellido del cliente
    FechaNacimiento DATE,                        -- Fecha de nacimiento
    Email NVARCHAR(255),                         -- Email del cliente
    Telefono NVARCHAR(15),                       -- Teléfono del cliente
    Direccion NVARCHAR(255),                     -- Dirección del cliente
    FechaRegistro DATETIME DEFAULT GETDATE()     -- Fecha de registro (valor por defecto con la fecha y hora actuales)
);
go
CREATE PROCEDURE CrearCliente
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @FechaNacimiento DATE,
    @Email NVARCHAR(255),
    @Telefono NVARCHAR(15),
    @Direccion NVARCHAR(255)
AS
BEGIN
    INSERT INTO Cliente (Nombre, Apellido, FechaNacimiento, Email, Telefono, Direccion)
    VALUES (@Nombre, @Apellido, @FechaNacimiento, @Email, @Telefono, @Direccion);
END;

CREATE PROCEDURE LeerClientes
AS
BEGIN
    SELECT * FROM Cliente;
END;

CREATE PROCEDURE ActualizarCliente
    @ClienteID INT,
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @FechaNacimiento DATE,
    @Email NVARCHAR(255),
    @Telefono NVARCHAR(15),
    @Direccion NVARCHAR(255)
AS
BEGIN
    UPDATE Cliente
    SET 
        Nombre = @Nombre,
        Apellido = @Apellido,
        FechaNacimiento = @FechaNacimiento,
        Email = @Email,
        Telefono = @Telefono,
        Direccion = @Direccion
    WHERE ClienteID = @ClienteID;
END;

CREATE PROCEDURE EliminarCliente
    @ClienteID INT
AS
BEGIN
    DELETE FROM Cliente
    WHERE ClienteID = @ClienteID;
END;

CREATE PROCEDURE BuscarClientePorID
    @ClienteID INT
AS
BEGIN
    SELECT * FROM Cliente
    WHERE ClienteID = @ClienteID;
END;

CREATE PROCEDURE BuscarClientePorNombre
    @Nombre NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Cliente
    WHERE Nombre LIKE '%' + @Nombre + '%';
END;