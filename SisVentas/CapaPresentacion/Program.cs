using System;
using System.Windows.Forms;

// Este es el punto de entrada de la aplicación.
namespace CapaPresentacion
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // The application will start with frmLogin.
            Application.Run(new frmLogin());
        }
    }
}

