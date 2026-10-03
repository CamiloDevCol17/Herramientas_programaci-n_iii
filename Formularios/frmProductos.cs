using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using PRACTICA_Saber_3_Camilo_Valderrama.Datos;

namespace PRACTICA_Saber_4_Camilo_Valderrama.Formularios
{
    public partial class frmProductos : Form
    {
        private DataGridView dgvProductos;
        private TextBox txtId, txtNombre, txtCodigo, txtPrecioC, txtPrecioV, txtStock;
        private Button btnGuardar, btnActualizar, btnEliminar, btnLimpiar;
        private AccesoDatos conexion = new AccesoDatos();

        public frmProductos()
        {
            ConfigurarInterfazProductos();
            CargarProductos();
        }

        private void ConfigurarInterfazProductos()
        {
            this.Text = "Gestión Real de Productos - Camilo Valderrama";
            this.Size = new System.Drawing.Size(720, 500);

            dgvProductos = new DataGridView() { Top = 20, Left = 20, Width = 650, Height = 180 };
            dgvProductos.ReadOnly = true;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.SelectionChanged += DgvProductos_SelectionChanged;
            this.Controls.Add(dgvProductos);

            Label lblId = new Label() { Text = "ID Producto:", Top = 220, Left = 20, Width = 80 };
            txtId = new TextBox() { Top = 215, Left = 110, Width = 150, ReadOnly = true, BackColor = System.Drawing.Color.LightGray };
            this.Controls.Add(lblId); this.Controls.Add(txtId);

            Label lbl1 = new Label() { Text = "Nombre:", Top = 255, Left = 20, Width = 80 };
            txtNombre = new TextBox() { Top = 250, Left = 110, Width = 150 };
            this.Controls.Add(lbl1); this.Controls.Add(txtNombre);

            Label lbl2 = new Label() { Text = "Código:", Top = 290, Left = 20, Width = 80 };
            txtCodigo = new TextBox() { Top = 285, Left = 110, Width = 150 };
            this.Controls.Add(lbl2); this.Controls.Add(txtCodigo);

            Label lbl3 = new Label() { Text = "P. Compra:", Top = 325, Left = 20, Width = 80 };
            txtPrecioC = new TextBox() { Top = 320, Left = 110, Width = 150 };
            this.Controls.Add(lbl3); this.Controls.Add(txtPrecioC);

            Label lbl4 = new Label() { Text = "P. Venta:", Top = 255, Left = 360, Width = 80 };
            txtPrecioV = new TextBox() { Top = 250, Left = 440, Width = 150 };
            this.Controls.Add(lbl4); this.Controls.Add(txtPrecioV);

            Label lbl5 = new Label() { Text = "Stock:", Top = 290, Left = 360, Width = 80 };
            txtStock = new TextBox() { Top = 285, Left = 440, Width = 150 };
            this.Controls.Add(lbl5); this.Controls.Add(txtStock);

            btnGuardar = new Button() { Text = "Registrar Nuevo", Top = 380, Left = 80, Width = 120 };
            btnGuardar.Click += BtnGuardar_Click;
            this.Controls.Add(btnGuardar);

            btnActualizar = new Button() { Text = "Actualizar", Top = 380, Left = 215, Width = 110 };
            btnActualizar.Click += BtnActualizar_Click;
            this.Controls.Add(btnActualizar);

            btnEliminar = new Button() { Text = "Eliminar", Top = 380, Left = 340, Width = 110 };
            btnEliminar.Click += BtnEliminar_Click;
            this.Controls.Add(btnEliminar);

            btnLimpiar = new Button() { Text = "Limpiar Campos", Top = 380, Left = 465, Width = 120 };
            btnLimpiar.Click += (s, e) => LimpiarCampos();
            this.Controls.Add(btnLimpiar);
        }

        private void CargarProductos()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TBLPRODUCTO", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvProductos.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar productos: " + ex.Message);
                }
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Por favor complete al menos el nombre y el código del producto.");
                return;
            }

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO TBLPRODUCTO (StrNombre, StrCodigo, NumPrecioCompra, NumPrecioVenta, IdCategoria, NumStock) VALUES (@nombre, @codigo, @pc, @pv, 1, @stock)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text);
                    cmd.Parameters.AddWithValue("@pc", Convert.ToDouble(txtPrecioC.Text));
                    cmd.Parameters.AddWithValue("@pv", Convert.ToDouble(txtPrecioV.Text));
                    cmd.Parameters.AddWithValue("@stock", Convert.ToInt32(txtStock.Text));

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("¡Producto registrado con éxito!");
                    CargarProductos();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar: " + ex.Message);
                }
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Por favor seleccione un producto de la tabla para actualizar.");
                return;
            }

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                try
                {
                    con.Open();
                    string query = "UPDATE TBLPRODUCTO SET StrNombre=@nombre, StrCodigo=@codigo, NumPrecioCompra=@pc, NumPrecioVenta=@pv, NumStock=@stock WHERE IdProducto=@id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text);
                    cmd.Parameters.AddWithValue("@pc", Convert.ToDouble(txtPrecioC.Text));
                    cmd.Parameters.AddWithValue("@pv", Convert.ToDouble(txtPrecioV.Text));
                    cmd.Parameters.AddWithValue("@stock", Convert.ToInt32(txtStock.Text));

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("¡Producto actualizado correctamente!");
                    CargarProductos();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar: " + ex.Message);
                }
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Por favor seleccione o indique el ID del producto que desea eliminar.");
                return;
            }

            DialogResult resultado = MessageBox.Show($"¿Desea eliminar el producto con ID {txtId.Text}?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (resultado == DialogResult.Yes)
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    try
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM TBLPRODUCTO WHERE IdProducto = @id", con);
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("¡Producto eliminado exitosamente!");
                        CargarProductos();
                        LimpiarCampos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar: " + ex.Message);
                    }
                }
            }
        }

        private void DgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvProductos.SelectedRows[0];
                txtId.Text = row.Cells["IdProducto"].Value.ToString();
                txtNombre.Text = row.Cells["StrNombre"].Value.ToString();
                txtCodigo.Text = row.Cells["StrCodigo"].Value.ToString();
                txtPrecioC.Text = row.Cells["NumPrecioCompra"].Value.ToString();
                txtPrecioV.Text = row.Cells["NumPrecioVenta"].Value.ToString();
                txtStock.Text = row.Cells["NumStock"].Value.ToString();
            }
        }

        private void LimpiarCampos()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtCodigo.Clear();
            txtPrecioC.Clear();
            txtPrecioV.Clear();
            txtStock.Clear();
            if (dgvProductos.Rows.Count > 0) dgvProductos.ClearSelection();
        }
    }
}