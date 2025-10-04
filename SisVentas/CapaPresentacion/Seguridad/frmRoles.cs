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

namespace CapaPresentacion.Formularios.Seguridad
{
    public partial class frmRoles : Form
    {
        public frmRoles()
        {
            InitializeComponent();
        }

        private void frmRoles_Load(object sender, EventArgs e)
        {
            // Configuración inicial del DataGridView
            dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // Cargamos los datos iniciales
            CargarRoles();
        }
        private void CargarRoles()
        {
            dgvData.Rows.Clear();
            List<Rol> listaRoles = new CN_Rol().Listar();

            foreach (Rol item in listaRoles)
            {
                dgvData.Rows.Add(new object[] {
                    item.Id,
                    item.Descripcion
                });
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indice = e.RowIndex;
            if (indice >= 0)
            {
                // Pasamos los datos de la fila seleccionada a los campos del formulario
                txtId.Text = dgvData.Rows[indice].Cells["Id"].Value?.ToString() ?? "";
                txtDescripcion.Text = dgvData.Rows[indice].Cells["Descripcion"].Value?.ToString() ?? "";
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Rol objRol = new Rol()
            {
                Id = txtId.Text,
                Descripcion = txtDescripcion.Text,
            };

            // Lógica para un NUEVO rol
            if (string.IsNullOrEmpty(objRol.Id))
            {
                string idRolGenerado = new CN_Rol().Registrar(objRol, out mensaje);

                if (idRolGenerado != "0")
                {
                    CargarRoles(); // Recargamos la tabla para mostrar el nuevo rol
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // Lógica para EDITAR un rol existente
            else
            {
                bool resultado = new CN_Rol().Editar(objRol, out mensaje);

                if (resultado)
                {
                    CargarRoles(); // Recargamos la tabla para mostrar los cambios
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error al Editar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtId.Text))
            {
                if (MessageBox.Show("¿Desea eliminar el rol?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    bool resultado = new CN_Rol().Eliminar(txtId.Text, out mensaje);

                    if (resultado)
                    {
                        CargarRoles();
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
        private void Limpiar()
        {
            txtId.Text = "";
            txtDescripcion.Text = "";
            txtDescripcion.Select(); // Pone el foco en el campo de descripción
        }
    }
}
