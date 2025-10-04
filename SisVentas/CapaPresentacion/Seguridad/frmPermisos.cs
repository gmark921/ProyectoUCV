using CapaEntidades;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion.Formularios.Seguridad
{
    public partial class frmPermisos : Form
    {
        public frmPermisos()
        {
            InitializeComponent();
        }

        private void frmPermisos_Load(object sender, EventArgs e)
        {
            // Cargar roles en el ComboBox
            List<Rol> listaRol = new CN_Rol().Listar();
            foreach (Rol item in listaRol)
            {
                cboRol.Items.Add(new OpcionCombo() { Valor = item.Id, Texto = item.Descripcion });
            }
            cboRol.DisplayMember = "Texto";
            cboRol.ValueMember = "Valor";
            cboRol.SelectedIndex = -1; // No seleccionar ninguno al inicio

            // Cargar todos los menús posibles en la grilla
            CargarMenus();
        }
        private void CargarMenus()
        {
            dgvData.Rows.Clear();

            // Esta es la lista actualizada que coincide con tu nuevo frmPrincipal
            var menus = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("menuSeguridad", "Seguridad"),
                new KeyValuePair<string, string>("submenuUsuarios", "   - Usuarios"),
                new KeyValuePair<string, string>("submenuPermisos", "   - Permisos"),
               
                new KeyValuePair<string, string>("submenuRoles", "   - Roles"),

                new KeyValuePair<string, string>("menuMantenedor", "Mantenedor"),
                new KeyValuePair<string, string>("submenuCategorias", "   - Categorías"),
                new KeyValuePair<string, string>("submenuProductos", "   - Productos"),
                new KeyValuePair<string, string>("submenuClientes", "   - Clientes"),
                new KeyValuePair<string, string>("submenuProveedores", "   - Proveedores"),
                new KeyValuePair<string, string>("menuVentas", "Ventas"),
                new KeyValuePair<string, string>("menuCompras", "Compras"),
                new KeyValuePair<string, string>("menuReportes", "Reportes"),
                new KeyValuePair<string, string>("submenuReporteVentas", "   - Reporte Ventas"),
                new KeyValuePair<string, string>("submenuReporteCompras", "   - Reporte Compras")
            };

            foreach (var menu in menus)
            {
                dgvData.Rows.Add(new object[] { false, menu.Key, menu.Value });
            }
        }

        private void cboRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRol.SelectedIndex < 0) return;

            string idRol = ((OpcionCombo)cboRol.SelectedItem).Valor.ToString();
            List<Permiso> permisos = new CN_Permiso().Listar(idRol);
            List<string> permisosRol = permisos.Select(p => p.NombreMenu).ToList();

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                string nombreMenu = row.Cells["NombreMenu"].Value.ToString();
                row.Cells["Acceso"].Value = permisosRol.Contains(nombreMenu);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cboRol.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar un rol", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable permisos = new DataTable();
            permisos.Columns.Add("NombreMenu", typeof(string));

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Acceso"].Value))
                {
                    permisos.Rows.Add(row.Cells["NombreMenu"].Value.ToString());
                }
            }

            string idRol = ((OpcionCombo)cboRol.SelectedItem).Valor.ToString();
            string mensaje = string.Empty;
            bool resultado = new CN_Permiso().ActualizarPermisos(idRol, permisos, out mensaje);

            if (resultado)
            {
                MessageBox.Show("Permisos actualizados correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se pudieron actualizar los permisos.\n" + mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Asegurarse de que el clic fue en una fila válida y en la columna del CheckBox
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvData.Columns["Acceso"].Index)
            {
                // Obtener la celda del checkbox
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dgvData.Rows[e.RowIndex].Cells["Acceso"];

                // Invertir el valor actual
                chk.Value = !Convert.ToBoolean(chk.Value);
            }
        }
    }
}
