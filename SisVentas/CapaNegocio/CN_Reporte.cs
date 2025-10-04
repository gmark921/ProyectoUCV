using System.Collections.Generic;
using CapaDatos;
using CapaEntidades;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        private CD_Reporte objcd_reporte = new CD_Reporte();

        public List<ReporteCompra> ReporteMaestroCompra(string fechaInicio, string fechaFin, string busqueda)
        {
            return objcd_reporte.ReporteMaestroCompra(fechaInicio, fechaFin, busqueda);
        }

        public List<ReporteVenta> ReporteMaestroVenta(string fechaInicio, string fechaFin, string busqueda)
        {
            return objcd_reporte.ReporteMaestroVenta(fechaInicio, fechaFin, busqueda);
        }
    }
}