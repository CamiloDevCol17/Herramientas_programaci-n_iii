using PRACTICA_Saber_4_Camilo_Valderrama.Formularios;

namespace PRACTICA_Saber_3_Camilo_Valderrama
{
    /// <summary>
    /// Clase principal de la aplicación.
    /// Inicializa la aplicación Windows Forms y muestra el formulario de login.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal de la aplicación.
        /// Inicializa la configuración de Windows Forms e inicia la aplicación mostrando frmLogin.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new frmLogin());
        }
    }
}