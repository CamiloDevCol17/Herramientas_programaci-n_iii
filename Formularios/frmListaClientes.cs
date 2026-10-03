using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PRACTICA_Saber_3_Camilo_Valderrama.Logica;

namespace PRACTICA_Saber_4_Camilo_Valderrama.Formularios
{
    /// <summary>
    /// Formulario principal del CRUD Clientes.
    /// Lista los registros y tiene botones: Nuevo, Buscar, Salir.
    /// El DataGridView tiene botones de Editar y Borrar por fila.
    /// </summary>
    public partial class frmListaClientes : Form
    {
        private DataGridView dgvClientes;
        private Button btnNuevo, btnBuscar, btnSalir;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private LogicaClientes logica = new LogicaClientes();

        public frmListaClientes()
        {
            ConfigurarInterfaz();
            CargarDatos();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Lista de Clientes - Camilo Valderrama";
            this.Size = new Size(860, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 245, 255);

            // ── Panel superior (título)
            Panel panelTitulo = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.FromArgb(30, 90, 160)
            };
            Label lblTitulo = new Label()
            {
                Text = "  GESTIÓN DE CLIENTES",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 14)
            };
            panelTitulo.Controls.Add(lblTitulo);
            this.Controls.Add(panelTitulo);

            // ── Barra de búsqueda
            lblBuscar = new Label()
            {
                Text = "Buscar:",
                Top = 75, Left = 25, Width = 55,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(30, 60, 120)
            };
            txtBuscar = new TextBox()
            {
                Top = 72, Left = 85, Width = 280,
                Font = new Font("Segoe UI", 9)
            };
            btnBuscar = new Button()
            {
                Text = "🔍 Buscar",
                Top = 70, Left = 375, Width = 110, Height = 28,
                BackColor = Color.FromArgb(30, 130, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Click += (s, e) => CargarDatos(txtBuscar.Text);
            txtBuscar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) CargarDatos(txtBuscar.Text); };

            this.Controls.Add(lblBuscar);
            this.Controls.Add(txtBuscar);
            this.Controls.Add(btnBuscar);

            // ── DataGridView
            dgvClientes = new DataGridView()
            {
                Top = 110, Left = 20, Width = 800, Height = 300,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(200, 215, 240),
                Font = new Font("Segoe UI", 9),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvClientes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 90, 160);
            dgvClientes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvClientes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvClientes.EnableHeadersVisualStyles = false;
            dgvClientes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 242, 255);
            dgvClientes.CellContentClick += DgvClientes_CellContentClick;
            this.Controls.Add(dgvClientes);

            // ── Botones inferiores
            btnNuevo = new Button()
            {
                Text = "➕ Nuevo Cliente",
                Top = 425, Left = 180, Width = 145, Height = 32,
                BackColor = Color.FromArgb(34, 160, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Click += (s, e) =>
            {
                frmCliente frm = new frmCliente();
                frm.ShowDialog();
                CargarDatos(txtBuscar.Text);
            };

            btnSalir = new Button()
            {
                Text = "❌ Salir",
                Top = 425, Left = 500, Width = 130, Height = 32,
                BackColor = Color.FromArgb(190, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.Click += (s, e) => this.Close();

            this.Controls.Add(btnNuevo);
            this.Controls.Add(btnSalir);
        }

        /// <summary>
        /// Carga los datos en el grid, opcionalmente filtrando.
        /// Agrega columnas de Editar y Borrar después de cargar.
        /// </summary>
        public void CargarDatos(string filtro = "")
        {
            try
            {
                DataTable dt = logica.ObtenerTodos(filtro);
                dgvClientes.DataSource = dt;

                // Añadir columna Editar si no existe
                if (!dgvClientes.Columns.Contains("Editar"))
                {
                    DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn()
                    {
                        Name = "Editar",
                        HeaderText = "Editar",
                        Text = "✏️ Editar",
                        UseColumnTextForButtonValue = true,
                        Width = 85,
                        FlatStyle = FlatStyle.Flat
                    };
                    dgvClientes.Columns.Add(btnEditar);
                }

                // Añadir columna Borrar si no existe
                if (!dgvClientes.Columns.Contains("Borrar"))
                {
                    DataGridViewButtonColumn btnBorrar = new DataGridViewButtonColumn()
                    {
                        Name = "Borrar",
                        HeaderText = "Borrar",
                        Text = "🗑️ Borrar",
                        UseColumnTextForButtonValue = true,
                        Width = 85,
                        FlatStyle = FlatStyle.Flat
                    };
                    dgvClientes.Columns.Add(btnBorrar);
                }

                // Estilo para botones de acción
                dgvClientes.Columns["Editar"].DefaultCellStyle.BackColor = Color.FromArgb(30, 130, 200);
                dgvClientes.Columns["Editar"].DefaultCellStyle.ForeColor = Color.White;
                dgvClientes.Columns["Borrar"].DefaultCellStyle.BackColor = Color.FromArgb(190, 50, 50);
                dgvClientes.Columns["Borrar"].DefaultCellStyle.ForeColor = Color.White;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int idCliente = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells["IdCliente"].Value);

            if (dgvClientes.Columns[e.ColumnIndex].Name == "Editar")
            {
                frmCliente frm = new frmCliente(idCliente);
                frm.ShowDialog();
                CargarDatos(txtBuscar.Text);
            }
            else if (dgvClientes.Columns[e.ColumnIndex].Name == "Borrar")
            {
                string nombre = dgvClientes.Rows[e.RowIndex].Cells["StrNombre"].Value?.ToString() ?? "";
                if (MessageBox.Show($"¿Desea eliminar al cliente \"{nombre}\"?",
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        logica.Eliminar(idCliente);
                        MessageBox.Show("Cliente eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarDatos(txtBuscar.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
