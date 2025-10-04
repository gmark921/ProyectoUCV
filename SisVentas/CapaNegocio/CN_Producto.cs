using System.Collections.Generic;
using CapaDatos;
using CapaEntidades;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Producto objcd_producto = new CD_Producto();

        public List<Producto> Listar() => objcd_producto.Listar();

        public string ObtenerSiguienteCodigo(string idCategoria)
        {
            return objcd_producto.ObtenerSiguienteCodigo(idCategoria);
        }

        public string Registrar(Producto obj, out string Mensaje)
        {
            /*Mensaje = string.Empty;
            if (string.IsNullOrWhiteSpace(obj.Nombre)) Mensaje += "El nombre del producto no puede ser vacío.\n";
            if (obj.PrecioVenta <= 0) Mensaje += "El precio de venta debe ser mayor a cero.\n";
            if (!obj.Estado) Mensaje += "No se puede registrar un producto como 'No Activo'.\n";

            if (string.IsNullOrEmpty(Mensaje))
                return objcd_producto.Registrar(obj, out Mensaje);
            else
                return "0";*/
            Mensaje = string.Empty;
            if (string.IsNullOrWhiteSpace(obj.Nombre)) Mensaje += "El nombre del producto no puede ser vacío.\n";

            // Asegúrate de que ambas validaciones estén aquí
            if (obj.PrecioCompra <= 0) Mensaje += "El precio de compra debe ser mayor a cero.\n";
            if (obj.PrecioVenta <= 0) Mensaje += "El precio de venta debe ser mayor a cero.\n";

            if (!obj.Estado) Mensaje += "No se puede registrar un producto como 'No Activo'.\n";

            if (string.IsNullOrEmpty(Mensaje))
                return objcd_producto.Registrar(obj, out Mensaje);
            else
                return "0";
        }

        // Método Editar en CN_Producto.cs
        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrWhiteSpace(obj.Nombre)) Mensaje += "El nombre del producto no puede ser vacío.\n";

            // Y también aquí
            if (obj.PrecioCompra <= 0) Mensaje += "El precio de compra debe ser mayor a cero.\n";
            if (obj.PrecioVenta <= 0) Mensaje += "El precio de venta debe ser mayor a cero.\n";

            if (string.IsNullOrEmpty(Mensaje))
                return objcd_producto.Editar(obj, out Mensaje);
            else
                return false;
        }

        public bool Eliminar(string id, out string Mensaje) => objcd_producto.Eliminar(id, out Mensaje);

        // --- MÉTODO CORREGIDO Y AÑADIDO ---
        // Este es el método que faltaba
        public Producto ObtenerPorCodigo(string codigo)
        {
            return objcd_producto.ObtenerPorCodigo(codigo);
        }
    }
}