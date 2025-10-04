namespace CapaEntidades
{
    public class ReporteCompra
    {
        public string FechaRegistro { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public decimal MontoTotal { get; set; }
        public string UsuarioRegistro { get; set; }
        public string RazonSocialProveedor { get; set; }
        public string NombreComercialProveedor { get; set; }
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Categoria { get; set; }
        public decimal PrecioCompra { get; set; }   
        public decimal CantidadComprada { get; set; }
        public string UnidadBase { get; set; }
        public decimal SubTotal { get; set; }
        public bool Estado { get; set; }
    }
}

