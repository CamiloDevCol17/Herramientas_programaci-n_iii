using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using PRACTICA_Saber_3_Camilo_Valderrama.Datos;

namespace PRACTICA_Saber_4_Camilo_Valderrama.Formularios
{
    public partial class frmListaEmpleados : Form
    {
        private DataGridView dgvLista;
        private Button btnNuevo, btnEditar, btnEliminar, btnSeguridad, btnClientes, btnProductos;
        private AccesoDatos conexion = new AccesoDatos();

        public frmListaEmpleados()
        {
            ConfigurarInterfaz();
            CargarDatos();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Sistema de Gestión - Camilo Valderrama";
            this.Size = new System.Drawing.Size(850, 510);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(240, 245, 255);

            // Panel título
            System.Windows.Forms.Panel panelTitulo = new System.Windows.Forms.Panel()
            {
                Dock = DockStyle.Top, Height = 55,
                BackColor = System.Drawing.Color.FromArgb(30, 90, 160)
            };
            System.Windows.Forms.Label lblTitulo = new System.Windows.Forms.Label()
            {
                Text = "👥  GESTIÓN DE EMPLEADOS",
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                AutoSize = true, Location = new System.Drawing.Point(20, 14)
            };
            panelTitulo.Controls.Add(lblTitulo);
            this.Controls.Add(panelTitulo);

            dgvLista = new DataGridView() { Top = 75, Left = 20, Width = 790, Height = 300 };
            dgvLista.ReadOnly = true;
            dgvLista.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLista.BackgroundColor = System.Drawing.Color.White;
            dgvLista.BorderStyle = BorderStyle.None;
            dgvLista.Font = new System.Drawing.Font("Segoe UI", 9);
            dgvLista.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 90, 160);
            dgvLista.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvLista.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);
            dgvLista.EnableHeadersVisualStyles = false;
            dgvLista.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(235, 242, 255);
            this.Controls.Add(dgvLista);

            // ── Fila de botones de empleados
            btnNuevo = new Button()
            {
                Text = "➕ Nuevo", Top = 400, Left = 30, Width = 110, Height = 32,
                BackColor = System.Drawing.Color.FromArgb(34, 160, 80),
                ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Click += (s, e) => { frmEmpleado formReg = new frmEmpleado(); formReg.ShowDialog(); CargarDatos(); };
            this.Controls.Add(btnNuevo);

            btnEditar = new Button()
            {
                Text = "✏️ Editar", Top = 400, Left = 150, Width = 110, Height = 32,
                BackColor = System.Drawing.Color.FromArgb(30, 130, 200),
                ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Click += (s, e) => {
                if (dgvLista.SelectedRows.Count > 0)
                {
                    int id = Convert.ToInt32(dgvLista.SelectedRows[0].Cells["IdEmpleado"].Value);
                    frmEmpleado formReg = new frmEmpleado(id);
                    formReg.ShowDialog(); CargarDatos();
                }
                else { MessageBox.Show("Seleccione un empleado para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            };
            this.Controls.Add(btnEditar);

            btnEliminar = new Button()
            {
                Text = "🗑️ Eliminar", Top = 400, Left = 270, Width = 110, Height = 32,
                BackColor = System.Drawing.Color.FromArgb(190, 50, 50),
                ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += (s, e) => {
                if (dgvLista.SelectedRows.Count > 0)
                {
                    int id = Convert.ToInt32(dgvLista.SelectedRows[0].Cells["IdEmpleado"].Value);
                    if (MessageBox.Show("¿Desea eliminar este empleado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    { EliminarEmpleado(id); CargarDatos(); }
                }
                else { MessageBox.Show("Seleccione un empleado para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            };
            this.Controls.Add(btnEliminar);

            // ── Botón Seguridad
            btnSeguridad = new Button()
            {
                Text = "🔒 Seguridad", Top = 400, Left = 390, Width = 120, Height = 32,
                BackColor = System.Drawing.Color.FromArgb(100, 70, 160),
                ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnSeguridad.FlatAppearance.BorderSize = 0;
            btnSeguridad.Click += (s, e) => { frmSeguridad formSeg = new frmSeguridad(); formSeg.ShowDialog(); };
            this.Controls.Add(btnSeguridad);

            // ── Botón Clientes (Segunda Entrega)
            btnClientes = new Button()
            {
                Text = "🧑‍💼 Clientes", Top = 400, Left = 520, Width = 130, Height = 32,
                BackColor = System.Drawing.Color.FromArgb(30, 90, 160),
                ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnClientes.FlatAppearance.BorderSize = 0;
            btnClientes.Click += (s, e) => { frmListaClientes frmCli = new frmListaClientes(); frmCli.ShowDialog(); };
            this.Controls.Add(btnClientes);

            // ── Botón Productos (Segunda Entrega)
            btnProductos = new Button()
            {
                Text = "📦 Productos", Top = 400, Left = 660, Width = 130, Height = 32,
                BackColor = System.Drawing.Color.FromArgb(40, 100, 60),
                ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnProductos.FlatAppearance.BorderSize = 0;
            btnProductos.Click += (s, e) => { frmListaProductos frmProd = new frmListaProductos(); frmProd.ShowDialog(); };
            this.Controls.Add(btnProductos);
        }

        public void CargarDatos()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TBLEMPLEADO", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvLista.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar: " + ex.Message);
                }
            }
        }

        private void EliminarEmpleado(int id)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM TBLEMPLEADO WHERE IdEmpleado = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Empleado eliminado con éxito.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message);
                }
            }
        }
    }
}