using System.Data;
using CapaDatos;
using CapaEntidades;

namespace CapaNegocio
{
    public class CN_Compra
    {
        private CD_Compra objcd_compra = new CD_Compra();

        public bool Registrar(Compra obj, DataTable detalleCompra, out string NumeroGenerado, out string Mensaje)
        {
            return objcd_compra.Registrar(obj, detalleCompra, out NumeroGenerado, out Mensaje);
        }

        public Compra ObtenerCompra(string numero)
        {
            return objcd_compra.ObtenerCompra(numero);
        }

        public DataTable ObtenerDetalleCompra(string idCompra)
        {
            return objcd_compra.ObtenerDetalleCompra(idCompra);
        }

        public bool Anular(string id, out string Mensaje)
        {
            return objcd_compra.Anular(id, out Mensaje);
        }
    }
}