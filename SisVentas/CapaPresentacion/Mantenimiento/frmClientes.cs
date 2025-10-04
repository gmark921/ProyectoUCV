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
    public partial class frmClientes : Form
    {
        public frmClientes()
        {
            InitializeComponent();
        }

        private void frmClientes_Load(object sender, EventArgs e)
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
                if (columna.Visible == true && !string.IsNullOrEmpty(columna.Name))
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

            // Cargar búsqueda y clientes...
            CargarClientes();
        }
        private void CargarClientes()
        {
            dgvData.Rows.Clear();
            List<Cliente> lista = new CN_Cliente().Listar();
            foreach (Cliente item in lista)
            {
                dgvData.Rows.Add(new object[] {
                    item.Id,
                    item.Documento,
                    item.NombreCompleto,
                    item.Correo,
                    item.Telefono,
                    item.Estado == true ? 1 : 0,
                    item.Estado == true ? "Activo" : "No Activo"
                });
            }
        }
        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int indice = e.RowIndex;
                DataGridViewRow row = dgvData.Rows[indice];

                // Rellenamos los campos de texto con los datos de la fila seleccionada
                txtIndice.Text = indice.ToString();
                txtId.Text = row.Cells["Id"].Value.ToString();
                txtDocumento.Text = row.Cells["Documento"].Value.ToString();
                txtNombreCompleto.Text = row.Cells["Nombre"].Value.ToString();
                txtCorreo.Text = row.Cells["Correo"].Value.ToString();
                txtTelefono.Text = row.Cells["Telefono"].Value.ToString();


                //txtDescripcion.Text = dgvData.Rows[indice].Cells["Descripcion"].Value.ToString(); //Por ahora lo no lo editare
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
            Cliente obj = new Cliente()
            {
                Id = txtId.Text,
                Documento = txtDocumento.Text,
                NombreCompleto = txtNombreCompleto.Text,
                Correo = txtCorreo.Text,
                Telefono = txtTelefono.Text,
                Estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1
            };

            if (string.IsNullOrEmpty(obj.Id))
            {
                string nuevoId = new CN_Cliente().Registrar(obj, out mensaje);
                if (!string.IsNullOrEmpty(nuevoId) && nuevoId != "0")
                {
                    CargarClientes();
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                bool resultado = new CN_Cliente().Editar(obj, out mensaje);
                if (resultado)
                {
                    CargarClientes();
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
                if (MessageBox.Show("¿Desea eliminar la cliente?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    bool resultado = new CN_Cliente().Eliminar(txtId.Text, out mensaje);

                    if (resultado)
                    {
                        CargarClientes();
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
            txtDocumento.Text = "";
            txtNombreCompleto.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            cboEstado.SelectedIndex = 0;
            txtDocumento.Select();
        }
    }
}
