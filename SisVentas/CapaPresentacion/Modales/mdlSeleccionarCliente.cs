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

namespace CapaPresentacion.Formularios.Modales
{
    public partial class mdlSeleccionarCliente : Form
    {
        public Cliente _Cliente { get; set; }

        public mdlSeleccionarCliente()
        {
            InitializeComponent();
        }

        private void mdlSeleccionarCliente_Load(object sender, EventArgs e)
        {
            // Cargar columnas de búsqueda
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible && !string.IsNullOrEmpty(columna.HeaderText))
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;

            // Cargar todos los clientes activos
            List<Cliente> lista = new CN_Cliente().Listar();
            foreach (Cliente item in lista)
            {
                if (item.Estado) // Solo mostrar clientes activos
                {
                    dgvData.Rows.Add(new object[] { item.Id, item.Documento, item.NombreCompleto });
                }
            }
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iCol = e.ColumnIndex;

            if (iRow >= 0 && iCol >= 0)
            {
                _Cliente = new Cliente()
                {
                    Id = dgvData.Rows[iRow].Cells["Id"].Value.ToString(),
                    Documento = dgvData.Rows[iRow].Cells["Documento"].Value.ToString(),
                    NombreCompleto = dgvData.Rows[iRow].Cells["NombreCompleto"].Value.ToString()
                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
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
    }
}
