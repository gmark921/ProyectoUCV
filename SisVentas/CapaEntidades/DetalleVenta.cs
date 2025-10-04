// Entidad DetalleVenta: Modela un item dentro de una venta.
namespace CapaEntidades
{
    public class DetalleVenta
    {
        public string Id { get; set; }
        public Producto oProducto { get; set; } // Producto vendido
        public decimal PrecioVenta { get; set; }
        public decimal CantidadVendida { get; set; }
        public decimal SubTotal { get; set; }
    }
}

