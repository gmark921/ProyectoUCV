using CapaEntidades;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion.Formularios.Reportes
{
    public partial class frmReporteVentas : Form
    {
        public frmReporteVentas()
        {
            InitializeComponent();
        }
        private void frmReporteVentas_Load(object sender, EventArgs e)
        {
            // Establecemos un rango de fechas por defecto (ej: último mes)
            dtpFechaFin.Value = DateTime.Now;
            dtpFechaInicio.Value = DateTime.Now.AddMonths(-1);

            // Llamamos al método para cargar todos los datos del último mes al iniciar
            BuscarReporte();
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarReporte();

            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("No se encontraron resultados para la búsqueda.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void BuscarReporte()
        {
            dgvData.Rows.Clear();
            dgvDetalle.Rows.Clear();

            var lista = new CN_Reporte().ReporteMaestroVenta(
                dtpFechaInicio.Value.ToString("yyyy-MM-dd"),
                dtpFechaFin.Value.ToString("yyyy-MM-dd"),
                txtBusqueda.Text
            );

            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    dgvData.Rows.Add(new object[] {
                        item.CodigoProducto, // ID de la venta (columna oculta)
                        item.FechaRegistro,
                        item.TipoDocumento,
                        item.NumeroDocumento,
                        item.MontoTotal.ToString("0.00"),
                        item.UsuarioRegistro,
                        item.DocumentoCliente,
                        item.NombreCliente
                        // El botón se añade solo, no es necesario ponerlo aquí
                    });
                }
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignorar clics en el encabezado

            DataGridViewRow row = dgvData.Rows[e.RowIndex];

            // --- LÓGICA PARA ANULAR LA VENTA ---
            if (dgvData.Columns[e.ColumnIndex].Name == "btnAnular")
            {
                string idVenta = row.Cells["IdVenta"].Value.ToString();

                // Pedir confirmación al usuario
                var result = MessageBox.Show("¿Está seguro de que desea anular esta venta?\n\nEsta acción revertirá el stock y no se puede deshacer.",
                                             "Confirmar Anulación",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    bool anuladoConExito = new CN_Venta().Anular(idVenta, out mensaje);

                    if (anuladoConExito)
                    {
                        MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // "Apagamos" la fila visualmente para que se note que está anulada
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                        row.DefaultCellStyle.ForeColor = Color.DarkGray;
                        row.DefaultCellStyle.SelectionBackColor = Color.LightGray;
                        row.DefaultCellStyle.SelectionForeColor = Color.DarkGray;
                        row.Cells["btnAnular"].ReadOnly = true; // Deshabilitamos el botón
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            // --- LÓGICA PARA VER EL DETALLE (se mantiene igual) ---
            else
            {
                string idVenta = row.Cells["IdVenta"].Value.ToString();
                dgvDetalle.Rows.Clear();
                DataTable dtDetalle = new CN_Venta().ObtenerDetalleVenta(idVenta);

                if (dtDetalle.Rows.Count > 0)
                {
                    foreach (DataRow detalleRow in dtDetalle.Rows)
                    {
                        dgvDetalle.Rows.Add(new object[] {
                    detalleRow["Codigo"],
                    detalleRow["Producto"],
                    Convert.ToDecimal(detalleRow["PrecioVenta"]).ToString("0.00"),
                    detalleRow["CantidadVendida"],
                    detalleRow["UnidadBase"],
                    Convert.ToDecimal(detalleRow["SubTotal"]).ToString("0.00")
                });
                    }
                }
            }
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            // Restablece los filtros y vuelve a cargar la lista por defecto
            txtBusqueda.Text = "";
            dtpFechaFin.Value = DateTime.Now;
            dtpFechaInicio.Value = DateTime.Now.AddMonths(-1);
            BuscarReporte();
        }

        
    }
}
