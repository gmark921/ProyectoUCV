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
    public partial class frmCategorias : Form
    {
        public frmCategorias()
        {
            InitializeComponent();
        }

        private void frmCategorias_Load(object sender, EventArgs e)
        {
            cboEstado.Items.Add(new OpcionCombo() { Valor = "1", Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = "0", Texto = "No Activo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true && !string.IsNullOrEmpty(columna.Name))
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;

            CargarCategorias();
        }
        private void CargarCategorias()
        {
            dgvData.Rows.Clear();
            List<Categoria> lista = new CN_Categoria().Listar();
            foreach (Categoria item in lista)
            {
                dgvData.Rows.Add(new object[] {
                    item.Id,
                    item.Descripcion,
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

                txtIndice.Text = indice.ToString();
                txtId.Text = dgvData.Rows[indice].Cells["Id"].Value.ToString();
                txtDescripcion.Text = dgvData.Rows[indice].Cells["Descripcion"].Value.ToString();

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
            Categoria obj = new Categoria()
            {
                Id = txtId.Text,
                Descripcion = txtDescripcion.Text,
                Estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1
            };

            if (string.IsNullOrEmpty(obj.Id))
            {
                string nuevoId = new CN_Categoria().Registrar(obj, out mensaje);
                if (!string.IsNullOrEmpty(nuevoId) && nuevoId != "0")
                {
                    CargarCategorias();
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                bool resultado = new CN_Categoria().Editar(obj, out mensaje);
                if (resultado)
                {
                    CargarCategorias();
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
            if (!string.IsNullOrEmpty(txtId.Text))
            {
                if (MessageBox.Show("¿Desea eliminar la categoría?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    bool resultado = new CN_Categoria().Eliminar(txtId.Text, out mensaje);

                    if (resultado)
                    {
                        CargarCategorias();
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
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
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
            txtDescripcion.Text = "";
            cboEstado.SelectedIndex = 0;
            txtDescripcion.Select();
        }
    }
}
