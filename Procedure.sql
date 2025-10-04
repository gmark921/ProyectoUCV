USE DBSISVENTAS;
GO

-- ====================================================================================
-- SECCIÓN DE SEGURIDAD
-- ====================================================================================

---------- LOGIN ----------
CREATE PROCEDURE sp_LoginUsuario(
    @Documento VARCHAR(50)
)
AS
BEGIN
    SELECT u.Id, u.Documento, u.NombreCompleto, u.Correo, u.Clave, u.Estado, r.Id as IdRol, r.Descripcion as DescripcionRol
    FROM USUARIO u
    INNER JOIN ROL r ON r.Id = u.IdRol
    WHERE u.Documento = @Documento
END
GO

---------- PERMISOS ----------
CREATE PROCEDURE sp_ActualizarPermisos(
    @IdRol VARCHAR(50),
    @Permisos udtPermiso READONLY,
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        SET @Respuesta = 1
        SET @Mensaje = ''

        BEGIN TRANSACTION

        -- 1. Borrar permisos antiguos para el rol
        DELETE FROM PERMISO WHERE IdRol = @IdRol

        -- 2. Encontrar el último ID numérico existente en TODA la tabla de permisos
        DECLARE @ultimoId INT
        SELECT @ultimoId = ISNULL(MAX(CONVERT(INT, SUBSTRING(Id, 5, LEN(Id)))), 0)
        FROM PERMISO
        WHERE Id LIKE 'PERM%' AND ISNUMERIC(SUBSTRING(Id, 5, LEN(Id))) = 1;

        -- 3. Insertar los nuevos permisos con IDs correlativos
        -- Se usa ROW_NUMBER() para generar una secuencia y sumarla al último ID encontrado
        INSERT INTO PERMISO(Id, IdRol, NombreMenu)
        SELECT
            'PERM' + RIGHT('000' + CONVERT(VARCHAR, @ultimoId + ROW_NUMBER() OVER (ORDER BY (SELECT NULL))), 3),
            @IdRol,
            NombreMenu
        FROM @Permisos

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SET @Respuesta = 0
        SET @Mensaje = 'No se pudieron actualizar los permisos. Error: ' + ERROR_MESSAGE()
    END CATCH
END
GO

-- ----- CORRECCIÓN PARA sp_RegistrarRol -----
CREATE PROCEDURE sp_RegistrarRol(
    @Descripcion VARCHAR(50),
    @IdResultado VARCHAR(50) OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET @IdResultado = ''
    IF NOT EXISTS (SELECT * FROM ROL WHERE Descripcion = @Descripcion)
    BEGIN
        DECLARE @ultimoId INT
        -- Lógica a prueba de errores: Solo convierte los IDs que parecen ser numéricos.
        SELECT @ultimoId = ISNULL(MAX(CONVERT(INT, SUBSTRING(Id, 4, LEN(Id)))), 0) FROM ROL WHERE Id LIKE 'ROL%' AND ISNUMERIC(SUBSTRING(Id, 4, LEN(Id))) = 1;
        SET @ultimoId = @ultimoId + 1;
        SET @IdResultado = 'ROL' + RIGHT('00' + CONVERT(VARCHAR, @ultimoId), 2);

        INSERT INTO ROL(Id, Descripcion) VALUES (@IdResultado, @Descripcion)
        SET @Mensaje = ''
    END
    ELSE
        SET @Mensaje = 'No se puede registrar un rol con la misma descripción.'
END
GO

CREATE PROCEDURE sp_EditarRol(
    @IdRol VARCHAR(50),
    @Descripcion VARCHAR(50),
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET @Respuesta = 0
    IF NOT EXISTS (SELECT * FROM ROL WHERE Descripcion = @Descripcion AND Id != @IdRol)
    BEGIN
        UPDATE ROL SET Descripcion = @Descripcion WHERE Id = @IdRol
        SET @Respuesta = 1
        SET @Mensaje = ''
    END
    ELSE
        SET @Mensaje = 'Ya existe otro rol con la misma descripción.'
END
GO

CREATE PROCEDURE sp_EliminarRol(
    @IdRol VARCHAR(50),
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET @Respuesta = 0
    SET @Mensaje = ''
    -- Verificamos si el rol está siendo usado por algún usuario
    IF NOT EXISTS (SELECT * FROM USUARIO WHERE IdRol = @IdRol)
    BEGIN
        -- También eliminamos los permisos asociados a ese rol
        DELETE FROM PERMISO WHERE IdRol = @IdRol
        DELETE FROM ROL WHERE Id = @IdRol
        SET @Respuesta = 1
    END
    ELSE
        SET @Mensaje = 'El rol no se puede eliminar, ya que está asignado a uno o más usuarios.'
END
GO

-- ----- CORRECCIÓN PARA sp_RegistrarUsuario -----
CREATE PROCEDURE sp_RegistrarUsuario(
    @Documento VARCHAR(50),
    @NombreCompleto VARCHAR(100),
    @Correo VARCHAR(100),
    @Clave VARCHAR(150),
    @IdRol VARCHAR(50),
    @Estado BIT,
    @IdUsuarioResultado VARCHAR(50) OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET @IdUsuarioResultado = ''
    IF NOT EXISTS(SELECT * FROM USUARIO WHERE Documento = @Documento)
    BEGIN
        DECLARE @ultimoId INT
        -- Lógica a prueba de errores
        SELECT @ultimoId = ISNULL(MAX(CONVERT(INT, SUBSTRING(Id, 4, LEN(Id)))), 0) FROM USUARIO WHERE Id LIKE 'USR%' AND ISNUMERIC(SUBSTRING(Id, 4, LEN(Id))) = 1;
        SET @ultimoId = @ultimoId + 1
        SET @IdUsuarioResultado = 'USR' + RIGHT('000' + CONVERT(VARCHAR, @ultimoId), 3)

        INSERT INTO USUARIO(Id, Documento, NombreCompleto, Correo, Clave, IdRol, Estado)
        VALUES(@IdUsuarioResultado, @Documento, @NombreCompleto, @Correo, @Clave, @IdRol, @Estado)
        
        SET @Mensaje = ''
    END
    ELSE
        SET @Mensaje = 'No se puede repetir el documento para más de un usuario'
END
GO


CREATE PROCEDURE sp_EditarUsuario(
    @IdUsuario VARCHAR(50),
    @Documento VARCHAR(50),
    @NombreCompleto VARCHAR(100),
    @Correo VARCHAR(100),
    @Clave VARCHAR(150),
    @IdRol VARCHAR(50),
    @Estado BIT,
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET @Respuesta = 0
    IF NOT EXISTS(SELECT * FROM USUARIO WHERE Documento = @Documento AND Id != @IdUsuario)
    BEGIN
        -- AÑADIMOS LA ACTUALIZACIÓN DE LA CLAVE
        UPDATE USUARIO SET
            Documento = @Documento,
            NombreCompleto = @NombreCompleto,
            Correo = @Correo,
            Clave = @Clave, -- <-- CAMPO AÑADIDO
            IdRol = @IdRol,
            Estado = @Estado
        WHERE Id = @IdUsuario

        SET @Respuesta = 1
        SET @Mensaje = ''
    END
    ELSE
        SET @Mensaje = 'No se puede repetir el documento para más de un usuario'
END
GO

CREATE PROCEDURE sp_EliminarUsuario(@IdUsuario VARCHAR(50), @Respuesta BIT OUTPUT, @Mensaje VARCHAR(500) OUTPUT) AS BEGIN SET @Respuesta = 0 SET @Mensaje = '' IF NOT EXISTS (SELECT 1 FROM COMPRA c WHERE c.IdUsuario=@IdUsuario UNION SELECT 1 FROM VENTA v WHERE v.IdUsuario=@IdUsuario) BEGIN DELETE FROM USUARIO WHERE Id = @IdUsuario SET @Respuesta = 1 END ELSE SET @Mensaje = 'El usuario se encuentra relacionado a una compra o venta.' END
GO


-- ====================================================================================
-- SECCIÓN DE TIPOS DE TABLA -- Estos tipos permiten enviar listas de datos desde C# a SQL Server en un solo viaje.
-- ====================================================================================
CREATE TYPE udtDetalleCompra AS TABLE(
    IdProducto VARCHAR(50),
    PrecioCompra DECIMAL(10,2),
    CantidadComprada DECIMAL(10,2),
    UnidadBase VARCHAR(10)
);
GO

CREATE TYPE udtDetalleVenta AS TABLE(
    IdProducto VARCHAR(50),
    PrecioVenta DECIMAL(10,2),
    CantidadVendida DECIMAL(10,2)
);
GO

CREATE TYPE udtPermiso AS TABLE(
    NombreMenu VARCHAR(100)
);
GO

-- ====================================================================================
-- SECCIÓN DE PROCEDIMIENTOS ALMACENADOS -- NOTA: Estos procedimientos usan NEWID() para generar IDs para los registros
-- creados DESDE LA APLICACIÓN, garantizando su unicidad.
-- ====================================================================================


-- CRUD para CATEGORIA
-- ------------------------------------------------------------------------------------
CREATE PROCEDURE sp_RegistrarCategoria(
    @Descripcion VARCHAR(100),
    @Estado BIT,
    @IdResultado VARCHAR(50) OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET @IdResultado = ''
    IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion)
    BEGIN
        -- --- LÓGICA PARA GENERAR EL ID CORRELATIVO ---
        DECLARE @ultimoId INT
        -- Busca el número más alto entre los IDs que empiezan con 'CAT' y son numéricos.
        SELECT @ultimoId = ISNULL(MAX(CONVERT(INT, SUBSTRING(Id, 4, LEN(Id)))), 0) 
        FROM CATEGORIA 
        WHERE Id LIKE 'CAT%' AND ISNUMERIC(SUBSTRING(Id, 4, LEN(Id))) = 1;
        
        SET @ultimoId = @ultimoId + 1;
        SET @IdResultado = 'CAT' + RIGHT('00' + CONVERT(VARCHAR, @ultimoId), 2);
        -- --- FIN DE LA LÓGICA ---

        INSERT INTO CATEGORIA (Id, Descripcion, Estado) VALUES (@IdResultado, @Descripcion, @Estado)
        SET @Mensaje = ''
    END
    ELSE
        SET @Mensaje = 'No se puede registrar una categoría con la misma descripción.'
END
GO
CREATE PROCEDURE sp_EditarCategoria(@IdCategoria VARCHAR(50),@Descripcion VARCHAR(100),@Estado BIT,@Respuesta BIT OUTPUT,@Mensaje VARCHAR(500) OUTPUT) AS BEGIN SET @Respuesta = 0 IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion AND Id != @IdCategoria) BEGIN UPDATE CATEGORIA SET Descripcion = @Descripcion, Estado = @Estado WHERE Id = @IdCategoria SET @Respuesta = 1 SET @Mensaje = '' END ELSE SET @Mensaje = 'Ya existe otra categoría con la misma descripción.' END
GO
CREATE PROCEDURE sp_EliminarCategoria(@IdCategoria VARCHAR(50),@Respuesta BIT OUTPUT,@Mensaje VARCHAR(500) OUTPUT) AS BEGIN SET @Respuesta = 0 SET @Mensaje = '' IF NOT EXISTS (SELECT 1 FROM PRODUCTO p WHERE p.IdCategoria = @IdCategoria) BEGIN DELETE FROM CATEGORIA WHERE Id = @IdCategoria SET @Respuesta = 1 END ELSE SET @Mensaje = 'La categoría se encuentra relacionada a un producto.' END
GO

-- =======================================================================
-- ACTUALIZACIÓN DE PROCEDIMIENTOS DE PRODUCTO
-- =======================================================================

-- 1. NUEVO: Procedimiento para obtener el siguiente código de producto por categoría
CREATE PROCEDURE sp_ObtenerSiguienteCodigoProducto(
    @IdCategoria VARCHAR(50),
    @CodigoGenerado VARCHAR(50) OUTPUT
)
AS
BEGIN
    DECLARE @Prefijo VARCHAR(4);
    DECLARE @ultimoNumero INT;

    -- Obtenemos las 4 primeras letras de la descripción de la categoría
    SELECT @Prefijo = UPPER(SUBSTRING(Descripcion, 1, 4)) FROM CATEGORIA WHERE Id = @IdCategoria;

    -- Buscamos el último número para ese prefijo
    SELECT @ultimoNumero = ISNULL(MAX(CONVERT(INT, SUBSTRING(Codigo, 6, LEN(Codigo)))), 0)
    FROM PRODUCTO
    WHERE Codigo LIKE @Prefijo + '-%';

    SET @ultimoNumero = @ultimoNumero + 1;
    SET @CodigoGenerado = @Prefijo + '-' + RIGHT('000' + CONVERT(VARCHAR, @ultimoNumero), 3);
END
GO


-- 1. CORRECCIÓN A sp_RegistrarProducto
-- Le añadimos el parámetro @PrecioCompra y lo usamos en el INSERT.
CREATE PROCEDURE sp_RegistrarProducto(
    @Codigo VARCHAR(50),
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(150),
    @IdCategoria VARCHAR(50),
    @UnidadBase VARCHAR(10),
    @PrecioCompra DECIMAL(10,2), -- <--- PARÁMETRO AÑADIDO
    @PrecioVenta DECIMAL(10,2),
    @Estado BIT,
    @IdResultado VARCHAR(50) OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET @IdResultado = ''
    IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Codigo = @Codigo)
    BEGIN
        DECLARE @ultimoId INT;
        SELECT @ultimoId = ISNULL(MAX(CONVERT(INT, SUBSTRING(Id, 5, LEN(Id)))), 0) 
        FROM PRODUCTO 
        WHERE Id LIKE 'PROD%' AND ISNUMERIC(SUBSTRING(Id, 5, LEN(Id))) = 1;
        SET @ultimoId = @ultimoId + 1;
        SET @IdResultado = 'PROD' + RIGHT('000' + CONVERT(VARCHAR, @ultimoId), 3);

        INSERT INTO PRODUCTO (Id, Codigo, Nombre, Descripcion, IdCategoria, Stock, UnidadBase, PrecioCompra, PrecioVenta, Estado)
        VALUES (@IdResultado, @Codigo, @Nombre, @Descripcion, @IdCategoria, 0, @UnidadBase, @PrecioCompra, @PrecioVenta, @Estado) -- <-- CAMBIO AQUÍ

        SET @Mensaje = ''
    END
    ELSE
        SET @Mensaje = 'Ya existe un producto con el mismo código.'
END
GO


-- 2. CORRECCIÓN A sp_EditarProducto
-- Añadimos el parámetro @PrecioCompra y lo incluimos en la sentencia UPDATE.
CREATE PROCEDURE sp_EditarProducto(
    @IdProducto VARCHAR(50),
    @Codigo VARCHAR(50),
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(150),
    @IdCategoria VARCHAR(50),
    @UnidadBase VARCHAR(10),
    @PrecioCompra DECIMAL(10,2), -- <--- PARÁMETRO AÑADIDO
    @PrecioVenta DECIMAL(10,2),
    @Estado BIT,
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET @Respuesta = 0
    IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Codigo = @Codigo AND Id != @IdProducto)
    BEGIN
        UPDATE PRODUCTO SET
            Codigo = @Codigo,
            Nombre = @Nombre,
            Descripcion = @Descripcion,
            IdCategoria = @IdCategoria,
            UnidadBase = @UnidadBase,
            PrecioCompra = @PrecioCompra, -- <-- LÍNEA AÑADIDA
            PrecioVenta = @PrecioVenta,
            Estado = @Estado
        WHERE Id = @IdProducto

        SET @Respuesta = 1
        SET @Mensaje = ''
    END
    ELSE
        SET @Mensaje = 'Ya existe otro producto con el mismo código.'
END
GO

CREATE PROCEDURE sp_EliminarProducto(@IdProducto VARCHAR(50),@Respuesta BIT OUTPUT,@Mensaje VARCHAR(500) OUTPUT) AS BEGIN SET @Respuesta = 0 SET @Mensaje = '' IF NOT EXISTS (SELECT * FROM DETALLE_COMPRA dc WHERE dc.IdProducto = @IdProducto UNION SELECT * FROM DETALLE_VENTA dv WHERE dv.IdProducto = @IdProducto) BEGIN DELETE FROM PRODUCTO WHERE Id = @IdProducto SET @Respuesta = 1 END ELSE SET @Mensaje = 'El producto se encuentra relacionado a una compra o una venta.' END
GO


-- =======================================================================
-- ACTUALIZACIÓN DE PROCEDIMIENTOS DE REGISTRO
-- =======================================================================

-- ----- Actualización para sp_RegistrarCliente -----
CREATE PROCEDURE sp_RegistrarCliente(
    @Documento VARCHAR(50),
    @NombreCompleto VARCHAR(100),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(50),
    @Estado BIT,
    @IdResultado VARCHAR(50) OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
) 
AS 
BEGIN 
    SET @IdResultado = '' 
    IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Documento = @Documento) 
    BEGIN 
        -- --- LÓGICA PARA GENERAR EL ID CORRELATIVO ---
        DECLARE @ultimoId_CLI INT
        SELECT @ultimoId_CLI = ISNULL(MAX(CONVERT(INT, SUBSTRING(Id, 4, LEN(Id)))), 0) 
        FROM CLIENTE 
        WHERE Id LIKE 'CLI%' AND ISNUMERIC(SUBSTRING(Id, 4, LEN(Id))) = 1;
        
        SET @ultimoId_CLI = @ultimoId_CLI + 1;
        SET @IdResultado = 'CLI' + RIGHT('00' + CONVERT(VARCHAR, @ultimoId_CLI), 2);
        -- --- FIN DE LA LÓGICA ---

        INSERT INTO CLIENTE (Id, Documento, NombreCompleto, Correo, Telefono, Estado) 
        VALUES (@IdResultado, @Documento, @NombreCompleto, @Correo, @Telefono, @Estado) 
        SET @Mensaje = '' 
    END 
    ELSE 
        SET @Mensaje = 'Ya existe un cliente con el mismo número de documento.' 
END
GO


CREATE PROCEDURE sp_EditarCliente(@IdCliente VARCHAR(50),@Documento VARCHAR(50),@NombreCompleto VARCHAR(100),@Correo VARCHAR(100),@Telefono VARCHAR(50),@Estado BIT,@Respuesta BIT OUTPUT,@Mensaje VARCHAR(500) OUTPUT) AS BEGIN SET @Respuesta = 0 IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Documento = @Documento AND Id != @IdCliente) BEGIN UPDATE CLIENTE SET Documento = @Documento, NombreCompleto = @NombreCompleto, Correo = @Correo, Telefono = @Telefono, Estado = @Estado WHERE Id = @IdCliente SET @Respuesta = 1 SET @Mensaje = '' END ELSE SET @Mensaje = 'Ya existe otro cliente con el mismo número de documento.' END
GO
CREATE PROCEDURE sp_EliminarCliente(@IdCliente VARCHAR(50),@Respuesta BIT OUTPUT,@Mensaje VARCHAR(500) OUTPUT) AS BEGIN SET @Respuesta = 0 SET @Mensaje = '' IF NOT EXISTS (SELECT 1 FROM VENTA v WHERE v.IdCliente = @IdCliente) BEGIN DELETE FROM CLIENTE WHERE Id = @IdCliente SET @Respuesta = 1 END ELSE SET @Mensaje = 'El cliente se encuentra relacionado a una venta.' END
GO

-- ----- Actualización para sp_RegistrarProveedor -----
ALTER PROCEDURE sp_RegistrarProveedor(
    @RazonSocial VARCHAR(100),
    @NombreComercial VARCHAR(100),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(50),
    @Estado BIT,
    @IdResultado VARCHAR(50) OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
) 
AS 
BEGIN 
    SET @IdResultado = '' 
    IF NOT EXISTS (SELECT * FROM PROVEEDOR WHERE RazonSocial = @RazonSocial) 
    BEGIN 
        -- --- LÓGICA PARA GENERAR EL ID CORRELATIVO ---
        DECLARE @ultimoId_PROV INT
        SELECT @ultimoId_PROV = ISNULL(MAX(CONVERT(INT, SUBSTRING(Id, 5, LEN(Id)))), 0) 
        FROM PROVEEDOR 
        WHERE Id LIKE 'PROV%' AND ISNUMERIC(SUBSTRING(Id, 5, LEN(Id))) = 1;
        
        SET @ultimoId_PROV = @ultimoId_PROV + 1;
        SET @IdResultado = 'PROV' + RIGHT('00' + CONVERT(VARCHAR, @ultimoId_PROV), 2);
        -- --- FIN DE LA LÓGICA ---

        INSERT INTO PROVEEDOR (Id, RazonSocial, NombreComercial, Correo, Telefono, Estado) 
        VALUES (@IdResultado, @RazonSocial, @NombreComercial, @Correo, @Telefono, @Estado) 
        SET @Mensaje = '' 
    END 
    ELSE 
        SET @Mensaje = 'Ya existe un proveedor con la misma razón social.' 
END
GO
CREATE PROCEDURE sp_EditarProveedor(@IdProveedor VARCHAR(50),@RazonSocial VARCHAR(100),@NombreComercial VARCHAR(100),@Correo VARCHAR(100),@Telefono VARCHAR(50),@Estado BIT,@Respuesta BIT OUTPUT,@Mensaje VARCHAR(500) OUTPUT) AS BEGIN SET @Respuesta = 0 IF NOT EXISTS (SELECT * FROM PROVEEDOR WHERE RazonSocial = @RazonSocial AND Id != @IdProveedor) BEGIN UPDATE PROVEEDOR SET RazonSocial = @RazonSocial, NombreComercial = @NombreComercial, Correo = @Correo, Telefono = @Telefono, Estado = @Estado WHERE Id = @IdProveedor SET @Respuesta = 1 SET @Mensaje = '' END ELSE SET @Mensaje = 'Ya existe otro proveedor con la misma razón social.' END
GO
CREATE PROCEDURE sp_EliminarProveedor(@IdProveedor VARCHAR(50),@Respuesta BIT OUTPUT,@Mensaje VARCHAR(500) OUTPUT) AS BEGIN SET @Respuesta = 0 SET @Mensaje = '' IF NOT EXISTS (SELECT 1 FROM COMPRA c WHERE c.IdProveedor = @IdProveedor) BEGIN DELETE FROM PROVEEDOR WHERE Id = @IdProveedor SET @Respuesta = 1 END ELSE SET @Mensaje = 'El proveedor se encuentra relacionado a una compra.' END
GO


-- ----- CORRECCIÓN PARA sp_RegistrarCompra -----
CREATE PROCEDURE sp_RegistrarCompra(
    @IdUsuario VARCHAR(50),
    @IdProveedor VARCHAR(50),
    @TipoDocumento VARCHAR(50),
    @MontoTotal DECIMAL(10,2),
    @DetalleCompra udtDetalleCompra READONLY,
    @IdResultado VARCHAR(50) OUTPUT,
    @NumeroDocumentoResultado VARCHAR(50) OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        SET @IdResultado = ''
        SET @Mensaje = ''

        BEGIN TRANSACTION
            DECLARE @ultimoNumero INT
            -- Lógica a prueba de errores: Solo convierte los NumeroDocumento que son puramente numéricos.
            SELECT @ultimoNumero = ISNULL(MAX(CONVERT(INT, NumeroDocumento)), 0) FROM COMPRA WHERE ISNUMERIC(NumeroDocumento) = 1;
            SET @ultimoNumero = @ultimoNumero + 1
            SET @NumeroDocumentoResultado = RIGHT('0000000000' + CONVERT(VARCHAR, @ultimoNumero), 10)

            DECLARE @IdCompra VARCHAR(50) = NEWID()

            INSERT INTO COMPRA(Id, IdUsuario, IdProveedor, TipoDocumento, NumeroDocumento, MontoTotal)
            VALUES(@IdCompra, @IdUsuario, @IdProveedor, @TipoDocumento, @NumeroDocumentoResultado, @MontoTotal)

            INSERT INTO DETALLE_COMPRA(Id, IdCompra, IdProducto, PrecioCompra, CantidadComprada, MontoTotal)
            SELECT NEWID(), @IdCompra, IdProducto, PrecioCompra, CantidadComprada, PrecioCompra * CantidadComprada
            FROM @DetalleCompra

            UPDATE p
            SET
                p.Stock = p.Stock + dc.CantidadComprada,
                p.PrecioCompra = dc.PrecioCompra
            FROM PRODUCTO p
            INNER JOIN @DetalleCompra dc ON dc.IdProducto = p.Id

            SET @IdResultado = @IdCompra
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SET @IdResultado = ''
        SET @NumeroDocumentoResultado = ''
        SET @Mensaje = 'No se pudo registrar la compra. Error: ' + ERROR_MESSAGE()
    END CATCH
END
GO

-- ----- CORRECCIÓN PARA sp_RegistrarVenta -----
CREATE PROCEDURE sp_RegistrarVenta(
    @IdUsuario VARCHAR(50),
    @IdCliente VARCHAR(50),
    @TipoDocumento VARCHAR(50),
    @MontoPago DECIMAL(10,2),
    @MontoCambio DECIMAL(10,2),
    @MontoTotal DECIMAL(10,2),
    @DetalleVenta udtDetalleVenta READONLY,
    @IdResultado VARCHAR(50) OUTPUT,
    @NumeroDocumentoResultado VARCHAR(50) OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        SET @IdResultado = ''
        SET @Mensaje = ''

        IF EXISTS (SELECT 1 FROM PRODUCTO p INNER JOIN @DetalleVenta dv ON dv.IdProducto = p.Id WHERE p.Stock < dv.CantidadVendida)
        BEGIN
            SET @Mensaje = 'No hay stock suficiente para uno de los productos.'
        END
        ELSE
        BEGIN
            BEGIN TRANSACTION
                DECLARE @ultimoNumero INT
                -- Lógica a prueba de errores: Solo convierte los NumeroDocumento que son puramente numéricos.
                SELECT @ultimoNumero = ISNULL(MAX(CONVERT(INT, NumeroDocumento)), 0) FROM VENTA WHERE ISNUMERIC(NumeroDocumento) = 1;
                SET @ultimoNumero = @ultimoNumero + 1
                SET @NumeroDocumentoResultado = RIGHT('0000000000' + CONVERT(VARCHAR, @ultimoNumero), 10)

                DECLARE @IdVenta VARCHAR(50) = NEWID()

                INSERT INTO VENTA(Id, IdUsuario, IdCliente, TipoDocumento, NumeroDocumento, MontoPago, MontoCambio, MontoTotal)
                VALUES(@IdVenta, @IdUsuario, @IdCliente, @TipoDocumento, @NumeroDocumentoResultado, @MontoPago, @MontoCambio, @MontoTotal)

                INSERT INTO DETALLE_VENTA(Id, IdVenta, IdProducto, PrecioVenta, CantidadVendida, SubTotal)
                SELECT NEWID(), @IdVenta, IdProducto, PrecioVenta, CantidadVendida, PrecioVenta * CantidadVendida
                FROM @DetalleVenta

                UPDATE p
                SET p.Stock = p.Stock - dv.CantidadVendida
                FROM PRODUCTO p
                INNER JOIN @DetalleVenta dv ON dv.IdProducto = p.Id

                SET @IdResultado = @IdVenta
            COMMIT TRANSACTION
        END
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SET @IdResultado = ''
        SET @NumeroDocumentoResultado = ''
        SET @Mensaje = 'No se pudo registrar la venta. Error: ' + ERROR_MESSAGE()
    END CATCH
END
GO



-- =======================================================================
-- NUEVOS PROCEDIMIENTOS PARA ANULACIÓN
-- =======================================================================

-- ----- Procedimiento para Anular Venta -----
CREATE PROCEDURE sp_AnularVenta(
    @IdVenta VARCHAR(50),
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        SET @Respuesta = 0;
        SET @Mensaje = 'La venta no existe o ya ha sido anulada.';

        -- Verificar que la venta exista y no esté ya anulada
        IF EXISTS (SELECT 1 FROM VENTA WHERE Id = @IdVenta AND Estado = 1)
        BEGIN
            BEGIN TRANSACTION;

            -- 1. Devolver el stock de los productos vendidos
            UPDATE p
            SET p.Stock = p.Stock + dv.CantidadVendida
            FROM PRODUCTO p
            INNER JOIN DETALLE_VENTA dv ON p.Id = dv.IdProducto
            WHERE dv.IdVenta = @IdVenta;

            -- 2. Cambiar el estado de la venta a "Anulada"
            UPDATE VENTA
            SET Estado = 0 -- 0 para Anulada
            WHERE Id = @IdVenta;

            COMMIT TRANSACTION;

            SET @Respuesta = 1;
            SET @Mensaje = 'Venta anulada correctamente.';
        END

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @Respuesta = 0;
        SET @Mensaje = 'No se pudo anular la venta. Error: ' + ERROR_MESSAGE();
    END CATCH
END
GO


-- ----- Procedimiento para Anular Compra -----
CREATE PROCEDURE sp_AnularCompra(
    @IdCompra VARCHAR(50),
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        SET @Respuesta = 0;
        SET @Mensaje = 'La compra no existe o ya ha sido anulada.';

        -- Verificar que la compra exista y no esté ya anulada
        IF EXISTS (SELECT 1 FROM COMPRA WHERE Id = @IdCompra AND Estado = 1)
        BEGIN
            BEGIN TRANSACTION;

            -- 1. Restar el stock de los productos comprados
            UPDATE p
            SET p.Stock = p.Stock - dc.CantidadComprada
            FROM PRODUCTO p
            INNER JOIN DETALLE_COMPRA dc ON p.Id = dc.IdProducto
            WHERE dc.IdCompra = @IdCompra;

            -- 2. Cambiar el estado de la compra a "Anulada"
            UPDATE COMPRA
            SET Estado = 0 -- 0 para Anulada
            WHERE Id = @IdCompra;

            COMMIT TRANSACTION;

            SET @Respuesta = 1;
            SET @Mensaje = 'Compra anulada correctamente.';
        END

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @Respuesta = 0;
        SET @Mensaje = 'No se pudo anular la compra. Error: ' + ERROR_MESSAGE();
    END CATCH
END
GO

-- =======================================================================
-- ACTUALIZACIÓN DE REPORTES MAESTROS
-- =======================================================================

-- ----- Actualización para Reporte de Ventas -----
CREATE PROCEDURE sp_ReporteMaestroVentas(
    @FechaInicio VARCHAR(10),
    @FechaFin VARCHAR(10),
    @Busqueda VARCHAR(100)
)
AS
BEGIN
    SELECT
        v.Id,
        v.Estado, -- Devolvemos el nuevo campo Estado
        CONVERT(CHAR(10), v.FechaRegistro, 103) AS FechaRegistro,
        v.TipoDocumento,
        v.NumeroDocumento,
        v.MontoTotal,
        u.NombreCompleto AS UsuarioRegistro,
        ISNULL(c.Documento, 'N/A') AS DocumentoCliente,
        ISNULL(c.NombreCompleto, 'Cliente Varios') AS NombreCliente
    FROM VENTA v
    INNER JOIN USUARIO u ON u.Id = v.IdUsuario
    LEFT JOIN CLIENTE c ON c.Id = v.IdCliente
    WHERE
        -- Por defecto, solo mostramos las activas (Estado = 1)
        v.Estado = 1 AND
        ( (@FechaInicio = '' AND @FechaFin = '') OR (CONVERT(DATE, v.FechaRegistro) BETWEEN @FechaInicio AND @FechaFin) )
        AND (
            v.NumeroDocumento LIKE '%' + @Busqueda + '%' OR
            c.NombreCompleto LIKE '%' + @Busqueda + '%' OR
            c.Documento LIKE '%' + @Busqueda + '%'
        )
    ORDER BY v.FechaRegistro DESC
END
GO


-- ----- Actualización para Reporte de Compras -----
CREATE PROCEDURE sp_ReporteMaestroCompras(
    @FechaInicio VARCHAR(10),
    @FechaFin VARCHAR(10),
    @Busqueda VARCHAR(100)
)
AS
BEGIN
    SELECT
        c.Id,
        c.Estado, -- Devolvemos el nuevo campo Estado
        CONVERT(CHAR(10), c.FechaRegistro, 103) AS FechaRegistro,
        c.TipoDocumento,
        c.NumeroDocumento,
        c.MontoTotal,
        u.NombreCompleto AS UsuarioRegistro,
        pr.RazonSocial AS RazonSocialProveedor
    FROM COMPRA c
    INNER JOIN USUARIO u ON u.Id = c.IdUsuario
    INNER JOIN PROVEEDOR pr ON pr.Id = c.IdProveedor
    WHERE
        -- Por defecto, solo mostramos las activas (Estado = 1)
        c.Estado = 1 AND
        ( (@FechaInicio = '' AND @FechaFin = '') OR (CONVERT(DATE, c.FechaRegistro) BETWEEN @FechaInicio AND @FechaFin) )
        AND (
            c.NumeroDocumento LIKE '%' + @Busqueda + '%' OR
            pr.RazonSocial LIKE '%' + @Busqueda + '%'
        )
    ORDER BY c.FechaRegistro DESC
END
GO



