// Entidad Cliente: Modela a un cliente de la tienda.
namespace CapaEntidades
{
    public class Cliente
    {
        public string Id { get; set; }
        public string Documento { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public bool Estado { get; set; }
    }
}

