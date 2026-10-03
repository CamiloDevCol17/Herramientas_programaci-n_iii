using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PRACTICA_Saber_3_Camilo_Valderrama.Logica;

namespace PRACTICA_Saber_4_Camilo_Valderrama.Formularios
{

    public partial class frmListaProductos : Form
    {
        private DataGridView dgvProductos;
        private Button btnNuevo, btnBuscar, btnSalir;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private LogicaProductos logica = new LogicaProductos();

        public frmListaProductos()
        {
            ConfigurarInterfaz();
            CargarDatos();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Lista de Productos - Camilo Valderrama";
            this.Size = new Size(920, 510);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 245, 255);

            // ── Panel superior (título)
            Panel panelTitulo = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.FromArgb(40, 100, 60)
            };
            Label lblTitulo = new Label()
            {
                Text = "📦  GESTIÓN DE PRODUCTOS",
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
                ForeColor = Color.FromArgb(30, 80, 40)
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
                BackColor = Color.FromArgb(40, 150, 80),
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
            dgvProductos = new DataGridView()
            {
                Top = 110, Left = 20, Width = 860, Height = 310,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(200, 220, 205),
                Font = new Font("Segoe UI", 9),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 100, 60);
            dgvProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvProductos.EnableHeadersVisualStyles = false;
            dgvProductos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 248, 240);
            dgvProductos.CellContentClick += DgvProductos_CellContentClick;
            this.Controls.Add(dgvProductos);

            // ── Botones inferiores
            btnNuevo = new Button()
            {
                Text = "➕ Nuevo Producto",
                Top = 435, Left = 200, Width = 155, Height = 32,
                BackColor = Color.FromArgb(34, 160, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Click += (s, e) =>
            {
                frmProducto frm = new frmProducto();
                frm.ShowDialog();
                CargarDatos(txtBuscar.Text);
            };

            btnSalir = new Button()
            {
                Text = "❌ Salir",
                Top = 435, Left = 545, Width = 130, Height = 32,
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
        /// Carga los datos en el grid, opcionalmente filtrando por nombre o código.
        /// </summary>
        public void CargarDatos(string filtro = "")
        {
            try
            {
                DataTable dt = logica.ObtenerTodos(filtro);
                dgvProductos.DataSource = dt;

                // Columna Editar
                if (!dgvProductos.Columns.Contains("Editar"))
                {
                    DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn()
                    {
                        Name = "Editar",
                        HeaderText = "Editar",
                        Text = "✏️ Editar",
                        UseColumnTextForButtonValue = true,
                        Width = 90,
                        FlatStyle = FlatStyle.Flat
                    };
                    dgvProductos.Columns.Add(btnEditar);
                }

                // Columna Borrar
                if (!dgvProductos.Columns.Contains("Borrar"))
                {
                    DataGridViewButtonColumn btnBorrar = new DataGridViewButtonColumn()
                    {
                        Name = "Borrar",
                        HeaderText = "Borrar",
                        Text = "🗑️ Borrar",
                        UseColumnTextForButtonValue = true,
                        Width = 90,
                        FlatStyle = FlatStyle.Flat
                    };
                    dgvProductos.Columns.Add(btnBorrar);
                }

                dgvProductos.Columns["Editar"].DefaultCellStyle.BackColor = Color.FromArgb(40, 130, 200);
                dgvProductos.Columns["Editar"].DefaultCellStyle.ForeColor = Color.White;
                dgvProductos.Columns["Borrar"].DefaultCellStyle.BackColor = Color.FromArgb(190, 50, 50);
                dgvProductos.Columns["Borrar"].DefaultCellStyle.ForeColor = Color.White;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Maneja el clic en los botones Editar y Borrar del grid.
        /// </summary>
        private void DgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int idProducto = Convert.ToInt32(dgvProductos.Rows[e.RowIndex].Cells["IdProducto"].Value);

            if (dgvProductos.Columns[e.ColumnIndex].Name == "Editar")
            {
                frmProducto frm = new frmProducto(idProducto);
                frm.ShowDialog();
                CargarDatos(txtBuscar.Text);
            }
            else if (dgvProductos.Columns[e.ColumnIndex].Name == "Borrar")
            {
                string nombre = dgvProductos.Rows[e.RowIndex].Cells["StrNombre"].Value?.ToString() ?? "";
                if (MessageBox.Show($"¿Desea eliminar el producto \"{nombre}\"?",
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        logica.Eliminar(idProducto);
                        MessageBox.Show("Producto eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
