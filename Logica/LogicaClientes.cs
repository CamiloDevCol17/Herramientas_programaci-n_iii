using System;
using System.Data;
using System.Data.SqlClient;
using PRACTICA_Saber_3_Camilo_Valderrama.Datos;

namespace PRACTICA_Saber_3_Camilo_Valderrama.Logica
{
    /// <summary>
    /// Clase de lógica de negocio para gestionar clientes.
    /// Contiene métodos para: obtener todos, buscar por ID, insertar, actualizar y eliminar clientes.
    /// Interactúa con la base de datos a través de AccesoDatos.
    /// </summary>
    public class LogicaClientes
    {
        private AccesoDatos conexion = new AccesoDatos();

        /// <summary>
        /// Obtiene todos los clientes de la base de datos.
        /// Parámetro: filtro (opcional) - Busca por nombre o documento
        /// Retorna: DataTable con los resultados
        /// </summary>
        public DataTable ObtenerTodos(string filtro = "")
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                string query = "SELECT IdCliente, StrNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail, StrCiudad FROM TBLCLIENTE";
                if (!string.IsNullOrWhiteSpace(filtro))
                    query += " WHERE StrNombre LIKE @filtro OR CAST(NumDocumento AS varchar) LIKE @filtro";

                SqlCommand cmd = new SqlCommand(query, con);
                if (!string.IsNullOrWhiteSpace(filtro))
                    cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Obtiene un cliente específico por su ID.
        /// Parámetro: id - ID del cliente a buscar
        /// Retorna: DataRow con los datos del cliente, o null si no existe
        /// </summary>
        public DataRow? ObtenerPorId(int id)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM TBLCLIENTE WHERE IdCliente = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
        }

        /// <summary>
        /// Inserta un nuevo cliente en la base de datos.
        /// Valida que los datos sean correctos antes de insertar.
        /// Parámetros: nombre, documento, dirección, teléfono, email, ciudad
        /// Lanza excepción si hay error de validación o base de datos
        /// </summary>
        public void Insertar(string nombre, long documento, string direccion, string telefono, string email, string ciudad)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del cliente es obligatorio.");
            if (documento <= 0)
                throw new ArgumentException("El documento debe ser un número válido mayor a cero.");

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                string query = "INSERT INTO TBLCLIENTE (StrNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail, StrCiudad) " +
                               "VALUES (@nom, @doc, @dir, @tel, @email, @ciudad)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@nom", nombre);
                cmd.Parameters.AddWithValue("@doc", documento);
                cmd.Parameters.AddWithValue("@dir", direccion);
                cmd.Parameters.AddWithValue("@tel", telefono);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@ciudad", ciudad);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Actualiza los datos de un cliente existente.
        /// Valida que los datos sean correctos antes de actualizar.
        /// Parámetros: id del cliente a actualizar, y nuevos datos (nombre, documento, etc.)
        /// Lanza excepción si hay error de validación o base de datos
        /// </summary>
        public void Actualizar(int id, string nombre, long documento, string direccion, string telefono, string email, string ciudad)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del cliente es obligatorio.");
            if (documento <= 0)
                throw new ArgumentException("El documento debe ser un número válido mayor a cero.");

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                string query = "UPDATE TBLCLIENTE SET StrNombre=@nom, NumDocumento=@doc, StrDireccion=@dir, " +
                               "StrTelefono=@tel, StrEmail=@email, StrCiudad=@ciudad WHERE IdCliente=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nom", nombre);
                cmd.Parameters.AddWithValue("@doc", documento);
                cmd.Parameters.AddWithValue("@dir", direccion);
                cmd.Parameters.AddWithValue("@tel", telefono);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@ciudad", ciudad);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Elimina un cliente de la base de datos por su ID.
        /// Parámetro: id del cliente a eliminar
        /// Lanza excepción si hay error en base de datos
        /// </summary>
        public void Eliminar(int id)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM TBLCLIENTE WHERE IdCliente = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
