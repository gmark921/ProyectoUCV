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

namespace CapaPresentacion.Formularios.Mantenimiento
{
    public partial class frmProveedores : Form
    {
        public frmProveedores()
        {
            InitializeComponent();
        }

        private void frmProveedores_Load(object sender, EventArgs e)
        {
            // Cargar estado
            cboEstado.Items.Add(new OpcionCombo() { Valor = "1", Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = "0", Texto = "No Activo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            //SE AGREGA ACA
            // Llenar el ComboBox de búsqueda con las columnas VISIBLES
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true && !string.IsNullOrEmpty(columna.Name)  )
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            if (cboBusqueda.Items.Count > 0)
            {
                cboBusqueda.SelectedIndex = 0;
            }
            //SE AGREGA ACA

            // Cargar búsqueda y proveedores...
            CargarProveedores();
        }
        private void CargarProveedores()
        {
            dgvData.Rows.Clear();
            List<Proveedor> lista = new CN_Proveedor().Listar();
            foreach (Proveedor item in lista)
            {
                dgvData.Rows.Add(new object[] {
                    item.Id,
                    item.RazonSocial,
                    item.NombreComercial,
                    item.Correo,
                    item.Telefono,
                    item.Estado == true ? 1 : 0,
                    item.Estado == true ? "Activo" : "No Activo"
                });
            }
        }
        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificamos que se haya hecho clic en una fila válida (no en el encabezado)
            if (e.RowIndex >= 0)
            {
                int indice = e.RowIndex;
                DataGridViewRow row = dgvData.Rows[indice];

                // Rellenamos los campos de texto con los datos de la fila seleccionada
                txtIndice.Text = indice.ToString();
                txtId.Text = row.Cells["Id"].Value.ToString();
                txtRazonSocial.Text = row.Cells["RazonSocial"].Value.ToString();
                txtNombreComercial.Text = row.Cells["NombreComercial"].Value.ToString();
                txtCorreo.Text = row.Cells["Correo"].Value.ToString();
                txtTelefono.Text = row.Cells["Telefono"].Value.ToString();

                // Buscamos y seleccionamos el Estado en el ComboBox
                string valorEstadoSeleccionado = row.Cells["EstadoValor"].Value.ToString();
                foreach (OpcionCombo oc in cboEstado.Items)
                {
                    if (oc.Valor.ToString() == valorEstadoSeleccionado)
                    {
                        cboEstado.SelectedIndex = cboEstado.Items.IndexOf(oc);
                        break;
                    }
                }
            }
        }
        
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            Proveedor obj = new Proveedor()
            {
                Id = txtId.Text,
                RazonSocial = txtRazonSocial.Text,
                NombreComercial = txtNombreComercial.Text,
                Correo = txtCorreo.Text,
                Telefono = txtTelefono.Text,
                Estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1
            };

            if (string.IsNullOrEmpty(obj.Id))
            {
                string nuevoId = new CN_Proveedor().Registrar(obj, out mensaje);
                if (!string.IsNullOrEmpty(nuevoId) && nuevoId != "0")
                {
                    CargarProveedores();
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                bool resultado = new CN_Proveedor().Editar(obj, out mensaje);
                if (resultado)
                {
                    CargarProveedores();
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtId.Text) && txtId.Text != "0")
            {
                if (MessageBox.Show("¿Desea eliminar el Proveedor?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    bool resultado = new CN_Proveedor().Eliminar(txtId.Text, out mensaje);

                    if (resultado)
                    {
                        CargarProveedores();
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();
            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Cells[columnaFiltro].Value != null &&
                        row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
        }

        private void btnLimpiarBuscador_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
        }
        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = "";
            txtRazonSocial.Text = "";
            txtNombreComercial.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            cboEstado.SelectedIndex = 0;
            txtRazonSocial.Select();
        }
        

        
    }
}
