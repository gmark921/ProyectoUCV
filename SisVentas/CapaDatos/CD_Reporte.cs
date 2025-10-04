using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public List<ReporteCompra> ReporteMaestroCompra(string fechaInicio, string fechaFin, string busqueda)
        {
            List<ReporteCompra> lista = new List<ReporteCompra>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
            {
                try
                {
                    // Se llama al procedimiento almacenado correcto para el reporte maestro de compras
                    SqlCommand cmd = new SqlCommand("sp_ReporteMaestroCompras", oconexion);
                    cmd.Parameters.AddWithValue("FechaInicio", fechaInicio ?? "");
                    cmd.Parameters.AddWithValue("FechaFin", fechaFin ?? "");
                    cmd.Parameters.AddWithValue("Busqueda", busqueda ?? "");
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteCompra()
                            {
                                CodigoProducto = dr["Id"].ToString(), // ID de la compra
                                FechaRegistro = dr["FechaRegistro"].ToString(),
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                                UsuarioRegistro = dr["UsuarioRegistro"].ToString(),
                                RazonSocialProveedor = dr["RazonSocialProveedor"].ToString()
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    lista = new List<ReporteCompra>();
                }
            }
            return lista;
        }

        public List<ReporteVenta> ReporteMaestroVenta(string fechaInicio, string fechaFin, string busqueda)
        {
            List<ReporteVenta> lista = new List<ReporteVenta>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteMaestroVentas", oconexion);
                    cmd.Parameters.AddWithValue("FechaInicio", fechaInicio ?? "");
                    cmd.Parameters.AddWithValue("FechaFin", fechaFin ?? "");
                    cmd.Parameters.AddWithValue("Busqueda", busqueda ?? "");
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteVenta()
                            {
                                CodigoProducto = dr["Id"].ToString(), // ID de la venta
                                FechaRegistro = dr["FechaRegistro"].ToString(),
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                                UsuarioRegistro = dr["UsuarioRegistro"].ToString(),
                                DocumentoCliente = dr["DocumentoCliente"].ToString(),
                                NombreCliente = dr["NombreCliente"].ToString()
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    lista = new List<ReporteVenta>();
                }
            }
            return lista;
        }
    }
}