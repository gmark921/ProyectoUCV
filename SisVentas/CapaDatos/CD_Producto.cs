using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidades;

namespace CapaDatos
{
    public class CD_Producto
    {
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
            {
                try
                {
                    string query = "SELECT p.Id, p.Codigo, p.Nombre, p.Descripcion, c.Id AS IdCategoria, c.Descripcion AS Categoria, p.Stock, p.UnidadBase, p.PrecioCompra, p.PrecioVenta, p.Estado FROM PRODUCTO p INNER JOIN CATEGORIA c ON c.Id = p.IdCategoria";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto()
                            {
                                Id = dr["Id"].ToString(),
                                Codigo = dr["Codigo"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                oCategoria = new Categoria() { Id = dr["IdCategoria"].ToString(), Descripcion = dr["Categoria"].ToString() },
                                Stock = Convert.ToDecimal(dr["Stock"]),
                                UnidadBase = dr["UnidadBase"].ToString(),
                                PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error en CD_Producto.Listar(): " + ex.Message);
                    lista = new List<Producto>();
                }
            }
            return lista;
        }

        public string ObtenerSiguienteCodigo(string idCategoria)
        {
            string codigoGenerado = string.Empty;
            using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ObtenerSiguienteCodigoProducto", oconexion);
                    cmd.Parameters.AddWithValue("IdCategoria", idCategoria);
                    cmd.Parameters.Add("CodigoGenerado", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    codigoGenerado = cmd.Parameters["CodigoGenerado"].Value.ToString();
                }
                catch { codigoGenerado = "Error"; }
            }
            return codigoGenerado;
        }

        public string Registrar(Producto obj, out string Mensaje)
        {
            string idResultado = string.Empty;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", oconexion);
                    cmd.Parameters.AddWithValue("Codigo", obj.Codigo);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria.Id);
                    cmd.Parameters.AddWithValue("UnidadBase", obj.UnidadBase);
                    cmd.Parameters.AddWithValue("PrecioCompra", obj.PrecioCompra); // <-- LÍNEA AÑADIDA
                    cmd.Parameters.AddWithValue("PrecioVenta", obj.PrecioVenta);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);
                    cmd.Parameters.Add("IdResultado", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    idResultado = cmd.Parameters["IdResultado"].Value.ToString();
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idResultado = string.Empty;
                Mensaje = ex.Message;
            }
            return idResultado;
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", oconexion);
                    cmd.Parameters.AddWithValue("IdProducto", obj.Id);
                    cmd.Parameters.AddWithValue("Codigo", obj.Codigo);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria.Id);
                    cmd.Parameters.AddWithValue("UnidadBase", obj.UnidadBase);
                    cmd.Parameters.AddWithValue("PrecioCompra", obj.PrecioCompra); // <-- LÍNEA AÑADIDA
                    cmd.Parameters.AddWithValue("PrecioVenta", obj.PrecioVenta); // <-- LÍNEA AÑADIDA
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);
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

        public bool Eliminar(string id, out string Mensaje)
        {
            // ... (Este método no necesita cambios)
            bool respuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarProducto", oconexion);
                    cmd.Parameters.AddWithValue("IdProducto", id);
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

        public Producto ObtenerPorCodigo(string codigo)
        {
            // ... (Este método no necesita cambios)
            Producto obj = null;
            using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
            {
                try
                {
                    string query = "SELECT p.Id, p.Codigo, p.Nombre, p.Descripcion, c.Id AS IdCategoria, c.Descripcion AS Categoria, p.Stock, p.UnidadBase, p.PrecioCompra, p.PrecioVenta, p.Estado FROM PRODUCTO p INNER JOIN CATEGORIA c ON c.Id = p.IdCategoria WHERE p.Codigo = @codigo AND p.Estado = 1";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@codigo", codigo);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = new Producto()
                            {
                                Id = dr["Id"].ToString(),
                                Codigo = dr["Codigo"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                oCategoria = new Categoria() { Id = dr["IdCategoria"].ToString(), Descripcion = dr["Categoria"].ToString() },
                                Stock = Convert.ToDecimal(dr["Stock"]),
                                UnidadBase = dr["UnidadBase"].ToString(),
                                PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener el objeto en CD_Producto: {ex.Message}");
                    obj = null;
                }
            }
            return obj;
        }
    }
}