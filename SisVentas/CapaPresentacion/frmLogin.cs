using CapaEntidades;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Este formulario gestiona el inicio de sesión de los usuarios.
namespace CapaPresentacion
{
    public partial class frmLogin : Form
    {
        // Variable para contar los intentos fallidos
        private int intentosFallidos = 0;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // 1. Buscamos al usuario en la BD de forma optimizada
            Usuario oUsuario = new CN_Usuario().Login(txtDocumento.Text);

            // 2. Verificamos si el usuario existe y si la clave coincide
            if (oUsuario != null && oUsuario.Clave == txtClave.Text)
            {
                // Verificamos si el usuario está activo
                if (oUsuario.Estado == false)
                {
                    MessageBox.Show("El usuario se encuentra inactivo.", "Login Fallido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Si el login es correcto, reseteamos el contador
                intentosFallidos = 0;

                frmPrincipal form = new frmPrincipal(oUsuario);
                form.Show();
                this.Hide();
                form.FormClosing += frm_FormClosing;
            }
            else
            {
                // Si el login falla, incrementamos el contador
                intentosFallidos++;
                int intentosRestantes = 3 - intentosFallidos;

                if (intentosRestantes > 0)
                {
                    MessageBox.Show($"No se encontró el usuario o la contraseña es incorrecta.\nLe quedan {intentosRestantes} intento(s).",
                                    "Login Fallido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Ha excedido el número máximo de intentos permitidos. La aplicación se cerrará.",
                                    "Acceso Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close(); // Cierra la aplicación
                }
            }
        }
        // Event to handle the closing of the main form.
        private void frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Limpia los campos y muestra el formulario de login de nuevo.
            txtDocumento.Text = "";
            txtClave.Text = "";
            this.Show();
        }
    }
}
