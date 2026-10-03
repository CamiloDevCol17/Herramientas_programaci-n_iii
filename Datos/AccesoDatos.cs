using System;
using System.Data.SqlClient;

namespace PRACTICA_Saber_3_Camilo_Valderrama.Datos
{
    /// <summary>
    /// Clase encargada de gestionar la conexión a la base de datos SQL Server.
    /// Proporciona un método para obtener una conexión a la base de datos DBFACTURAS.
    /// </summary>
    public class AccesoDatos
    {
        /// <summary>
        /// Cadena de conexión con los parámetros para conectarse a SQL Server localmente.
        /// Incluye: servidor, base de datos, seguridad integrada y certificado de confianza.
        /// </summary>
        private string cadenaConexion = "Server=localhost;Database=DBFACTURAS;Integrated Security=True;TrustServerCertificate=True;";

        /// <summary>
        /// Obtiene una nueva conexión a la base de datos.
        /// Retorna: SqlConnection lista para ser abierta y utilizada
        /// </summary>
        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}