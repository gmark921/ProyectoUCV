-- Crear la base de datos si no existe
CREATE DATABASE DBSISVENTAS;
GO

-- Usar la base de datos recién creada
USE DBSISVENTAS;
GO

-- ------------------------------------------------------------------------------------
-- Tabla: ROL -- Propósito: Almacena los diferentes perfiles de usuario (Ej: Administrador, Empleado).
-- ------------------------------------------------------------------------------------
CREATE TABLE ROL(
    Id VARCHAR(50) PRIMARY KEY,
    Descripcion VARCHAR(50)
);
GO

-- ------------------------------------------------------------------------------------
-- Tabla: USUARIO -- Propósito: Contiene la información de las personas que pueden iniciar sesión.
-- ------------------------------------------------------------------------------------
CREATE TABLE USUARIO(
    Id VARCHAR(50) PRIMARY KEY,
    Documento VARCHAR(50),
    NombreCompleto VARCHAR(100),
    Correo VARCHAR(100),
    Clave VARCHAR(150),
    IdRol VARCHAR(50) REFERENCES ROL(Id),
    Estado BIT
);
GO
CREATE UNIQUE INDEX UQ_Usuario_Documento ON USUARIO(Documento);
GO

-- ------------------------------------------------------------------------------------
-- Tabla: PERMISO -- Propósito: Vincula un ROL a los menús que puede ver en la interfaz.
-- ------------------------------------------------------------------------------------
CREATE TABLE PERMISO(
    Id VARCHAR(50) PRIMARY KEY,
    IdRol VARCHAR(50) REFERENCES ROL(Id),
    NombreMenu VARCHAR(100)
);
GO

-- ------------------------------------------------------------------------------------
-- Tabla: CATEGORIA -- Propósito: Clasifica los productos en grupos.
-- ------------------------------------------------------------------------------------
CREATE TABLE CATEGORIA(
    Id VARCHAR(50) PRIMARY KEY,
    Descripcion VARCHAR(100),
    Estado BIT
);
GO

-- ------------------------------------------------------------------------------------
-- Tabla: PRODUCTO -- Propósito: El catálogo de todos los productos que maneja el negocio.
-- ------------------------------------------------------------------------------------
CREATE TABLE PRODUCTO(
    Id VARCHAR(50) PRIMARY KEY,
    Codigo VARCHAR(50),
    Nombre VARCHAR(100),
    Descripcion VARCHAR(150),
    IdCategoria VARCHAR(50) REFERENCES CATEGORIA(Id),
    Stock DECIMAL(10,2) NOT NULL DEFAULT 0,
    UnidadBase VARCHAR(10), -- Ej: 'Kg', 'Toneladas', 'Unidades'
    PrecioCompra DECIMAL(10, 2) DEFAULT 0,
    PrecioVenta DECIMAL(10, 2) DEFAULT 0,
    Estado BIT
);
GO
CREATE UNIQUE INDEX UQ_Producto_Codigo ON PRODUCTO(Codigo);
GO

-- ------------------------------------------------------------------------------------
-- Tabla: CLIENTE -- Propósito: Almacena la información de los clientes.
-- ------------------------------------------------------------------------------------
CREATE TABLE CLIENTE(
    Id VARCHAR(50) PRIMARY KEY,
    Documento VARCHAR(50),
    NombreCompleto VARCHAR(100),
    Correo VARCHAR(100),
    Telefono VARCHAR(50),
    Estado BIT
);
GO
CREATE UNIQUE INDEX UQ_Cliente_Documento ON CLIENTE(Documento);
GO

-- ------------------------------------------------------------------------------------
-- Tabla: PROVEEDOR -- Propósito: Almacena la información de los proveedores.
-- ------------------------------------------------------------------------------------
CREATE TABLE PROVEEDOR(
    Id VARCHAR(50) PRIMARY KEY,
    RazonSocial VARCHAR(100),
    NombreComercial VARCHAR(100),
    Correo VARCHAR(100),
    Telefono VARCHAR(50),
    Estado BIT
);
GO
CREATE UNIQUE INDEX UQ_Proveedor_RazonSocial ON PROVEEDOR(RazonSocial);
GO

-- ------------------------------------------------------------------------------------
-- Tabla: COMPRA y DETALLE_COMPRA -- Propósito: Registran las operaciones de compra a proveedores.
-- ------------------------------------------------------------------------------------
CREATE TABLE COMPRA(
    Id VARCHAR(50) PRIMARY KEY,
    IdUsuario VARCHAR(50) REFERENCES USUARIO(Id),
    IdProveedor VARCHAR(50) REFERENCES PROVEEDOR(Id),
    TipoDocumento VARCHAR(50),
    NumeroDocumento VARCHAR(50) NULL,
    MontoTotal DECIMAL(10,2),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Estado BIT NOT NULL DEFAULT 1 -- Columna añadida aquí

);
GO

CREATE TABLE DETALLE_COMPRA(
    Id VARCHAR(50) PRIMARY KEY,
    IdCompra VARCHAR(50) REFERENCES COMPRA(Id),
    IdProducto VARCHAR(50) REFERENCES PRODUCTO(Id),
    PrecioCompra DECIMAL(10, 2) DEFAULT 0,
    CantidadComprada DECIMAL(10,2),
    MontoTotal DECIMAL(10,2)
);
GO

-- ------------------------------------------------------------------------------------
-- Tabla: VENTA y DETALLE_VENTA -- Propósito: Registran las operaciones de venta a clientes.
-- ------------------------------------------------------------------------------------
CREATE TABLE VENTA(
    Id VARCHAR(50) PRIMARY KEY,
    IdUsuario VARCHAR(50) REFERENCES USUARIO(Id),
    IdCliente VARCHAR(50) REFERENCES CLIENTE(Id) NULL,
    TipoDocumento VARCHAR(50),
    NumeroDocumento VARCHAR(50) NULL,
    MontoPago DECIMAL(10, 2),
    MontoCambio DECIMAL(10, 2),
    MontoTotal DECIMAL(10,2),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Estado BIT NOT NULL DEFAULT 1 -- Columna añadida aquí

);
GO

CREATE TABLE DETALLE_VENTA(
    Id VARCHAR(50) PRIMARY KEY,
    IdVenta VARCHAR(50) REFERENCES VENTA(Id),
    IdProducto VARCHAR(50) REFERENCES PRODUCTO(Id),
    PrecioVenta DECIMAL(10, 2),
    CantidadVendida DECIMAL(10,2),
    SubTotal DECIMAL(10,2)
);
GO


-- PROPÓSITO: Define la estructura para enviar una lista de productos en una compra.
-- ------------------------------------------------------------------------------------
CREATE TYPE udtDetalleCompra AS TABLE(
    IdProducto VARCHAR(50),
    PrecioCompra DECIMAL(10,2),
    CantidadComprada DECIMAL(10,2),
    UnidadBase VARCHAR(10)
);
GO

-- PROPÓSITO: Define la estructura para enviar una lista de productos en una venta.
-- ------------------------------------------------------------------------------------
CREATE TYPE udtDetalleVenta AS TABLE(
    IdProducto VARCHAR(50),
    PrecioVenta DECIMAL(10,2),
    CantidadVendida DECIMAL(10,2)
);
GO

-- PROPÓSITO: Define la estructura para enviar una lista de permisos para un rol.
-- ------------------------------------------------------------------------------------
CREATE TYPE udtPermiso AS TABLE(
    NombreMenu VARCHAR(100)
);
GO
