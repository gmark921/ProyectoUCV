USE DBSISVENTAS;
GO

DELETE FROM DETALLE_VENTA;
DELETE FROM DETALLE_COMPRA;
GO

-- Tablas principales que contienen las llaves foráneas
DELETE FROM VENTA;
DELETE FROM COMPRA;
DELETE FROM PERMISO;
DELETE FROM PRODUCTO;
DELETE FROM USUARIO;
GO

-- Tablas catálogo
DELETE FROM CLIENTE;
DELETE FROM PROVEEDOR;
DELETE FROM CATEGORIA;
DELETE FROM ROL;
GO

select * from USUARIO
-- =======================================================================
-- SECCIÓN DE INSERCIÓN DE NUEVOS DATOS SIMPLIFICADOS
-- Se inserta el doble de la cantidad de registros originales.
-- =======================================================================

-- 1. Insertar 4 Roles
INSERT INTO ROL(Id, Descripcion) VALUES
('ROL01', 'ADMINISTRADOR'),
('ROL02', 'VENDEDOR'),
('ROL03', 'SUPERVISOR')
GO

-- 2. Insertar 4 Usuarios (Clave para todos: '123')
INSERT INTO USUARIO(Id, Documento, NombreCompleto, Correo, Clave, IdRol, Estado) VALUES
('USR010', '55', 'Juan aPérez García', 'jperez@correo.com', '123456789', 'ROL01', 1),
('USR020', '222', 'Ana aLópez Martínez', 'alopez@correo.com', '222', 'ROL02', 1),
('USR030', '333', 'Carlosa Sánchez Rodríguez', 'csanchez@correo.com', '333', 'ROL03', 1),
('USR040', '444', 'Laura aTorres Flores', 'ltorres@correo.com', '444', 'ROL02', 0);
GO

-- 3. Insertar Permisos para los 4 roles
-- Administrador (Todos los permisos)
INSERT INTO PERMISO(Id, IdRol, NombreMenu) VALUES
('PERM001', 'ROL01', 'menuSeguridad'),
('PERM002', 'ROL01', 'submenuUsuarios'),
('PERM003', 'ROL01', 'submenuPermisos'),
('PERM004', 'ROL01', 'menuMantenedor'),
('PERM005', 'ROL01', 'submenuCategorias'),
('PERM006', 'ROL01', 'submenuProductos'),
('PERM007', 'ROL01', 'submenuClientes'),
('PERM008', 'ROL01', 'submenuProveedores'),
('PERM009', 'ROL01', 'menuVentas'),
('PERM010', 'ROL01', 'menuCompras'),
('PERM011', 'ROL01', 'menuReportes'),
('PERM012', 'ROL01', 'submenuReporteVentas'),
('PERM013', 'ROL01', 'submenuReporteCompras');

-- Vendedor (Solo Ventas, Clientes y reportes de ventas)
INSERT INTO PERMISO(Id, IdRol, NombreMenu) VALUES
('PERM014', 'ROL02', 'menuVentas'),
('PERM015', 'ROL02', 'submenuClientes'),
('PERM016', 'ROL02', 'menuReportes'),
('PERM017', 'ROL02', 'submenuReporteVentas');

-- Supervisor (Todo excepto Seguridad)
INSERT INTO PERMISO(Id, IdRol, NombreMenu) VALUES
('PERM018', 'ROL03', 'menuMantenedor'),
('PERM019', 'ROL03', 'submenuCategorias'),
('PERM020', 'ROL03', 'submenuProductos'),
('PERM021', 'ROL03', 'submenuClientes'),
('PERM022', 'ROL03', 'submenuProveedores'),
('PERM023', 'ROL03', 'menuVentas'),
('PERM024', 'ROL03', 'menuCompras'),
('PERM025', 'ROL03', 'menuReportes'),
('PERM026', 'ROL03', 'submenuReporteVentas'),
('PERM027', 'ROL03', 'submenuReporteCompras');

-- 4. Insertar 8 Categorías
INSERT INTO CATEGORIA(Id, Descripcion, Estado) VALUES
('CAT01', 'Electrónica', 1),
('CAT02', 'Ropa y Accesorios', 1),
('CAT03', 'Hogar y Cocina', 1),
('CAT04', 'Alimentos y Bebidas', 1),
('CAT05', 'Deportes', 1),
('CAT06', 'Juguetes', 1),
('CAT07', 'Libros', 1),
('CAT08', 'Salud y Belleza', 1);
GO

-- 5. Insertar 20 Productos
INSERT INTO PRODUCTO(Id, Codigo, Nombre, Descripcion, IdCategoria, Stock, UnidadBase, PrecioCompra, PrecioVenta, Estado) VALUES
('PROD001', 'ELEC-001', 'Teclado Mecánico RGB', 'Teclado para gaming con luces.', 'CAT01', 50, 'Unidades', 45.00, 75.00, 1),
('PROD002', 'ELEC-002', 'Mouse Inalámbrico', 'Mouse ergonómico de 6 botones.', 'CAT01', 80, 'Unidades', 15.00, 25.00, 1),
('PROD003', 'ROPA-001', 'Camiseta de Algodón', 'Camiseta básica color negro.', 'CAT02', 200, 'Unidades', 8.00, 15.00, 1),
('PROD004', 'ROPA-002', 'Pantalón Jean Clásico', 'Pantalón de mezclilla azul.', 'CAT02', 150, 'Unidades', 25.00, 45.00, 1),
('PROD005', 'Hogar-001', 'Juego de Sartenes', 'Set de 3 sartenes antiadherentes.', 'CAT03', 60, 'Unidades', 30.00, 55.00, 1),
('PROD006', 'Hogar-002', 'Licuadora 1.5L', 'Licuadora de 5 velocidades.', 'CAT03', 40, 'Unidades', 40.00, 65.00, 1),
('PROD007', 'ALIM-001', 'Café en Grano 1kg', 'Café tostado origen colombiano.', 'CAT04', 300, 'Kg', 12.00, 22.00, 1),
('PROD008', 'ALIM-002', 'Aceite de Oliva 500ml', 'Aceite extra virgen.', 'CAT04', 400, 'Unidades', 7.00, 13.00, 1),
('PROD009', 'DEPO-001', 'Balón de Fútbol N°5', 'Balón profesional.', 'CAT05', 120, 'Unidades', 20.00, 35.00, 1),
('PROD010', 'JUGU-001', 'Set de Bloques de Construcción', 'Caja con 500 piezas.', 'CAT06', 90, 'Unidades', 22.00, 40.00, 1),
('PROD011', 'LIBR-001', 'Novela de Ficción', 'Bestseller internacional.', 'CAT07', 250, 'Unidades', 10.00, 18.00, 1),
('PROD012', 'SALU-001', 'Crema Hidratante Facial', 'Crema para todo tipo de piel.', 'CAT08', 180, 'Unidades', 15.00, 28.00, 1),
('PROD013', 'ELEC-003', 'Audífonos Bluetooth', 'Audífonos con cancelación de ruido.', 'CAT01', 100, 'Unidades', 50.00, 90.00, 1),
('PROD014', 'ROPA-003', 'Chaqueta Impermeable', 'Chaqueta ligera para lluvia.', 'CAT02', 70, 'Unidades', 35.00, 60.00, 1),
('PROD015', 'HOGAR-003', 'Robot Aspiradora', 'Aspiradora inteligente con mapeo.', 'CAT03', 30, 'Unidades', 150.00, 250.00, 1),
('PROD016', 'ALIM-003', 'Barra de Chocolate 70%', 'Chocolate oscuro orgánico.', 'CAT04', 500, 'Unidades', 2.00, 4.50, 1),
('PROD017', 'DEPO-002', 'Set de Mancuernas 10kg', 'Par de mancuernas ajustables.', 'CAT05', 50, 'Unidades', 40.00, 70.00, 1),
('PROD018', 'JUGU-002', 'Auto a Control Remoto', 'Auto de carreras escala 1:16.', 'CAT06', 80, 'Unidades', 25.00, 45.00, 1),
('PROD019', 'LIBR-002', 'Libro de Cocina Vegana', 'Más de 100 recetas.', 'CAT07', 150, 'Unidades', 18.00, 30.00, 1),
('PROD020', 'SALU-002', 'Protector Solar SPF 50', 'Protector de amplio espectro.', 'CAT08', 200, 'Unidades', 12.00, 22.00, 1);
GO

-- 6. Insertar 10 Clientes
INSERT INTO CLIENTE(Id, Documento, NombreCompleto, Correo, Telefono, Estado) VALUES
('CLI01', '20111111111', 'Comercial ABC S.A.C.', 'compras@abc.com', '999111222', 1),
('CLI02', '10222222222', 'María Gómez', 'mgomez@personal.com', '999222333', 1),
('CLI03', '20333333333', 'Distribuidora El Sol E.I.R.L.', 'logistica@elsol.com', '999333444', 1),
('CLI04', '10444444444', 'Pedro Castillo', 'pcastillo@personal.com', '999444555', 1),
('CLI05', '20555555555', 'Importaciones Rápidas S.A.', 'admin@rapidas.com', '999555666', 1),
('CLI06', '20666666666', 'Tiendas La Confianza S.A.C.', 'gerencia@laconfianza.com', '999666777', 1),
('CLI07', '10777777777', 'Lucía Fernández', 'lfernandez@personal.com', '999777888', 1),
('CLI08', '20888888888', 'Mercado Central S.R.L.', 'pedidos@mcentral.com', '999888999', 1),
('CLI09', '10999999999', 'Javier Díaz', 'jdiaz@personal.com', '999999000', 1),
('CLI10', '20101010101', 'Servicios Generales Perú S.A.', 'contacto@sgperu.com', '999000111', 1);
GO

-- 7. Insertar 8 Proveedores
INSERT INTO PROVEEDOR(Id, RazonSocial, NombreComercial, Correo, Telefono, Estado) VALUES
('PROV01', 'TECNOLOGIA GLOBAL S.A.', 'TecnoGlobal', 'ventas@tecnoglobal.com', '014441111', 1),
('PROV02', 'TEXTILES NACIONALES S.A.C.', 'TextilNac', 'pedidos@textilnac.com', '014442222', 1),
('PROV03', 'DISTRIBUIDORA HOGAR FELIZ', 'Hogar Feliz', 'contacto@hogarfeliz.com', '014443333', 1),
('PROV04', 'ALIMENTOS DEL PERU S.A.', 'AliPerú', 'info@aliperu.com', '014444444', 1),
('PROV05', 'IMPORTADORA DEPORTIVA MUNDIAL', 'DeporMundo', 'ventas@depormundo.com', '015551111', 1),
('PROV06', 'JUGUETES DIDACTICOS S.R.L.', 'DidaJuegos', 'pedidos@didajuegos.com', '015552222', 1),
('PROV07', 'EDITORIAL EL SABER E.I.R.L.', 'El Saber', 'contacto@elsaber.com', '015553333', 1),
('PROV08', 'LABORATORIOS DERMA BELLA S.A.C.', 'DermaBella', 'info@dermabella.com', '015554444', 1);
GO