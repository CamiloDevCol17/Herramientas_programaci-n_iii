using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using PRACTICA_Saber_3_Camilo_Valderrama.Datos;

namespace PRACTICA_Saber_4_Camilo_Valderrama.Formularios
{
    public partial class frmSeguridad : Form
    {
        private DataGridView dgvUsuarios;
        private TextBox txtId, txtUsuario, txtClave, txtRol;
        private Button btnGuardar, btnActualizar, btnEliminar, btnLimpiar;
        private AccesoDatos conexion = new AccesoDatos();
        private bool cargando = true;

        public frmSeguridad()
        {
            ConfigurarInterfaz();
            CargarUsuarios();
            cargando = false;
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Gestión de Seguridad (Usuarios) - Camilo Valderrama";
            this.Size = new System.Drawing.Size(600, 420);
            this.StartPosition = FormStartPosition.CenterScreen;

            dgvUsuarios = new DataGridView() { Top = 20, Left = 20, Width = 540, Height = 150 };
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.SelectionChanged += DgvUsuarios_SelectionChanged;
            this.Controls.Add(dgvUsuarios);

            Label lblId = new Label() { Text = "ID:", Top = 195, Left = 20, Width = 80 };
            txtId = new TextBox() { Top = 190, Left = 100, Width = 150, ReadOnly = true, BackColor = System.Drawing.Color.LightGray };
            this.Controls.Add(lblId); this.Controls.Add(txtId);

            Label lbl1 = new Label() { Text = "Usuario:", Top = 230, Left = 20, Width = 80 };
            txtUsuario = new TextBox() { Top = 225, Left = 100, Width = 150 };
            this.Controls.Add(lbl1); this.Controls.Add(txtUsuario);

            Label lbl2 = new Label() { Text = "Clave:", Top = 195, Left = 280, Width = 80 };
            txtClave = new TextBox() { Top = 190, Left = 360, Width = 150 };
            this.Controls.Add(lbl2); this.Controls.Add(txtClave);

            Label lbl3 = new Label() { Text = "Rol:", Top = 230, Left = 280, Width = 80 };
            txtRol = new TextBox() { Top = 225, Left = 360, Width = 150 };
            this.Controls.Add(lbl3); this.Controls.Add(txtRol);

            btnGuardar = new Button() { Text = "Registrar", Top = 280, Left = 50, Width = 100 };
            btnGuardar.Click += BtnGuardar_Click;
            this.Controls.Add(btnGuardar);

            btnActualizar = new Button() { Text = "Actualizar", Top = 280, Left = 170, Width = 100 };
            btnActualizar.Click += BtnActualizar_Click;
            this.Controls.Add(btnActualizar);

            btnEliminar = new Button() { Text = "Eliminar", Top = 280, Left = 290, Width = 100 };
            btnEliminar.Click += BtnEliminar_Click;
            this.Controls.Add(btnEliminar);

            btnLimpiar = new Button() { Text = "Limpiar", Top = 280, Left = 410, Width = 100 };
            btnLimpiar.Click += (s, e) => LimpiarCampos();
            this.Controls.Add(btnLimpiar);
        }

        private void CargarUsuarios()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TBLSEGURIDAD", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvUsuarios.DataSource = dt;
                    dgvUsuarios.ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar usuarios: " + ex.Message);
                }
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
            {
                MessageBox.Show("Usuario y clave son obligatorios.");
                return;
            }

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO TBLSEGURIDAD (StrUsuario, StrClave, StrRol) VALUES (@usu, @cla, @rol)", con);
                cmd.Parameters.AddWithValue("@usu", txtUsuario.Text);
                cmd.Parameters.AddWithValue("@cla", txtClave.Text);
                cmd.Parameters.AddWithValue("@rol", txtRol.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Usuario registrado.");
                CargarUsuarios();
                LimpiarCampos();
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text)) return;

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE TBLSEGURIDAD SET StrUsuario=@usu, StrClave=@cla, StrRol=@rol WHERE IdSeguridad=@id", con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                cmd.Parameters.AddWithValue("@usu", txtUsuario.Text);
                cmd.Parameters.AddWithValue("@cla", txtClave.Text);
                cmd.Parameters.AddWithValue("@rol", txtRol.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Usuario actualizado.");
                CargarUsuarios();
                LimpiarCampos();
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text)) return;

            if (MessageBox.Show("¿Eliminar usuario?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM TBLSEGURIDAD WHERE IdSeguridad = @id", con);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Usuario eliminado.");
                    CargarUsuarios();
                    LimpiarCampos();
                }
            }
        }

        private void DgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvUsuarios.SelectedRows[0];
                txtId.Text = row.Cells["IdSeguridad"].Value?.ToString() ?? "";
                txtUsuario.Text = row.Cells["StrUsuario"].Value?.ToString() ?? "";
                txtClave.Text = row.Cells["StrClave"].Value?.ToString() ?? "";
                txtRol.Text = row.Cells["StrRol"].Value?.ToString() ?? "";
            }
        }

        private void LimpiarCampos()
        {
            txtId.Clear(); txtUsuario.Clear(); txtClave.Clear(); txtRol.Clear();
            if (dgvUsuarios.Rows.Count > 0) dgvUsuarios.ClearSelection();
        }
    }
}