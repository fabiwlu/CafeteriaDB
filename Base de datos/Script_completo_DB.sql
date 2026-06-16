USE CafeteriaDB;
GO

CREATE TABLE Usuario (
    ID INT PRIMARY KEY IDENTITY(1,1),
    CI VARCHAR(15) UNIQUE NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Activo CHAR(1) NOT NULL CHECK (Activo IN ('S', 'N')),
    Nivel VARCHAR(20) NOT NULL CHECK (Nivel IN ('Admin', 'Usuario')),
    Contraseña VARCHAR(255) NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

CREATE TABLE Cliente (
    ID INT PRIMARY KEY IDENTITY(1,1),
    CI VARCHAR(15) UNIQUE NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Teléfono VARCHAR(15),
    Correo VARCHAR(100),
    Dirección VARCHAR(200)
);

CREATE TABLE Proveedor (
    ID INT PRIMARY KEY IDENTITY(1,1),
    CIoRUC VARCHAR(20) UNIQUE NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Teléfono VARCHAR(15),
    Correo VARCHAR(100),
    Dirección VARCHAR(200),
    Ciudad VARCHAR(50)
);

CREATE TABLE Producto (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Código VARCHAR(20) UNIQUE NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Descripción VARCHAR(300),
    PrecioVenta DECIMAL(10, 2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    Categoría VARCHAR(50)
);

CREATE TABLE Empleado (
    ID INT PRIMARY KEY IDENTITY(1,1),
    CI VARCHAR(15) UNIQUE NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Teléfono VARCHAR(15),
    Puesto VARCHAR(50),
    Salario DECIMAL(10, 2),
    FechaIngreso DATE
);

CREATE TABLE Compra (
    ID INT PRIMARY KEY IDENTITY(1,1),
    NumeroCompra VARCHAR(20) UNIQUE NOT NULL,
    IDProveedor INT NOT NULL,
    Fecha DATE NOT NULL DEFAULT GETDATE(),
    Subtotal DECIMAL(12, 2) NOT NULL,
    IVA DECIMAL(12, 2) DEFAULT 0,
    Total DECIMAL(12, 2) NOT NULL,
    Estado VARCHAR(20) NOT NULL CHECK (Estado IN ('Pendiente', 'Recibida', 'Cancelada')),
    Observaciones VARCHAR(300),
    FOREIGN KEY (IDProveedor) REFERENCES Proveedor(ID)
);

CREATE TABLE Detalle_Compra (
    ID INT PRIMARY KEY IDENTITY(1,1),
    IDCompra INT NOT NULL,
    IDProducto INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10, 2) NOT NULL,
    Subtotal DECIMAL(12, 2) NOT NULL,
    FOREIGN KEY (IDCompra) REFERENCES Compra(ID),
    FOREIGN KEY (IDProducto) REFERENCES Producto(ID)
);

CREATE TABLE Venta (
    ID INT PRIMARY KEY IDENTITY(1,1),
    NumeroFactura VARCHAR(20) UNIQUE NOT NULL,
    IDCliente INT NOT NULL,
    IDEmpleado INT NOT NULL,
    Fecha DATE NOT NULL DEFAULT GETDATE(),
    Hora TIME NOT NULL DEFAULT CAST(GETDATE() AS TIME),
    Subtotal DECIMAL(12, 2) NOT NULL,
    Descuento DECIMAL(12, 2) DEFAULT 0,
    IVA DECIMAL(12, 2) DEFAULT 0,
    Total DECIMAL(12, 2) NOT NULL,
    MetodoPago VARCHAR(20) NOT NULL CHECK (MetodoPago IN ('Efectivo', 'Tarjeta', 'QR')),
    Observaciones VARCHAR(300),
    FOREIGN KEY (IDCliente) REFERENCES Cliente(ID),
    FOREIGN KEY (IDEmpleado) REFERENCES Empleado(ID)
);

CREATE TABLE Detalle_Venta (
    ID INT PRIMARY KEY IDENTITY(1,1),
    IDVenta INT NOT NULL,
    IDProducto INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10, 2) NOT NULL,
    Subtotal DECIMAL(12, 2) NOT NULL,
    FOREIGN KEY (IDVenta) REFERENCES Venta(ID),
    FOREIGN KEY (IDProducto) REFERENCES Producto(ID)
);

CREATE INDEX idx_Compra_IDProveedor ON Compra(IDProveedor);
CREATE INDEX idx_Compra_Fecha ON Compra(Fecha);
CREATE INDEX idx_Venta_IDCliente ON Venta(IDCliente);
CREATE INDEX idx_Venta_IDEmpleado ON Venta(IDEmpleado);
CREATE INDEX idx_Venta_Fecha ON Venta(Fecha);
CREATE INDEX idx_Detalle_Compra_IDCompra ON Detalle_Compra(IDCompra);
CREATE INDEX idx_Detalle_Compra_IDProducto ON Detalle_Compra(IDProducto);
CREATE INDEX idx_Detalle_Venta_IDVenta ON Detalle_Venta(IDVenta);
CREATE INDEX idx_Detalle_Venta_IDProducto ON Detalle_Venta(IDProducto);