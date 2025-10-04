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
    public partial class frmRegistrarVenta : Form
    {
        private readonly Usuario _usuarioActual;
        private Producto _productoSeleccionado;
        public frmRegistrarVenta(Usuario oUsuario)
        {
            _usuarioActual = oUsuario;
            InitializeComponent();
        }

        private void frmRegistrarVenta_Load(object sender, EventArgs e)
        {
            cboTipoDoc.Items.Add("Boleta");
            cboTipoDoc.Items.Add("Factura");
            cboTipoDoc.SelectedIndex = 0;
        }

        #region Búsquedas
        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            using (var modal = new mdlSeleccionarCliente())
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    txtIdCliente.Text = modal._Cliente.Id;
                    txtDocCliente.Text = modal._Cliente.Documento;
                    txtNombreCliente.Text = modal._Cliente.NombreCompleto;
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
                MessageBox.Show("Debe seleccionar un producto.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_productoSeleccionado.Stock < nudCantidadVenta.Value)
            {
                MessageBox.Show($"No hay stock suficiente. Stock actual: {_productoSeleccionado.Stock}", "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                _productoSeleccionado.Codigo,
                _productoSeleccionado.Nombre,
                _productoSeleccionado.PrecioVenta.ToString("0.00"),
                nudCantidadVenta.Value,
                _productoSeleccionado.UnidadBase,
                (nudCantidadVenta.Value * _productoSeleccionado.PrecioVenta).ToString("0.00")
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
                string idProducto = selectedRow.Cells["IdProducto"].Value.ToString();

                // Buscamos el stock actual del producto
                Producto productoEnGrid = new CN_Producto().Listar().FirstOrDefault(p => p.Id == idProducto);
                if (productoEnGrid == null) return; // Si el producto no existe, salimos.

                decimal cantidadActual = Convert.ToDecimal(selectedRow.Cells["Cantidad"].Value);
                decimal nuevaCantidad = cantidadActual + cantidadCambio;

                if (nuevaCantidad <= 0) return; // No permitir cantidades menores o iguales a 0.

                if (nuevaCantidad > productoEnGrid.Stock)
                {
                    MessageBox.Show($"No hay stock suficiente. Stock actual: {productoEnGrid.Stock}", "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                selectedRow.Cells["Cantidad"].Value = nuevaCantidad;
                decimal precioVenta = Convert.ToDecimal(selectedRow.Cells["PrecioVenta"].Value);
                selectedRow.Cells["SubTotal"].Value = (nuevaCantidad * precioVenta).ToString("0.00");
                CalcularTotal();
            }
            else
            {
                MessageBox.Show("Seleccione un producto de la lista para modificar la cantidad.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion


        #region Registro y Métodos Auxiliares
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("Debe agregar al menos un producto a la venta.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPagaCon.Text, out decimal montoPago) || montoPago < Convert.ToDecimal(txtTotalPagar.Text))
            {
                MessageBox.Show("El monto de pago no es válido o es menor al total a pagar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPagaCon.Focus();
                return;
            }

            DataTable detalle_venta = new DataTable();
            detalle_venta.Columns.Add("IdProducto", typeof(string));
            detalle_venta.Columns.Add("PrecioVenta", typeof(decimal));
            detalle_venta.Columns.Add("CantidadVendida", typeof(decimal));

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                detalle_venta.Rows.Add(
                    row.Cells["IdProducto"].Value.ToString(),
                    Convert.ToDecimal(row.Cells["PrecioVenta"].Value),
                    Convert.ToDecimal(row.Cells["Cantidad"].Value)
                );
            }

            Venta oVenta = new Venta()
            {
                oUsuario = new Usuario() { Id = _usuarioActual.Id },
                oCliente = string.IsNullOrEmpty(txtIdCliente.Text) ? null : new Cliente() { Id = txtIdCliente.Text },
                TipoDocumento = cboTipoDoc.Text,
                MontoPago = montoPago,
                MontoCambio = Convert.ToDecimal(txtCambio.Text),
                MontoTotal = Convert.ToDecimal(txtTotalPagar.Text)
            };

            bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out string numeroGenerado, out string mensaje);

            if (respuesta)
            {
                MessageBox.Show($"Venta registrada con éxito.\nNúmero de Documento: {numeroGenerado}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                txtPrecio.Text = _productoSeleccionado.PrecioVenta.ToString("0.00");
                txtStock.Text = _productoSeleccionado.Stock.ToString();
                lblUnidadStock.Text = _productoSeleccionado.UnidadBase;
                lblUnidadVenta.Text = _productoSeleccionado.UnidadBase;

                // --- LÓGICA PARA CONTROLAR DECIMALES ---
                if (_productoSeleccionado.UnidadBase.ToLower() == "unidades")
                {
                    nudCantidadVenta.DecimalPlaces = 0;
                }
                else
                {
                    nudCantidadVenta.DecimalPlaces = 2;
                }
                // --- FIN DE LA LÓGICA ---

                nudCantidadVenta.Focus();
                nudCantidadVenta.Select(0, nudCantidadVenta.Text.Length);
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
            CalcularCambio();
        }
        private void CalcularCambio()
        {
            if (decimal.TryParse(txtPagaCon.Text, out decimal pagaCon) && decimal.TryParse(txtTotalPagar.Text, out decimal total))
            {
                decimal cambio = pagaCon - total;
                txtCambio.Text = cambio > 0 ? cambio.ToString("0.00") : "0.00";
            }
            else
            {
                txtCambio.Text = "0.00";
            }
        }
        private void txtPagaCon_KeyDown(object sender, KeyEventArgs e)
        {
            CalcularCambio();
        }
        private void LimpiarProducto()
        {
            _productoSeleccionado = null;
            txtIdProducto.Text = "";
            txtCodProducto.Text = "";
            txtProducto.Text = "";
            txtStock.Text = "";
            lblUnidadStock.Text = "Unidad";
            lblUnidadVenta.Text = "Unidad";
            txtPrecio.Text = "";
            nudCantidadVenta.Value = 1;
            nudCantidadVenta.DecimalPlaces = 0; // <-- Reseteamos a 0 por defecto
        }

        private void LimpiarTodo()
        {
            txtIdCliente.Text = "";
            txtDocCliente.Text = "";
            txtNombreCliente.Text = "";
            LimpiarProducto();
            dgvData.Rows.Clear();
            txtPagaCon.Text = "";
            txtCambio.Text = "";
            CalcularTotal();
        }
        #endregion
    }
}
