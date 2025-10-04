using System.Collections.Generic;
using System.Data;
using CapaDatos;
using CapaEntidades;

namespace CapaNegocio
{
    public class CN_Permiso
    {
        private CD_Permiso objcd_permiso = new CD_Permiso();

        public List<Permiso> Listar(string idRol)
        {
            return objcd_permiso.Listar(idRol);
        }

        public bool ActualizarPermisos(string idRol, DataTable permisos, out string Mensaje)
        {
            return objcd_permiso.ActualizarPermisos(idRol, permisos, out Mensaje);
        }
    }
}

