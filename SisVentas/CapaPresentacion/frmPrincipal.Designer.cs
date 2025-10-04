namespace CapaPresentacion
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menu = new System.Windows.Forms.MenuStrip();
            this.menuSeguridad = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuPermisos = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuRoles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMantenedor = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuCategorias = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuProductos = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuClientes = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuProveedores = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVentas = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCompras = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReportes = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuReporteVentas = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuReporteCompras = new System.Windows.Forms.ToolStripMenuItem();
            this.lblUsuarioConectado = new System.Windows.Forms.Label();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.AutoSize = false;
            this.menu.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSeguridad,
            this.menuMantenedor,
            this.menuVentas,
            this.menuCompras,
            this.menuReportes});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(1284, 40);
            this.menu.TabIndex = 0;
            this.menu.Text = "menuStrip1";
            // 
            // menuSeguridad
            // 
            this.menuSeguridad.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.submenuUsuarios,
            this.submenuPermisos,
            this.submenuRoles});
            this.menuSeguridad.Name = "menuSeguridad";
            this.menuSeguridad.Size = new System.Drawing.Size(147, 36);
            this.menuSeguridad.Text = "🔐Seguridad";
            // 
            // submenuUsuarios
            // 
            this.submenuUsuarios.Name = "submenuUsuarios";
            this.submenuUsuarios.Size = new System.Drawing.Size(197, 34);
            this.submenuUsuarios.Text = "👤Usuarios";
            this.submenuUsuarios.Click += new System.EventHandler(this.submenuUsuarios_Click);
            // 
            // submenuPermisos
            // 
            this.submenuPermisos.Name = "submenuPermisos";
            this.submenuPermisos.Size = new System.Drawing.Size(197, 34);
            this.submenuPermisos.Text = "🪪Permisos";
            this.submenuPermisos.Click += new System.EventHandler(this.submenuPermisos_Click);
            // 
            // submenuRoles
            // 
            this.submenuRoles.Name = "submenuRoles";
            this.submenuRoles.Size = new System.Drawing.Size(197, 34);
            this.submenuRoles.Text = "🗞️Roles";
            this.submenuRoles.Click += new System.EventHandler(this.submenuRoles_Click);
            // 
            // menuMantenedor
            // 
            this.menuMantenedor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.submenuCategorias,
            this.submenuProductos,
            this.submenuClientes,
            this.submenuProveedores});
            this.menuMantenedor.Name = "menuMantenedor";
            this.menuMantenedor.Size = new System.Drawing.Size(174, 36);
            this.menuMantenedor.Text = "🔧 Mantenedor";
            // 
            // submenuCategorias
            // 
            this.submenuCategorias.Name = "submenuCategorias";
            this.submenuCategorias.Size = new System.Drawing.Size(235, 34);
            this.submenuCategorias.Text = "📁 Categorías";
            this.submenuCategorias.Click += new System.EventHandler(this.submenuCategorias_Click_1);
            // 
            // submenuProductos
            // 
            this.submenuProductos.Name = "submenuProductos";
            this.submenuProductos.Size = new System.Drawing.Size(235, 34);
            this.submenuProductos.Text = "📦 Productos";
            this.submenuProductos.Click += new System.EventHandler(this.submenuProductos_Click_1);
            // 
            // submenuClientes
            // 
            this.submenuClientes.Name = "submenuClientes";
            this.submenuClientes.Size = new System.Drawing.Size(235, 34);
            this.submenuClientes.Text = "👥 Clientes";
            this.submenuClientes.Click += new System.EventHandler(this.submenuClientes_Click_1);
            // 
            // submenuProveedores
            // 
            this.submenuProveedores.Name = "submenuProveedores";
            this.submenuProveedores.Size = new System.Drawing.Size(235, 34);
            this.submenuProveedores.Text = "\t🚚 Proveedores";
            this.submenuProveedores.Click += new System.EventHandler(this.submenuProveedores_Click_1);
            // 
            // menuVentas
            // 
            this.menuVentas.Name = "menuVentas";
            this.menuVentas.Size = new System.Drawing.Size(122, 36);
            this.menuVentas.Text = "🛒 Ventas";
            this.menuVentas.Click += new System.EventHandler(this.menuVentas_Click_1);
            // 
            // menuCompras
            // 
            this.menuCompras.Name = "menuCompras";
            this.menuCompras.Size = new System.Drawing.Size(142, 36);
            this.menuCompras.Text = "🛍️ Compras";
            this.menuCompras.Click += new System.EventHandler(this.menuCompras_Click_1);
            // 
            // menuReportes
            // 
            this.menuReportes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.submenuReporteVentas,
            this.submenuReporteCompras});
            this.menuReportes.Name = "menuReportes";
            this.menuReportes.Size = new System.Drawing.Size(141, 36);
            this.menuReportes.Text = "📊 Reportes";
            // 
            // submenuReporteVentas
            // 
            this.submenuReporteVentas.Name = "submenuReporteVentas";
            this.submenuReporteVentas.Size = new System.Drawing.Size(310, 34);
            this.submenuReporteVentas.Text = "📈 Reporte de Ventas";
            this.submenuReporteVentas.Click += new System.EventHandler(this.submenuReporteVentas_Click_1);
            // 
            // submenuReporteCompras
            // 
            this.submenuReporteCompras.Name = "submenuReporteCompras";
            this.submenuReporteCompras.Size = new System.Drawing.Size(310, 34);
            this.submenuReporteCompras.Text = "📋 Reporte de Compras";
            this.submenuReporteCompras.Click += new System.EventHandler(this.submenuReporteCompras_Click_1);
            // 
            // lblUsuarioConectado
            // 
            this.lblUsuarioConectado.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblUsuarioConectado.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuarioConectado.Location = new System.Drawing.Point(0, 720);
            this.lblUsuarioConectado.Name = "lblUsuarioConectado";
            this.lblUsuarioConectado.Size = new System.Drawing.Size(1284, 40);
            this.lblUsuarioConectado.TabIndex = 2;
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 760);
            this.Controls.Add(this.lblUsuarioConectado);
            this.Controls.Add(this.menu);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menu;
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPrincipal";
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem menuSeguridad;
        private System.Windows.Forms.ToolStripMenuItem menuMantenedor;
        private System.Windows.Forms.ToolStripMenuItem menuVentas;
        private System.Windows.Forms.ToolStripMenuItem menuCompras;
        private System.Windows.Forms.ToolStripMenuItem menuReportes;
        private System.Windows.Forms.ToolStripMenuItem submenuCategorias;
        private System.Windows.Forms.ToolStripMenuItem submenuProductos;
        private System.Windows.Forms.ToolStripMenuItem submenuClientes;
        private System.Windows.Forms.ToolStripMenuItem submenuProveedores;
        private System.Windows.Forms.ToolStripMenuItem submenuReporteVentas;
        private System.Windows.Forms.ToolStripMenuItem submenuReporteCompras;
        private System.Windows.Forms.Label lblUsuarioConectado;
        private System.Windows.Forms.ToolStripMenuItem submenuUsuarios;
        private System.Windows.Forms.ToolStripMenuItem submenuPermisos;
        private System.Windows.Forms.ToolStripMenuItem submenuRoles;
    }
}