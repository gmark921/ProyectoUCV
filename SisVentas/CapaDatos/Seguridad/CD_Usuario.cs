using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidades;

namespace CapaDatos
{
    public class CD_Usuario
    {
        public Usuario Login(string documento)
        {
            Usuario usuario = null;
            using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_LoginUsuario", oconexion);
                    cmd.Parameters.AddWithValue("@Documento", documento);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read()) // Usamos if porque solo esperamos un resultado
                        {
                            usuario = new Usuario()
                            {
                                Id = dr["Id"].ToString(),
                                Documento = dr["Documento"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"]),
                                oRol = new Rol() { Id = dr["IdRol"].ToString(), Descripcion = dr["DescripcionRol"].ToString() }
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error en CD_Usuario.Login(): " + ex.Message);
                    usuario = null;
                }
            }
            return usuario;
        }
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
            {
                try
                {
                    string query = "SELECT u.Id, u.Documento, u.NombreCompleto, u.Correo, u.Clave, u.Estado, r.Id as IdRol, r.Descripcion as DescripcionRol FROM USUARIO u INNER JOIN ROL r ON r.Id = u.IdRol";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario()
                            {
                                Id = dr["Id"].ToString(),
                                Documento = dr["Documento"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"]),
                                oRol = new Rol() { Id = dr["IdRol"].ToString(), Descripcion = dr["DescripcionRol"].ToString() }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error en CD_Usuario.Listar(): " + ex.Message);
                    lista = new List<Usuario>();
                }
            }
            return lista;
        }
        public string Registrar(Usuario obj, out string Mensaje)
        {
            string idautogenerado = "0";
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de ENTRADA
                    cmd.Parameters.AddWithValue("Documento", obj.Documento);
                    cmd.Parameters.AddWithValue("NombreCompleto", obj.NombreCompleto);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("IdRol", obj.oRol.Id); // <-- ESTA LÍNEA FALTABA
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);

                    // Parámetros de SALIDA
                    cmd.Parameters.Add("IdUsuarioResultado", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idautogenerado = cmd.Parameters["IdUsuarioResultado"].Value.ToString();
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerado = "0";
                Mensaje = ex.Message;
            }

            return idautogenerado;

        }
        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de ENTRADA
                    cmd.Parameters.AddWithValue("IdUsuario", obj.Id);
                    cmd.Parameters.AddWithValue("Documento", obj.Documento);
                    cmd.Parameters.AddWithValue("NombreCompleto", obj.NombreCompleto);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave); // <-- PARÁMETRO AÑADIDO
                    cmd.Parameters.AddWithValue("IdRol", obj.oRol.Id);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);

                    // Parámetros de SALIDA
                    cmd.Parameters.Add("Respuesta", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

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
        public bool Eliminar(string idUsuario, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", idUsuario);
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

