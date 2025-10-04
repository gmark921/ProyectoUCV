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

namespace CapaPresentacion.Formularios.Reportes
{
    public partial class frmReporteCompras : Form
    {
        public frmReporteCompras()
        {
            InitializeComponent();
        }

        private void frmReporteCompras_Load(object sender, EventArgs e)
        {
            dtpFechaFin.Value = DateTime.Now;
            dtpFechaInicio.Value = DateTime.Now.AddMonths(-1);
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

            var lista = new CN_Reporte().ReporteMaestroCompra(
                dtpFechaInicio.Value.ToString("yyyy-MM-dd"),
                dtpFechaFin.Value.ToString("yyyy-MM-dd"),
                txtBusqueda.Text
            );

            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    dgvData.Rows.Add(new object[] {
                        item.CodigoProducto, // ID de la compra (columna oculta)
                        item.FechaRegistro,
                        item.TipoDocumento,
                        item.NumeroDocumento,
                        item.MontoTotal.ToString("0.00"),
                        item.UsuarioRegistro,
                        item.RazonSocialProveedor
                        // El botón se añade solo
                    });
                }
            }
        }
        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignorar clics en el encabezado

            DataGridViewRow row = dgvData.Rows[e.RowIndex];

            // --- LÓGICA PARA ANULAR LA COMPRA ---
            if (dgvData.Columns[e.ColumnIndex].Name == "btnAnular")
            {
                string idCompra = row.Cells["IdCompra"].Value.ToString();

                var result = MessageBox.Show("¿Está seguro de que desea anular esta compra?\n\nEsta acción revertirá el stock y no se puede deshacer.",
                                             "Confirmar Anulación",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    bool anuladoConExito = new CN_Compra().Anular(idCompra, out mensaje);

                    if (anuladoConExito)
                    {
                        MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                        row.DefaultCellStyle.ForeColor = Color.DarkGray;
                        row.DefaultCellStyle.SelectionBackColor = Color.LightGray;
                        row.DefaultCellStyle.SelectionForeColor = Color.DarkGray;
                        row.Cells["btnAnular"].ReadOnly = true;
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
                string idCompra = row.Cells["IdCompra"].Value.ToString();
                dgvDetalle.Rows.Clear();
                DataTable dtDetalle = new CN_Compra().ObtenerDetalleCompra(idCompra);

                if (dtDetalle.Rows.Count > 0)
                {
                    foreach (DataRow detalleRow in dtDetalle.Rows)
                    {
                        dgvDetalle.Rows.Add(new object[] {
                    detalleRow["Codigo"],
                    detalleRow["Producto"],
                    Convert.ToDecimal(detalleRow["PrecioCompra"]).ToString("0.00"),
                    detalleRow["CantidadComprada"],
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
