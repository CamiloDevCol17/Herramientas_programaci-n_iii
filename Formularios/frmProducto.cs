using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PRACTICA_Saber_3_Camilo_Valderrama.Logica;

namespace PRACTICA_Saber_4_Camilo_Valderrama.Formularios
{
    /// <summary>
    /// Formulario de registro y edición de un Producto (Segunda Entrega).
    /// Se muestra al presionar "Nuevo" o "Editar" desde frmListaProductos.
    /// Contiene validación de campos obligatorios y valores numéricos.
    /// </summary>
    public partial class frmProducto : Form
    {
        private TextBox txtNombre, txtCodigo, txtPrecioCompra, txtPrecioVenta, txtStock;
        private Button btnGuardar, btnCancelar;
        private Label lblTitulo;
        private LogicaProductos logica = new LogicaProductos();
        private int? idProductoEditar = null;

        public frmProducto(int? id = null)
        {
            idProductoEditar = id;
            ConfigurarInterfaz();
            if (idProductoEditar != null)
                CargarDatosProducto(idProductoEditar.Value);
        }

        private void ConfigurarInterfaz()
        {
            this.Text = idProductoEditar == null ? "Registrar Producto" : "Editar Producto";
            this.Size = new Size(440, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 250, 248);

            // ── Panel encabezado
            Panel panelHeader = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(40, 100, 60)
            };
            lblTitulo = new Label()
            {
                Text = idProductoEditar == null ? "➕ Registrar Producto" : "✏️ Editar Producto",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 13)
            };
            panelHeader.Controls.Add(lblTitulo);
            this.Controls.Add(panelHeader);

            // ── Campos del formulario
            int leftLabel = 30, leftInput = 155, inputWidth = 220, gap = 44, topStart = 75;

            CrearCampo("Nombre: *", ref txtNombre, leftLabel, leftInput, inputWidth, topStart + gap * 0);
            CrearCampo("Código: *", ref txtCodigo, leftLabel, leftInput, inputWidth, topStart + gap * 1);
            CrearCampo("Precio Compra: *", ref txtPrecioCompra, leftLabel, leftInput, inputWidth, topStart + gap * 2);
            CrearCampo("Precio Venta: *", ref txtPrecioVenta, leftLabel, leftInput, inputWidth, topStart + gap * 3);
            CrearCampo("Stock: *", ref txtStock, leftLabel, leftInput, inputWidth, topStart + gap * 4);

            // Nota de campos obligatorios
            Label lblNota = new Label()
            {
                Text = "(*) Campos obligatorios — precios y stock deben ser números",
                Top = topStart + gap * 5, Left = leftLabel,
                Width = 380, ForeColor = Color.FromArgb(130, 80, 30),
                Font = new Font("Segoe UI", 8)
            };
            this.Controls.Add(lblNota);

            // ── Botones
            btnGuardar = new Button()
            {
                Text = idProductoEditar == null ? "💾 Guardar" : "💾 Actualizar",
                Top = 330, Left = 65, Width = 135, Height = 35,
                BackColor = Color.FromArgb(34, 160, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            btnCancelar = new Button()
            {
                Text = "❌ Cancelar",
                Top = 330, Left = 215, Width = 135, Height = 35,
                BackColor = Color.FromArgb(190, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += (s, e) => this.Close();

            this.Controls.Add(btnGuardar);
            this.Controls.Add(btnCancelar);
        }

        /// <summary>
        /// Crea un par Label + TextBox y los agrega al formulario.
        /// </summary>
        private void CrearCampo(string etiqueta, ref TextBox txt, int leftLabel, int leftInput, int inputWidth, int top)
        {
            Label lbl = new Label()
            {
                Text = etiqueta, Top = top + 3, Left = leftLabel,
                Width = 120, Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(30, 80, 40)
            };
            txt = new TextBox()
            {
                Top = top, Left = leftInput, Width = inputWidth,
                Font = new Font("Segoe UI", 9),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(lbl);
            this.Controls.Add(txt);
        }

        /// <summary>
        /// Carga los datos del producto en los campos para edición.
        /// </summary>
        private void CargarDatosProducto(int id)
        {
            try
            {
                DataRow? row = logica.ObtenerPorId(id);
                if (row != null)
                {
                    txtNombre.Text = row["StrNombre"].ToString();
                    txtCodigo.Text = row["StrCodigo"].ToString();
                    txtPrecioCompra.Text = row["NumPrecioCompra"].ToString();
                    txtPrecioVenta.Text = row["NumPrecioVenta"].ToString();
                    txtStock.Text = row["NumStock"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Valida los campos y guarda o actualiza el producto usando la capa lógica.
        /// </summary>
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            // Validación en capa de presentación
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del producto es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("El código del producto es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus(); return;
            }
            if (!double.TryParse(txtPrecioCompra.Text, out double precioCompra) || precioCompra < 0)
            {
                MessageBox.Show("El precio de compra debe ser un número mayor o igual a cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioCompra.Focus(); return;
            }
            if (!double.TryParse(txtPrecioVenta.Text, out double precioVenta) || precioVenta < 0)
            {
                MessageBox.Show("El precio de venta debe ser un número mayor o igual a cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioVenta.Focus(); return;
            }
            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("El stock debe ser un número entero mayor o igual a cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus(); return;
            }

            try
            {
                if (idProductoEditar == null)
                {
                    logica.Insertar(txtNombre.Text.Trim(), txtCodigo.Text.Trim(), precioCompra, precioVenta, stock);
                    MessageBox.Show("¡Producto registrado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    logica.Actualizar(idProductoEditar.Value, txtNombre.Text.Trim(), txtCodigo.Text.Trim(), precioCompra, precioVenta, stock);
                    MessageBox.Show("¡Producto actualizado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
