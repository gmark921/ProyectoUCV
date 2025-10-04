using System.Data;
using CapaDatos;
using CapaEntidades;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Venta objcd_venta = new CD_Venta();

        // ----- MODIFICACIÓN AQUÍ -----
        // Adaptamos la firma del método para que coincida con la de la capa de datos.
        public bool Registrar(Venta obj, DataTable detalleVenta, out string NumeroGenerado, out string Mensaje)
        {
            return objcd_venta.Registrar(obj, detalleVenta, out NumeroGenerado, out Mensaje);
        }

        public Venta ObtenerVenta(string numero)
        {
            return objcd_venta.ObtenerVenta(numero);
        }

        public DataTable ObtenerDetalleVenta(string idVenta)
        {
            return objcd_venta.ObtenerDetalleVenta(idVenta);
        }


        public bool Anular(string id, out string Mensaje)
        {
            return objcd_venta.Anular(id, out Mensaje);
        }
    }
}