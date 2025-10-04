// Entidad Producto: Representa un producto del inventario.
namespace CapaEntidades
{
    public class Producto
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Categoria oCategoria { get; set; } // Referencia a la entidad Categoria
        public decimal Stock { get; set; }
        public string UnidadBase { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public bool Estado { get; set; }
    }
}

