using System;
using System.Data;
using System.Data.SqlClient;
using CapaEntidades;

namespace CapaDatos
{
    public class CD_Venta
    {
        // ----- MODIFICACIÓN AQUÍ -----
        // El método ahora devuelve un booleano y entrega el número generado por parámetro de salida.
        public bool Registrar(Venta obj, DataTable detalleVenta, out string NumeroGenerado, out string Mensaje)
        {
            bool respuesta = false;
            NumeroGenerado = "";
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarVenta", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.Id);
                    // Manejamos el caso de un cliente nulo (venta genérica)
                    cmd.Parameters.AddWithValue("IdCliente", obj.oCliente != null ? obj.oCliente.Id : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("TipoDocumento", obj.TipoDocumento);
                    cmd.Parameters.AddWithValue("MontoPago", obj.MontoPago);
                    cmd.Parameters.AddWithValue("MontoCambio", obj.MontoCambio);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("DetalleVenta", detalleVenta);

                    cmd.Parameters.Add("IdResultado", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("NumeroDocumentoResultado", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output; // NUEVO
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    if (!string.IsNullOrEmpty(cmd.Parameters["IdResultado"].Value.ToString()))
                    {
                        NumeroGenerado = cmd.Parameters["NumeroDocumentoResultado"].Value.ToString();
                        respuesta = true;
                    }
                    else
                    {
                        Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;
        }

        public Venta ObtenerVenta(string numero)
        {
            Venta obj = null;
            using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
            {
                // ... (Este método se mantiene igual)
            }
            return obj;
        }

        public DataTable ObtenerDetalleVenta(string idVenta)
        {
            DataTable dt = new DataTable();
            using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
            {
                try
                {
                    // Query mejorada para incluir el código y la unidad del producto.
                    string query = "SELECT p.Codigo, p.Nombre AS Producto, dv.PrecioVenta, dv.CantidadVendida, p.UnidadBase, dv.SubTotal " +
                                   "FROM DETALLE_VENTA dv " +
                                   "INNER JOIN PRODUCTO p ON p.Id = dv.IdProducto " +
                                   "WHERE dv.IdVenta = @idVenta";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idVenta", idVenta);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error al obtener los datos en CD_Venta: " + ex.Message);
                    dt = new DataTable();
                }
            }
            return dt;
        }


        public bool Anular(string id, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
                {
                    SqlCommand cmd = new SqlCommand("sp_AnularVenta", oconexion);
                    cmd.Parameters.AddWithValue("IdVenta", id);
                    cmd.Parameters.Add("Respuesta", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Respuesta"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;
        }
    }
}