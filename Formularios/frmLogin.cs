using System;
using System.Windows.Forms;
using PRACTICA_Saber_3_Camilo_Valderrama.Logica;

namespace PRACTICA_Saber_4_Camilo_Valderrama.Formularios
{
    public partial class frmLogin : Form
    {
        private TextBox txtUsuario, txtClave;
        private Button btnIngresar;
        private LogicaLogin logica = new LogicaLogin();

        public frmLogin()
        {
            ConfigurarLogin();
        }

        private void ConfigurarLogin()
        {
            this.Text = "Login del Sistema - Camilo Valderrama";
            this.Size = new System.Drawing.Size(350, 250);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lbl1 = new Label() { Text = "Usuario:", Top = 40, Left = 30, Width = 80 };
            txtUsuario = new TextBox() { Top = 35, Left = 120, Width = 150 };
            this.Controls.Add(lbl1); this.Controls.Add(txtUsuario);

            Label lbl2 = new Label() { Text = "Contraseña:", Top = 90, Left = 30, Width = 80 };
            txtClave = new TextBox() { Top = 85, Left = 120, Width = 150, PasswordChar = '*' };
            this.Controls.Add(lbl2); this.Controls.Add(txtClave);

            btnIngresar = new Button() { Text = "Ingresar", Top = 140, Left = 120, Width = 100 };
            btnIngresar.Click += BtnIngresar_Click;
            this.Controls.Add(btnIngresar);
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            if (logica.ValidarUsuario(txtUsuario.Text, txtClave.Text))
            {
                MessageBox.Show("¡Bienvenido al sistema!", "Acceso Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                frmListaEmpleados principal = new frmListaEmpleados();
                principal.Show();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Acceso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    }
}