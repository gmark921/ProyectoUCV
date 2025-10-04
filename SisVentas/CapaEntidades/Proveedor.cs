// Entidad Proveedor: Modela a un proveedor de productos.
namespace CapaEntidades
{
    public class Proveedor
    {
        public string Id { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public bool Estado { get; set; }
    }
}

