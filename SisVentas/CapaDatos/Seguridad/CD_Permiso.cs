using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidades;

namespace CapaDatos
{
    public class CD_Permiso
    {
        public List<Permiso> Listar(string idRol)
        {
            List<Permiso> lista = new List<Permiso>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
            {
                try
                {
                    // Query to get menu names for a given role.
                    string query = "SELECT p.IdRol, p.NombreMenu FROM PERMISO p INNER JOIN ROL r ON r.Id = p.IdRol WHERE r.Id = @idRol";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idRol", idRol);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Permiso()
                            {
                                oRol = new Rol() { Id = dr["IdRol"].ToString() },
                                NombreMenu = dr["NombreMenu"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error en CD_Permiso.Listar(): " + ex.Message);
                    lista = new List<Permiso>();
                }
            }
            return lista;
        }
        public bool ActualizarPermisos(string idRol, DataTable permisos, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadena()))
                {
                    SqlCommand cmd = new SqlCommand("sp_ActualizarPermisos", oconexion);
                    cmd.Parameters.AddWithValue("IdRol", idRol);
                    cmd.Parameters.AddWithValue("Permisos", permisos);

                    // Output parameters
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

