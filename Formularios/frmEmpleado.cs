using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using PRACTICA_Saber_3_Camilo_Valderrama.Datos;

namespace PRACTICA_Saber_4_Camilo_Valderrama.Formularios
{
    public partial class frmEmpleado : Form
    {
        private TextBox txtNombre, txtDocumento, txtDireccion, txtTelefono, txtEmail;
        private Button btnGuardar;
        private AccesoDatos conexion = new AccesoDatos();
        private int? idEmpleadoEditar = null;

        public frmEmpleado(int? id = null)
        {
            idEmpleadoEditar = id;
            ConfigurarInterfaz();
            if (idEmpleadoEditar != null)
            {
                CargarDatosEmpleado(idEmpleadoEditar.Value);
            }
        }

        private void ConfigurarInterfaz()
        {
            this.Text = idEmpleadoEditar == null ? "Registrar Empleado" : "Actualizar Empleado";
            this.Size = new System.Drawing.Size(400, 360);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label l1 = new Label() { Text = "Nombre:", Top = 30, Left = 30, Width = 80 };
            txtNombre = new TextBox() { Top = 25, Left = 120, Width = 200 };
            this.Controls.Add(l1); this.Controls.Add(txtNombre);

            Label l2 = new Label() { Text = "Documento:", Top = 70, Left = 30, Width = 80 };
            txtDocumento = new TextBox() { Top = 65, Left = 120, Width = 200 };
            this.Controls.Add(l2); this.Controls.Add(txtDocumento);

            Label l3 = new Label() { Text = "Dirección:", Top = 110, Left = 30, Width = 80 };
            txtDireccion = new TextBox() { Top = 105, Left = 120, Width = 200 };
            this.Controls.Add(l3); this.Controls.Add(txtDireccion);

            Label l4 = new Label() { Text = "Teléfono:", Top = 150, Left = 30, Width = 80 };
            txtTelefono = new TextBox() { Top = 145, Left = 120, Width = 200 };
            this.Controls.Add(l4); this.Controls.Add(txtTelefono);

            Label l5 = new Label() { Text = "Email:", Top = 190, Left = 30, Width = 80 };
            txtEmail = new TextBox() { Top = 185, Left = 120, Width = 200 };
            this.Controls.Add(l5); this.Controls.Add(txtEmail);

            btnGuardar = new Button() { Text = idEmpleadoEditar == null ? "Guardar" : "Actualizar", Top = 240, Left = 120, Width = 120 };
            btnGuardar.Click += BtnGuardar_Click;
            this.Controls.Add(btnGuardar);
        }

        private void CargarDatosEmpleado(int id)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM TBLEMPLEADO WHERE IdEmpleado = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtNombre.Text = dr["strNombre"].ToString();
                    txtDocumento.Text = dr["NumDocumento"].ToString();
                    txtDireccion.Text = dr["StrDireccion"].ToString();
                    txtTelefono.Text = dr["StrTelefono"].ToString();
                    txtEmail.Text = dr["StrEmail"].ToString();
                }
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                try
                {
                    con.Open();
                    string query = "";
                    if (idEmpleadoEditar == null)
                    {
                        query = "INSERT INTO TBLEMPLEADO (strNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail, IdRolEmpleado) VALUES (@nom, @doc, @dir, @tel, @email, 1)";
                    }
                    else
                    {
                        query = "UPDATE TBLEMPLEADO SET strNombre=@nom, NumDocumento=@doc, StrDireccion=@dir, StrTelefono=@tel, StrEmail=@email WHERE IdEmpleado=@id";
                    }

                    SqlCommand cmd = new SqlCommand(query, con);
                    if (idEmpleadoEditar != null) cmd.Parameters.AddWithValue("@id", idEmpleadoEditar.Value);
                    cmd.Parameters.AddWithValue("@nom", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@doc", Convert.ToInt64(txtDocumento.Text));
                    cmd.Parameters.AddWithValue("@dir", txtDireccion.Text);
                    cmd.Parameters.AddWithValue("@tel", txtTelefono.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("¡Operación realizada con éxito!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}