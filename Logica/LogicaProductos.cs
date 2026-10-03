using System;
using System.Data;
using System.Data.SqlClient;
using PRACTICA_Saber_3_Camilo_Valderrama.Datos;

namespace PRACTICA_Saber_3_Camilo_Valderrama.Logica
{
    public class LogicaProductos
    {
        private AccesoDatos conexion = new AccesoDatos();

        /// <summary>
        /// Obtiene todos los productos. Si se pasa filtro, busca por nombre o código.
        /// </summary>
        public DataTable ObtenerTodos(string filtro = "")
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                string query = "SELECT IdProducto, StrNombre, StrCodigo, NumPrecioCompra, NumPrecioVenta, NumStock FROM TBLPRODUCTO";
                if (!string.IsNullOrWhiteSpace(filtro))
                    query += " WHERE StrNombre LIKE @filtro OR StrCodigo LIKE @filtro";

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
        /// Obtiene un producto por su ID.
        /// </summary>
        public DataRow? ObtenerPorId(int id)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM TBLPRODUCTO WHERE IdProducto = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
        }

        /// <summary>
        /// Inserta un nuevo producto con validación de campos obligatorios.
        /// </summary>
        public void Insertar(string nombre, string codigo, double precioCompra, double precioVenta, int stock)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del producto es obligatorio.");
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código del producto es obligatorio.");
            if (precioCompra < 0)
                throw new ArgumentException("El precio de compra no puede ser negativo.");
            if (precioVenta < 0)
                throw new ArgumentException("El precio de venta no puede ser negativo.");
            if (stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.");

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                string query = "INSERT INTO TBLPRODUCTO (StrNombre, StrCodigo, NumPrecioCompra, NumPrecioVenta, IdCategoria, NumStock) " +
                               "VALUES (@nom, @cod, @pc, @pv, 1, @stock)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@nom", nombre);
                cmd.Parameters.AddWithValue("@cod", codigo);
                cmd.Parameters.AddWithValue("@pc", precioCompra);
                cmd.Parameters.AddWithValue("@pv", precioVenta);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Actualiza los datos de un producto existente con validación.
        /// </summary>
        public void Actualizar(int id, string nombre, string codigo, double precioCompra, double precioVenta, int stock)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del producto es obligatorio.");
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código del producto es obligatorio.");
            if (precioCompra < 0)
                throw new ArgumentException("El precio de compra no puede ser negativo.");
            if (precioVenta < 0)
                throw new ArgumentException("El precio de venta no puede ser negativo.");
            if (stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.");

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                string query = "UPDATE TBLPRODUCTO SET StrNombre=@nom, StrCodigo=@cod, NumPrecioCompra=@pc, " +
                               "NumPrecioVenta=@pv, NumStock=@stock WHERE IdProducto=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nom", nombre);
                cmd.Parameters.AddWithValue("@cod", codigo);
                cmd.Parameters.AddWithValue("@pc", precioCompra);
                cmd.Parameters.AddWithValue("@pv", precioVenta);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Elimina un producto por su ID.
        /// </summary>
        public void Eliminar(int id)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM TBLPRODUCTO WHERE IdProducto = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
