using System.Collections.Generic;
using CapaDatos;
using CapaEntidades;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria objcd_categoria = new CD_Categoria();

        public List<Categoria> Listar()
        {
            return objcd_categoria.Listar();
        }

        public string Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje += "La descripción de la categoría no puede ser vacía\n";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objcd_categoria.Registrar(obj, out Mensaje);
            }
            else
            {
                return "0";
            }
        }

        public bool Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje += "La descripción de la categoría no puede ser vacía\n";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objcd_categoria.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(string id, out string Mensaje)
        {
            return objcd_categoria.Eliminar(id, out Mensaje);
        }
    }
}

