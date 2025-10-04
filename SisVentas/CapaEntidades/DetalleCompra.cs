// Entidad DetalleCompra: Modela un item dentro de una compra.
namespace CapaEntidades
{
    public class DetalleCompra
    {
        public string Id { get; set; }
        public Producto oProducto { get; set; } // Producto comprado
        public decimal PrecioCompra { get; set; }
        public decimal CantidadComprada { get; set; }
        public decimal MontoTotal { get; set; }
    }
}

