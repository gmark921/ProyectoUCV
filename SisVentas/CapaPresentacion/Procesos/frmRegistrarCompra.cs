using CapaEntidades;
using CapaNegocio;
using CapaPresentacion.Formularios.Modales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion.Formularios.Procesos
{
    public partial class frmRegistrarCompra : Form
    {
        private Producto _productoSeleccionado;
        private readonly Usuario _usuarioActual;

        public frmRegistrarCompra(Usuario oUsuario)
        {
            _usuarioActual = oUsuario;
            InitializeComponent();
        }
        private void frmRegistrarCompra_Load(object sender, EventArgs e)
        {
            cboTipoDoc.Items.Add("Boleta");
            cboTipoDoc.Items.Add("Factura");
            cboTipoDoc.SelectedIndex = 0;
        }
        #region Búsqueda de Proveedor y Producto
        private void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            using (var modal = new mdlSeleccionarProveedor())
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    txtIdProveedor.Text = modal._Proveedor.Id;
                    txtRazonSocial.Text = modal._Proveedor.RazonSocial;
                    txtNombreComercial.Text = modal._Proveedor.NombreComercial;
                }
            }
        }
        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            using (var modal = new mdlSeleccionarProducto())
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    _productoSeleccionado = modal._Producto;
                    MostrarDatosProducto();
                }
            }
        }
        private void txtCodProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                // CORRECCIÓN: Usamos el método optimizado para buscar por código.
                _productoSeleccionado = new CN_Producto().ObtenerPorCodigo(txtCodProducto.Text);
                MostrarDatosProducto();
            }
        }
        #endregion

        #region Lógica del DataGridView
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (_productoSeleccionado == null)
            {
                MessageBox.Show("Debe seleccionar un producto primero", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecioCompra.Text, out decimal precioCompra) || precioCompra <= 0)
            {
                MessageBox.Show("El precio de compra no es válido.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioCompra.Focus();
                return;
            }

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells["IdProducto"].Value.ToString() == _productoSeleccionado.Id)
                {
                    MessageBox.Show("El producto ya fue agregado a la lista.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    LimpiarProducto();
                    return;
                }
            }

            dgvData.Rows.Add(new object[] {
                _productoSeleccionado.Id,
                _productoSeleccionado.Nombre,
                precioCompra.ToString("0.00"),
                nudCantidad.Value,
                _productoSeleccionado.UnidadBase,
                (nudCantidad.Value * precioCompra).ToString("0.00")
            });

            CalcularTotal();
            LimpiarProducto();
            txtCodProducto.Focus();
        }
        private void btnAumentar_Click(object sender, EventArgs e) => ActualizarCantidadEnGrid(1);
        private void btnDisminuir_Click(object sender, EventArgs e) => ActualizarCantidadEnGrid(-1);
        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                dgvData.Rows.Remove(dgvData.SelectedRows[0]);
                CalcularTotal();
            }
            else
            {
                MessageBox.Show("Seleccione un producto de la lista para quitarlo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ActualizarCantidadEnGrid(int cantidadCambio)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvData.SelectedRows[0];
                decimal cantidadActual = Convert.ToDecimal(selectedRow.Cells["Cantidad"].Value);
                decimal nuevaCantidad = cantidadActual + cantidadCambio;

                if (nuevaCantidad > 0)
                {
                    selectedRow.Cells["Cantidad"].Value = nuevaCantidad;
                    decimal precioCompra = Convert.ToDecimal(selectedRow.Cells["PrecioCompra"].Value);
                    selectedRow.Cells["SubTotal"].Value = (nuevaCantidad * precioCompra).ToString("0.00");
                    CalcularTotal();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto de la lista para modificar la cantidad.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Registro y Limpieza
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdProveedor.Text))
            {
                MessageBox.Show("Debe seleccionar un proveedor.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("Debe agregar al menos un producto a la compra.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable detalle_compra = new DataTable();
            detalle_compra.Columns.Add("IdProducto", typeof(string));
            detalle_compra.Columns.Add("PrecioCompra", typeof(decimal));
            detalle_compra.Columns.Add("CantidadComprada", typeof(decimal));
            detalle_compra.Columns.Add("UnidadBase", typeof(string));

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                detalle_compra.Rows.Add(
                    row.Cells["IdProducto"].Value.ToString(),
                    Convert.ToDecimal(row.Cells["PrecioCompra"].Value),
                    Convert.ToDecimal(row.Cells["Cantidad"].Value),
                    row.Cells["UnidadBase"].Value.ToString()
                );
            }

            Compra oCompra = new Compra()
            {
                oUsuario = new Usuario() { Id = _usuarioActual.Id },
                oProveedor = new Proveedor() { Id = txtIdProveedor.Text },
                TipoDocumento = cboTipoDoc.Text,
                MontoTotal = Convert.ToDecimal(txtTotalPagar.Text)
            };

            bool respuesta = new CN_Compra().Registrar(oCompra, detalle_compra, out string numeroGenerado, out string mensaje);

            if (respuesta)
            {
                MessageBox.Show($"Compra registrada con éxito.\nNúmero de Documento: {numeroGenerado}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarTodo();
            }
            else
            {
                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MostrarDatosProducto()
        {
            if (_productoSeleccionado != null)
            {
                txtIdProducto.Text = _productoSeleccionado.Id;
                txtCodProducto.Text = _productoSeleccionado.Codigo;
                txtProducto.Text = _productoSeleccionado.Nombre;
                txtPrecioCompra.Text = _productoSeleccionado.PrecioCompra.ToString("0.00");
                lblUnidadCompra.Text = _productoSeleccionado.UnidadBase;

                // --- LÓGICA PARA CONTROLAR DECIMALES ---
                if (_productoSeleccionado.UnidadBase.ToLower() == "unidades")
                {
                    nudCantidad.DecimalPlaces = 0;
                }
                else
                {
                    nudCantidad.DecimalPlaces = 2; // O 3 si manejas gramos, etc.
                }
                // --- FIN DE LA LÓGICA ---

                nudCantidad.Focus();
                nudCantidad.Select(0, nudCantidad.Text.Length);
            }
            else
            {
                MessageBox.Show("No se encontró el producto o está inactivo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                LimpiarProducto();
                txtCodProducto.Focus();
            }
        }
        private void CalcularTotal()
        {
            decimal total = dgvData.Rows.Cast<DataGridViewRow>().Sum(row => Convert.ToDecimal(row.Cells["SubTotal"].Value));
            txtTotalPagar.Text = total.ToString("0.00");
        }
        private void LimpiarProducto()
        {
            _productoSeleccionado = null;
            txtIdProducto.Text = "";
            txtCodProducto.Text = "";
            txtProducto.Text = "";
            txtPrecioCompra.Text = "0.00";
            lblUnidadCompra.Text = "Unidad";
            nudCantidad.Value = 1;
            nudCantidad.DecimalPlaces = 0; // <-- Reseteamos a 0 por defecto
        }
        private void LimpiarTodo()
        {
            txtIdProveedor.Text = "";
            txtRazonSocial.Text = "";
            txtNombreComercial.Text = "";
            LimpiarProducto();
            dgvData.Rows.Clear();
            CalcularTotal();
        }
        #endregion
       
        
    }
}
