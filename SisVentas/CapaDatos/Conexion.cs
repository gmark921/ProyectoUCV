using System.Configuration;
namespace CapaDatos
{
    public class Conexion
    {
        public static string GetCadena()
        {
            return ConfigurationManager.ConnectionStrings["cadena_conexion"].ToString();
        }
    }
}

