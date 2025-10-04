// Entidad Usuario: Representa a un usuario del sistema.
namespace CapaEntidades
{
    public class Usuario
    {
        public string Id { get; set; }
        public string Documento { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string ConfirmarClave { get; set; } // <-- PROPIEDAD AÑADIDA
        public Rol oRol { get; set; } // Referencia a la entidad Rol
        public bool Estado { get; set; }
    }
}

