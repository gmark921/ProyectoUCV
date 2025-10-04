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
    public partial class frmProductos : Form
    {
        public frmProductos()
        {
            InitializeComponent();
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            // --- Configuración de ComboBoxes (esto se mantiene igual) ---
            cboEstado.Items.Clear();
            cboEstado.Items.Add(new OpcionCombo() { Valor = "1", Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = "0", Texto = "No Activo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            cboCategoria.Items.Clear();
            List<Categoria> listaCategoria = new CN_Categoria().Listar();
            foreach (Categoria item in listaCategoria)
            {
                cboCategoria.Items.Add(new OpcionCombo() { Valor = item.Id, Texto = item.Descripcion });
            }
            cboCategoria.DisplayMember = "Texto";
            cboCategoria.ValueMember = "Valor";
            if (cboCategoria.Items.Count > 0) cboCategoria.SelectedIndex = 0;

            cboUnidadBase.Items.Clear();
            cboUnidadBase.Items.Add(new OpcionCombo() { Valor = "Unidades", Texto = "Unidades" });
            cboUnidadBase.Items.Add(new OpcionCombo() { Valor = "Kg", Texto = "Kilogramos (Kg)" });
            cboUnidadBase.Items.Add(new OpcionCombo() { Valor = "Gramos", Texto = "Gramos" });
            cboUnidadBase.Items.Add(new OpcionCombo() { Valor = "Toneladas", Texto = "Toneladas" });
            cboUnidadBase.Items.Add(new OpcionCombo() { Valor = "Litros", Texto = "Litros" });
            cboUnidadBase.DisplayMember = "Texto";
            cboUnidadBase.ValueMember = "Valor";
            cboUnidadBase.SelectedIndex = 0;

            // --- CORRECCIÓN Y MEJORA DE cboBusqueda ---
            cboBusqueda.Items.Clear();
            // Añadimos manualmente solo las columnas más importantes para buscar.
            cboBusqueda.Items.Add(new OpcionCombo() { Valor = "Codigo", Texto = "Código" });
            cboBusqueda.Items.Add(new OpcionCombo() { Valor = "Nombre", Texto = "Nombre" });
            cboBusqueda.Items.Add(new OpcionCombo() { Valor = "Categoria", Texto = "Categoría" });
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;
            // --- FIN DE LA CORRECCIÓN ---

            // Carga inicial de datos
            CargarProductos();
            Limpiar(); // Limpiamos al inicio para generar el primer código de producto.
        }
        // --- MÉTODO NUEVO: Se dispara al cambiar la categoría ---
        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Solo generamos un código nuevo si estamos creando un producto (txtId está vacío)
            if (string.IsNullOrEmpty(txtId.Text))
            {
                if (cboCategoria.SelectedItem != null)
                {
                    string idCategoria = ((OpcionCombo)cboCategoria.SelectedItem).Valor.ToString();
                    txtCodigo.Text = new CN_Producto().ObtenerSiguienteCodigo(idCategoria);
                }
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            // --- VALIDACIONES PREVIAS (AQUÍ ESTÁ LA CORRECCIÓN) ---

            // Validación para Precio de Compra
            if (string.IsNullOrWhiteSpace(txtPrecioCompra.Text) || !decimal.TryParse(txtPrecioCompra.Text, out _))
            {
                MessageBox.Show("Debe ingresar un precio de compra válido.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioCompra.Focus();
                return; // Detiene la ejecución
            }

            // Validación para Precio de Venta (LA QUE FALTABA)
            if (string.IsNullOrWhiteSpace(txtPrecioVenta.Text) || !decimal.TryParse(txtPrecioVenta.Text, out _))
            {
                MessageBox.Show("Debe ingresar un precio de venta válido.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioVenta.Focus();
                return; // Detiene la ejecución
            }

            // --- FIN DE LAS VALIDACIONES ---


            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("No se pudo generar el código del producto. Intente seleccionar la categoría de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Si pasa las validaciones, ahora sí creamos el objeto Producto
            Producto obj = new Producto()
            {
                Id = txtId.Text,
                Codigo = txtCodigo.Text,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                oCategoria = new Categoria() { Id = ((OpcionCombo)cboCategoria.SelectedItem).Valor.ToString() },
                UnidadBase = ((OpcionCombo)cboUnidadBase.SelectedItem).Valor.ToString(),

                // Estas conversiones ahora son seguras porque ya validamos el texto
                PrecioCompra = Convert.ToDecimal(txtPrecioCompra.Text),
                PrecioVenta = Convert.ToDecimal(txtPrecioVenta.Text),

                Estado = ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString() == "1"
            };

            // El resto del código para guardar o editar se mantiene exactamente igual...
            if (string.IsNullOrEmpty(obj.Id))
            {
                string idGenerado = new CN_Producto().Registrar(obj, out mensaje);
                if (!string.IsNullOrEmpty(idGenerado) && idGenerado != "0")
                {
                    MessageBox.Show("Producto registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                bool resultado = new CN_Producto().Editar(obj, out mensaje);
                if (resultado)
                {
                    MessageBox.Show("Producto modificado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtStock.Text = "0";
            txtPrecioCompra.Text = ""; //0.00
            txtPrecioVenta.Text = ""; //0.00

            if (cboEstado.Items.Count > 0) cboEstado.SelectedIndex = 0;
            if (cboUnidadBase.Items.Count > 0) cboUnidadBase.SelectedIndex = 0;

            if (cboCategoria.Items.Count > 0)
            {
                cboCategoria.SelectedIndex = 0;
            }
            // Forzamos la llamada al evento para asegurar la generación del código
            cboCategoria_SelectedIndexChanged(this, EventArgs.Empty);

            txtNombre.Focus();
        }

        #region Métodos Auxiliares (sin cambios)
        private void CargarProductos()
        {
            dgvData.Rows.Clear();
            List<Producto> lista = new CN_Producto().Listar();
            foreach (Producto item in lista)
            {
                dgvData.Rows.Add(new object[] {
                    item.Id, item.Codigo, item.Nombre, item.Descripcion,
                    item.oCategoria.Id, item.oCategoria.Descripcion,
                    item.Stock, item.UnidadBase, item.PrecioCompra, item.PrecioVenta,
                    item.Estado ? "1" : "0", item.Estado ? "Activo" : "No Activo"
                });
            }
        }
        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvData.Rows[e.RowIndex];
                txtIndice.Text = e.RowIndex.ToString();
                txtId.Text = row.Cells["Id"].Value.ToString();
                txtCodigo.Text = row.Cells["Codigo"].Value.ToString();
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtDescripcion.Text = row.Cells["Descripcion"].Value.ToString();
                txtStock.Text = row.Cells["Stock"].Value.ToString();
                txtPrecioCompra.Text = Convert.ToDecimal(row.Cells["PrecioCompra"].Value).ToString("0.00");
                txtPrecioVenta.Text = Convert.ToDecimal(row.Cells["PrecioVenta"].Value).ToString("0.00");

                string valorUnidad = row.Cells["UnidadBase"].Value.ToString();
                foreach (OpcionCombo oc in cboUnidadBase.Items)
                {
                    if (oc.Valor.ToString() == valorUnidad) { cboUnidadBase.SelectedItem = oc; break; }
                }

                string valorCategoria = row.Cells["IdCategoria"].Value.ToString();
                foreach (OpcionCombo oc in cboCategoria.Items)
                {
                    if (oc.Valor.ToString() == valorCategoria) { cboCategoria.SelectedItem = oc; break; }
                }

                string valorEstado = row.Cells["EstadoValor"].Value.ToString();
                foreach (OpcionCombo oc in cboEstado.Items)
                {
                    if (oc.Valor.ToString() == valorEstado) { cboEstado.SelectedItem = oc; break; }
                }
            }
        }

        
        private void btnLimpiar_Click(object sender, EventArgs e) => Limpiar();
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtId.Text))
            {
                if (MessageBox.Show("¿Desea eliminar el Producto?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    if (new CN_Producto().Eliminar(txtId.Text, out mensaje))
                    {
                        CargarProductos();
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
                    if (row.Cells[columnaFiltro].Value != null &&
                        row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = false;
                    }
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
        #endregion
    }
}
