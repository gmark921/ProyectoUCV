using System;
using System.Data;
using System.Data.SqlClient;
using CapaEntidades;

namespace CapaDatos
{
    public class CD_Compra
    {
        public bool Registrar(Compra obj, DataTable detalleCompra, out string NumeroGenerado, out string Mensaje)
        {
            bool respuesta = false;
            NumeroGenerado = "";
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCompra", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.Id);
                    cmd.Parameters.AddWithValue("IdProveedor", obj.oProveedor.Id);
                    cmd.Parameters.AddWithValue("TipoDocumento", obj.TipoDocumento);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("DetalleCompra", detalleCompra);

                    cmd.Parameters.Add("IdResultado", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("NumeroDocumentoResultado", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
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

        public Compra ObtenerCompra(string numero)
        {
            Compra obj = null;
            using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
            {
                try
                {
                    string query = "SELECT c.Id, CONVERT(CHAR(10), c.FechaRegistro, 103) AS FechaRegistro, c.TipoDocumento, c.NumeroDocumento, c.MontoTotal, u.Id AS IdUsuario, u.NombreCompleto, pr.Id AS IdProveedor, pr.RazonSocial, pr.NombreComercial FROM COMPRA c INNER JOIN USUARIO u ON u.Id = c.IdUsuario INNER JOIN PROVEEDOR pr ON pr.Id = c.IdProveedor WHERE c.NumeroDocumento = @numero";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@numero", numero);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = new Compra()
                            {
                                Id = dr["Id"].ToString(),
                                FechaRegistro = dr["FechaRegistro"].ToString(),
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                                oUsuario = new Usuario() { Id = dr["IdUsuario"].ToString(), NombreCompleto = dr["NombreCompleto"].ToString() },
                                oProveedor = new Proveedor() { Id = dr["IdProveedor"].ToString(), RazonSocial = dr["RazonSocial"].ToString(), NombreComercial = dr["NombreComercial"].ToString() }
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener el objeto en CD_Compra: {ex.Message}");
                    obj = null;
                }
            }
            return obj;
        }

        public DataTable ObtenerDetalleCompra(string idCompra)
        {
            DataTable dt = new DataTable();
            using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
            {
                try
                {
                    string query = "SELECT p.Codigo, p.Nombre AS Producto, dc.PrecioCompra, dc.CantidadComprada, p.UnidadBase, dc.MontoTotal AS SubTotal " +
                                   "FROM DETALLE_COMPRA dc " +
                                   "INNER JOIN PRODUCTO p ON p.Id = dc.IdProducto " +
                                   "WHERE dc.IdCompra = @idCompra";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idCompra", idCompra);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error al obtener los datos: " + ex.Message);
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
                    SqlCommand cmd = new SqlCommand("sp_AnularCompra", oconexion);
                    cmd.Parameters.AddWithValue("IdCompra", id);
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