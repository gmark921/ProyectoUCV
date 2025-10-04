using System.Collections.Generic;
using CapaDatos;
using CapaEntidades;

namespace CapaNegocio
{
    public class CN_Rol
    {
        private CD_Rol objcd_rol = new CD_Rol();

        public List<Rol> Listar()
        {
            return objcd_rol.Listar();
        }

        public string Registrar(Rol obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje += "La descripción del rol no puede estar vacía.\n";
            }
            if (!string.IsNullOrEmpty(Mensaje))
            {
                return "0";
            }
            return objcd_rol.Registrar(obj, out Mensaje);
        }

        public bool Editar(Rol obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje += "La descripción del rol no puede estar vacía.\n";
            }
            if (!string.IsNullOrEmpty(Mensaje))
            {
                return false;
            }
            return objcd_rol.Editar(obj, out Mensaje);
        }

        public bool Eliminar(string idRol, out string Mensaje)
        {
            return objcd_rol.Eliminar(idRol, out Mensaje);
        }
    }
}

