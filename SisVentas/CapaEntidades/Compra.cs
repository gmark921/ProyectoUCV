using System;
using System.Collections.Generic;

// Entidad Compra: Representa una operación de compra a un proveedor.
namespace CapaEntidades
{
    public class Compra
    {
        public string Id { get; set; }
        public Usuario oUsuario { get; set; } // Usuario que registra la compra
        public Proveedor oProveedor { get; set; } // Proveedor al que se compra
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public decimal MontoTotal { get; set; }
        public List<DetalleCompra> oDetalleCompra { get; set; } // Lista de productos
        public string FechaRegistro { get; set; }
    }
}

