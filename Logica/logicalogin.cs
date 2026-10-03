using System.Data;
using System.Data.SqlClient;
using PRACTICA_Saber_3_Camilo_Valderrama.Datos;

namespace PRACTICA_Saber_3_Camilo_Valderrama.Logica
{
    public class LogicaLogin
    {
        private AccesoDatos conexion = new AccesoDatos();

        public bool ValidarUsuario(string usuario, string clave)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM TBLSEGURIDAD WHERE StrUsuario = @usu AND StrClave = @cla";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@usu", usuario);
                cmd.Parameters.AddWithValue("@cla", clave);

                int resultado = (int)cmd.ExecuteScalar();
                return resultado > 0;
            }
        }
    }
}