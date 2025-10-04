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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CapaPresentacion
{
    public partial class frmPrincipal : Form
    {
        private static Usuario _UsuarioActual;
        private static ToolStripMenuItem _MenuActivo = null;
        private static Form _FormularioActivo = null;

        public frmPrincipal(Usuario objUsuario)
        {
            _UsuarioActual = objUsuario;
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            /*// Display the current user's name.
            lblUsuarioConectado.Text = "👤Usuario: " + _UsuarioActual.NombreCompleto;

            // Load menu permissions.
            var listaPermisos = new CN_Permiso().Listar(_UsuarioActual.oRol.Id);

            // Get all menus.
            var menus = menu.Items.OfType<ToolStripMenuItem>().ToList();

            foreach (ToolStripMenuItem menu in menus)
            {
                bool encontrado = listaPermisos.Any(p => p.NombreMenu == menu.Name);

                if (!encontrado && menu.DropDownItems.Count > 0)
                {
                    // If the main menu is not found, check its submenus.
                    foreach (ToolStripMenuItem submenu in menu.DropDownItems.OfType<ToolStripMenuItem>())
                    {
                        if (listaPermisos.Any(p => p.NombreMenu == submenu.Name))
                        {
                            encontrado = true;
                            break;
                        }
                    }
                }

                // Set menu visibility based on permissions.
                menu.Visible = encontrado;
            }*/
            // Muestra el nombre del usuario conectado (esto no cambia)
            lblUsuarioConectado.Text = "👤Usuario: " + _UsuarioActual.NombreCompleto;

            // Obtiene la lista de permisos para el rol del usuario (esto no cambia)
            var listaPermisos = new CN_Permiso().Listar(_UsuarioActual.oRol.Id);

            // Obtiene todos los menús de nivel superior (esto no cambia)
            var menus = menu.Items.OfType<ToolStripMenuItem>().ToList();

            // --- INICIA EL CÓDIGO CORREGIDO ---

            // 1. Recorremos cada menú principal (Ej: Seguridad, Reportes, Ventas)
            foreach (ToolStripMenuItem menuPrincipal in menus)
            {
                // 2. Verificamos si este menú tiene sub-elementos (un menú desplegable)
                if (menuPrincipal.DropDownItems.Count > 0)
                {
                    int submenusConPermiso = 0;

                    // 3. Si tiene sub-elementos, recorremos CADA UNO de ellos
                    foreach (ToolStripMenuItem submenu in menuPrincipal.DropDownItems.OfType<ToolStripMenuItem>())
                    {
                        // Verificamos si el usuario tiene permiso para ESTE SUBMENÚ específico
                        bool tienePermiso = listaPermisos.Any(p => p.NombreMenu == submenu.Name);

                        // Asignamos la visibilidad AL SUBMENÚ directamente
                        submenu.Visible = tienePermiso;

                        // Si este submenú se hizo visible, lo contamos
                        if (tienePermiso)
                        {
                            submenusConPermiso++;
                        }
                    }

                    // 4. Finalmente, el MENÚ PRINCIPAL solo será visible si al menos uno de sus hijos tiene permiso
                    menuPrincipal.Visible = submenusConPermiso > 0;
                }
                // 5. Si el menú NO tiene sub-elementos (es un botón directo como "Ventas" o "Compras")
                else
                {
                    // Simplemente verificamos el permiso para ese menú y lo mostramos u ocultamos
                    menuPrincipal.Visible = listaPermisos.Any(p => p.NombreMenu == menuPrincipal.Name);
                }
            }
            // --- TERMINA EL CÓDIGO CORREGIDO ---
        }
        // Method to open child forms within the MDI container.
        private void AbrirFormulario(ToolStripMenuItem menu, Form formulario)
        {
            if (_MenuActivo != null)
            {
                _MenuActivo.BackColor = System.Drawing.Color.White;
            }
            menu.BackColor = System.Drawing.Color.Silver;
            _MenuActivo = menu;

            if (_FormularioActivo != null)
            {
                _FormularioActivo.Close();
            }

            _FormularioActivo = formulario;
            formulario.MdiParent = this;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            formulario.Show();
        }
        // SEGURIDAD 
        private void submenuUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario((ToolStripMenuItem)sender, new Formularios.Mantenimiento.frmUsuarios());
        }
        private void submenuPermisos_Click(object sender, EventArgs e)
        {
            AbrirFormulario((ToolStripMenuItem)sender, new Formularios.Seguridad.frmPermisos());
        }
        private void submenuRoles_Click(object sender, EventArgs e)
        {
            AbrirFormulario((ToolStripMenuItem)sender, new Formularios.Seguridad.frmRoles());
        }
        //MANTENEDOR
        private void submenuCategorias_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario((ToolStripMenuItem)sender, new Formularios.Mantenimiento.frmCategorias());
        }

        private void submenuProductos_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario((ToolStripMenuItem)sender, new Formularios.Mantenimiento.frmProductos());
        }

        private void submenuClientes_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario((ToolStripMenuItem)sender, new Formularios.Mantenimiento.frmClientes());
        }

        private void submenuProveedores_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario((ToolStripMenuItem)sender, new Formularios.Mantenimiento.frmProveedores());
        }
        // Menu click events
        private void menuVentas_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario((ToolStripMenuItem)sender, new Formularios.Procesos.frmRegistrarVenta(_UsuarioActual));
        }

        private void menuCompras_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario((ToolStripMenuItem)sender, new Formularios.Procesos.frmRegistrarCompra(_UsuarioActual));
        }

        private void submenuReporteVentas_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario((ToolStripMenuItem)sender, new Formularios.Reportes.frmReporteVentas());
        }

        private void submenuReporteCompras_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario((ToolStripMenuItem)sender, new Formularios.Reportes.frmReporteCompras());
        }

    }
}
