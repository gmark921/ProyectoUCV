// Entidad Permiso: Define los accesos de un Rol a un menú.
namespace CapaEntidades
{
    public class Permiso
    {
        public string Id { get; set; }
        public Rol oRol { get; set; } // Referencia a la entidad Rol
        public string NombreMenu { get; set; }
    }
}

