using System;
using System.Collections.Generic;

// Entidad Venta: Representa una operación de venta a un cliente.
namespace CapaEntidades
{
    public class Venta
    {
        public string Id { get; set; }
        public Usuario oUsuario { get; set; } // Usuario que registra la venta
        public Cliente oCliente { get; set; } // Cliente al que se vende
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public decimal MontoPago { get; set; }
        public decimal MontoCambio { get; set; }
        public decimal MontoTotal { get; set; }
        public List<DetalleVenta> oDetalleVenta { get; set; } // Lista de productos
        public string FechaRegistro { get; set; }
    }
}

