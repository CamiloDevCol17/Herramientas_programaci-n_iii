using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PRACTICA_Saber_3_Camilo_Valderrama.Logica;

namespace PRACTICA_Saber_4_Camilo_Valderrama.Formularios
{
    /// <summary>
    /// Formulario para registrar y editar clientes.
    /// Si se pasa un ID, carga los datos del cliente para edición.
    /// Si no se pasa ID, prepara el formulario para crear un nuevo cliente.
    /// </summary>
    public partial class frmCliente : Form
    {
        private TextBox txtNombre, txtDocumento, txtDireccion, txtTelefono, txtEmail, txtCiudad;
        private Button btnGuardar, btnCancelar;
        private Label lblTitulo;
        private LogicaClientes logica = new LogicaClientes();
        private int? idClienteEditar = null;

        /// <summary>
        /// Constructor del formulario.
        /// Parámetro: id (opcional) - Si se proporciona, carga los datos del cliente para editar.
        /// Si es null, prepara un formulario en blanco para registro.
        /// </summary>
        public frmCliente(int? id = null)
        {
            idClienteEditar = id;
            ConfigurarInterfaz();
            if (idClienteEditar != null)
                CargarDatosCliente(idClienteEditar.Value);
        }

        /// <summary>
        /// Configura la interfaz gráfica del formulario.
        /// Crea todos los controles (TextBox, Button, Label) y los añade al formulario.
        /// </summary>
        private void ConfigurarInterfaz()
        {
            this.Text = idClienteEditar == null ? "Registrar Cliente" : "Editar Cliente";
            this.Size = new Size(440, 430);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 248, 255);

            // ── Panel encabezado
            Panel panelHeader = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(30, 90, 160)
            };
            lblTitulo = new Label()
            {
                Text = idClienteEditar == null ? "➕ Registrar Cliente" : "✏️ Editar Cliente",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 13)
            };
            panelHeader.Controls.Add(lblTitulo);
            this.Controls.Add(panelHeader);

            // ── Campos del formulario
            int leftLabel = 30, leftInput = 150, inputWidth = 230, gap = 44, topStart = 75;

            CrearCampo("Nombre: *", ref txtNombre, leftLabel, leftInput, inputWidth, topStart + gap * 0);
            CrearCampo("Documento: *", ref txtDocumento, leftLabel, leftInput, inputWidth, topStart + gap * 1);
            CrearCampo("Dirección:", ref txtDireccion, leftLabel, leftInput, inputWidth, topStart + gap * 2);
            CrearCampo("Teléfono:", ref txtTelefono, leftLabel, leftInput, inputWidth, topStart + gap * 3);
            CrearCampo("Email:", ref txtEmail, leftLabel, leftInput, inputWidth, topStart + gap * 4);
            CrearCampo("Ciudad:", ref txtCiudad, leftLabel, leftInput, inputWidth, topStart + gap * 5);

            // Nota de campos obligatorios
            Label lblNota = new Label()
            {
                Text = "(*) Campos obligatorios",
                Top = topStart + gap * 6, Left = leftLabel,
                Width = 260, ForeColor = Color.FromArgb(130, 80, 30),
                Font = new Font("Segoe UI", 8)
            };
            this.Controls.Add(lblNota);

            // ── Botones
            btnGuardar = new Button()
            {
                Text = idClienteEditar == null ? "💾 Guardar" : "💾 Actualizar",
                Top = 360, Left = 70, Width = 130, Height = 35,
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
                Top = 360, Left = 215, Width = 130, Height = 35,
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
                Width = 115, Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(30, 60, 120)
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
        /// Carga los datos del cliente en los campos para edición.
        /// Parámetro: id - ID del cliente a cargar
        /// </summary>
        private void CargarDatosCliente(int id)
        {
            try
            {
                DataRow? row = logica.ObtenerPorId(id);
                if (row != null)
                {
                    txtNombre.Text = row["StrNombre"].ToString();
                    txtDocumento.Text = row["NumDocumento"].ToString();
                    txtDireccion.Text = row["StrDireccion"].ToString();
                    txtTelefono.Text = row["StrTelefono"].ToString();
                    txtEmail.Text = row["StrEmail"].ToString();
                    txtCiudad.Text = row["StrCiudad"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Valida todos los campos requeridos (nombre, documento, etc.).
        /// Si son válidos, llama a la lógica para guardar o actualizar el cliente.
        /// Si hay errores, muestra un mensaje al usuario y no guarda.
        /// </summary>
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            // Validación en la capa de presentación
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del cliente es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDocumento.Text) || !long.TryParse(txtDocumento.Text, out long documento) || documento <= 0)
            {
                MessageBox.Show("El documento debe ser un número válido mayor a cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDocumento.Focus();
                return;
            }

            try
            {
                if (idClienteEditar == null)
                {
                    logica.Insertar(txtNombre.Text.Trim(), documento,
                        txtDireccion.Text.Trim(), txtTelefono.Text.Trim(),
                        txtEmail.Text.Trim(), txtCiudad.Text.Trim());
                    MessageBox.Show("¡Cliente registrado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    logica.Actualizar(idClienteEditar.Value, txtNombre.Text.Trim(), documento,
                        txtDireccion.Text.Trim(), txtTelefono.Text.Trim(),
                        txtEmail.Text.Trim(), txtCiudad.Text.Trim());
                    MessageBox.Show("¡Cliente actualizado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
