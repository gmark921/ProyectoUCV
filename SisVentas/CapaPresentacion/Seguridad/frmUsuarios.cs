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
        public partial class frmUsuarios : Form
        {
            public frmUsuarios()
            {
                InitializeComponent();
            }

            private void frmUsuarios_Load(object sender, EventArgs e)
            {
                // ESTADO
                cboEstado.Items.Add(new OpcionCombo() { Valor = "1", Texto = "Activo" });
                cboEstado.Items.Add(new OpcionCombo() { Valor = "0", Texto = "No Activo" });
                cboEstado.DisplayMember = "Texto";
                cboEstado.ValueMember = "Valor";
                cboEstado.SelectedIndex = 0;

                // ROL
                List<Rol> listaRol = new CN_Rol().Listar();
                foreach (Rol item in listaRol)
                {
                    cboRol.Items.Add(new OpcionCombo() { Valor = item.Id, Texto = item.Descripcion });
                }
                cboRol.DisplayMember = "Texto";
                cboRol.ValueMember = "Valor";
                if (cboRol.Items.Count > 0)
                {
                    cboRol.SelectedIndex = 0;
                }

                dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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

                CargarUsuarios();
            }

            private void CargarUsuarios()
            {
                dgvData.Rows.Clear();
                List<Usuario> listaUsuario = new CN_Usuario().Listar();

                foreach (Usuario item in listaUsuario)
                {
                    dgvData.Rows.Add(new object[] {
                        item.Id,
                        item.Documento,
                        item.NombreCompleto,
                        item.Correo,
                        item.Clave,
                        item.oRol.Id,
                        item.oRol.Descripcion, // Esta es la columna "Rol"
                        item.Estado == true ? 1 : 0, // Columna "EstadoValor"
                        item.Estado == true ? "Activo" : "No Activo" // Columna "Estado"
                    });
                }
            }

            private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                int indice = e.RowIndex;
                if (indice >= 0)
                {
                    txtId.Text = dgvData.Rows[indice].Cells["Id"].Value.ToString();
                    txtDocumento.Text = dgvData.Rows[indice].Cells["Documento"].Value.ToString();
                    txtNombreCompleto.Text = dgvData.Rows[indice].Cells["NombreCompleto"].Value.ToString();
                    txtCorreo.Text = dgvData.Rows[indice].Cells["Correo"].Value.ToString();
                    txtClave.Text = dgvData.Rows[indice].Cells["Clave"].Value.ToString();
                    txtConfirmarClave.Text = dgvData.Rows[indice].Cells["Clave"].Value.ToString();

                    // Buscar el Rol en el ComboBox
                    string idRolSeleccionado = dgvData.Rows[indice].Cells["IdRol"].Value.ToString();

                    foreach (OpcionCombo oc in cboRol.Items)
                    {
                        if (oc.Valor.ToString() == idRolSeleccionado)
                        {
                            cboRol.SelectedIndex = cboRol.Items.IndexOf(oc);
                            break;
                        }
                    }

                    // Buscar el Estado en el ComboBox
                    string valorEstadoSeleccionado = dgvData.Rows[indice].Cells["EstadoValor"].Value.ToString();
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

                Usuario objusuario = new Usuario()
                {
                    Id = txtId.Text,
                    Documento = txtDocumento.Text,
                    NombreCompleto = txtNombreCompleto.Text,
                    Correo = txtCorreo.Text,
                    Clave = txtClave.Text,
                    ConfirmarClave = txtConfirmarClave.Text,
                    oRol = new Rol() { Id = ((OpcionCombo)cboRol.SelectedItem).Valor.ToString() },
                    Estado = ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString() == "1"
                };

                if (string.IsNullOrEmpty(objusuario.Id)) // Lógica para un NUEVO usuario
                {
                    string idusuariogenerado = new CN_Usuario().Registrar(objusuario, out mensaje);

                    if (idusuariogenerado != "0")
                    {
                        CargarUsuarios(); // Recargamos la tabla para mostrar el nuevo usuario
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Errores de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else // Lógica para EDITAR un usuario existente
                {
                    bool resultado = new CN_Usuario().Editar(objusuario, out mensaje);

                    if (resultado)
                    {
                        CargarUsuarios(); // Recargamos la tabla para mostrar los cambios
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
                    if (MessageBox.Show("¿Desea eliminar el usuario?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string mensaje = string.Empty;
                        bool resultado = new CN_Usuario().Eliminar(txtId.Text, out mensaje);

                        if (resultado)
                        {
                            CargarUsuarios();
                            Limpiar();
                        }
                        else
                        {
                            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            private void btnBuscar_Click(object sender, EventArgs e)
            {
                string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();
                if (dgvData.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in dgvData.Rows)
                    {
                        // Asegurarse de que el valor no sea nulo antes de buscar
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
            private void btnLimpiar_Click(object sender, EventArgs e)
            {
                Limpiar();
            }
            private void Limpiar()
            {
                txtId.Text = "";
                txtDocumento.Text = "";
                txtNombreCompleto.Text = "";
                txtCorreo.Text = "";
                txtClave.Text = "";
                txtConfirmarClave.Text = "";
                if (cboRol.Items.Count > 0) cboRol.SelectedIndex = 0;
                if (cboEstado.Items.Count > 0) cboEstado.SelectedIndex = 0;
                txtDocumento.Select();
            }
        }
    }
