namespace CapaEntidades
{
    public class ReporteVenta
    {
        public string FechaRegistro { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public decimal MontoTotal { get; set; }
        public string UsuarioRegistro { get; set; }
        public string DocumentoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Categoria { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal CantidadVendida { get; set; }
        public string UnidadBase { get; set; }
        public decimal SubTotal { get; set; }
        public bool Estado { get; set; }
    }
}

