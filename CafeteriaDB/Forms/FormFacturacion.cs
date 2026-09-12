using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CafeteriaDB.Clases;
using System.Linq;

namespace CafeteriaDB.Forms
{
    public partial class FormFacturacion : Form
    {
        private const decimal DIVISOR_IVA_10 = 11m;

        private Label lblFecha, lblNumFactura, lblClienteCI, lblNombreCliente, lblDireccionCliente, lblTelefonoCliente;
        private DateTimePicker dtpFecha;
        private TextBox txtNumFactura, txtClienteCI, txtNombreCliente, txtDireccionCliente, txtTelefonoCliente;
        private Button btnBuscarCliente;
        private Label lblTipoPago;
        private ComboBox cmbTipoPago;
        private Button btnHistorial;

        private Label lblProducto, lblCantidad;
        private TextBox txtProductoSeleccionado;
        private Button btnBuscarProducto;
        private NumericUpDown nudCantidad;
        private Button btnAgregar;
        private int idProductoBuscado = 0;
        private string nombreProductoBuscado = "";

        private DataGridView dgvCarrito;
        private DataTable dtCarrito;
        private Button btnQuitarItem;

        private Label lblTotalPagar, lblTotalPagarMonto, lblIVAInfo, lblIVAMonto;
        private Button btnGuardarVenta, btnCancelar;

        public FormFacturacion()
        {
            InicializarComponentes();
            ConfigurarTablaCarrito();
            txtNumFactura.Text = GenerarNumeroFactura();
        }

        private void InicializarComponentes()
        {
            this.Text = "CafeteriaDB - Facturación y Ventas";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 245, 241);
            this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "cafeteria.ico"));
            this.KeyPreview = true;
            this.KeyDown += FormFacturacion_KeyDown;

            Panel pnlHeader = new Panel { 
                Dock = DockStyle.Top, 
                Height = 60, 
                BackColor = Color.FromArgb(30, 57, 36) };
            Label lblTitulo = new Label
            {
                Text = "REGISTRO DE VENTA A CLIENTES",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitulo);
            this.Controls.Add(pnlHeader);


            Button btnSalirHeader = new Button
            {
                Text = "Salir",
                Location = new Point(870, 13),
                Size = new Size(90, 34),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(154, 114, 101),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnSalirHeader.FlatAppearance.BorderSize = 0;
            btnSalirHeader.Click += (s, e) => this.Close();
            pnlHeader.Controls.Add(btnSalirHeader);
            btnHistorial = new Button
            {
                Text = "Historial",
                Location = new Point(760, 13),
                Size = new Size(100, 34),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnHistorial.FlatAppearance.BorderSize = 0;
            btnHistorial.Click += (s, e) => new FormHistorialVentas().ShowDialog();
            pnlHeader.Controls.Add(btnHistorial);



            // Fila 1: Fecha, N° Factura, CI/RUC + Buscar
            lblFecha = new Label { Text = "Fecha:", Location = new Point(20, 80), AutoSize = true };
            dtpFecha = new DateTimePicker { Location = new Point(80, 77), Width = 130, Format = DateTimePickerFormat.Short, Enabled = false, Value = DateTime.Now };

            lblNumFactura = new Label { Text = "N° Factura:", Location = new Point(230, 80), AutoSize = true };
            txtNumFactura = new TextBox { Location = new Point(310, 77), Width = 150, ReadOnly = true, BackColor = Color.FromArgb(226, 224, 222) };
            lblClienteCI = new Label { Text = "CI/RUC:", Location = new Point(480, 80), AutoSize = true };
            txtClienteCI = new TextBox { Location = new Point(540, 77), Width = 100, ReadOnly = true, BackColor = Color.FromArgb(226, 224, 222) };
            btnBuscarCliente = new Button
            {
                Text = "Buscar",
                Location = new Point(650, 76),
                Size = new Size(70, 25),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBuscarCliente.Click += BtnBuscarCliente_Click;

            lblTipoPago = new Label { Text = "Pago:", Location = new Point(770, 80), AutoSize = true };
            cmbTipoPago = new ComboBox { Location = new Point(815, 77), Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTipoPago.Items.AddRange(new string[] { "Efectivo", "Tarjeta", "Transferencia", "QR" });
            cmbTipoPago.SelectedIndex = 0;

            this.Controls.AddRange(new Control[] { lblFecha, dtpFecha, lblNumFactura, txtNumFactura, lblClienteCI, txtClienteCI, btnBuscarCliente, lblTipoPago, cmbTipoPago });
            // Fila 2: Cliente
            lblNombreCliente = new Label { Text = "Cliente:", Location = new Point(20, 115), AutoSize = true };
            txtNombreCliente = new TextBox { Location = new Point(90, 112), Width = 680, ReadOnly = true };
            this.Controls.AddRange(new Control[] { lblNombreCliente, txtNombreCliente });

            // Fila 3: Dirección, Teléfono
            lblDireccionCliente = new Label { Text = "Dirección:", Location = new Point(20, 150), AutoSize = true };
            txtDireccionCliente = new TextBox { Location = new Point(90, 147), Width = 320, ReadOnly = true };

            lblTelefonoCliente = new Label { Text = "Teléfono:", Location = new Point(430, 150), AutoSize = true };
            txtTelefonoCliente = new TextBox { Location = new Point(500, 147), Width = 200, ReadOnly = true };

            this.Controls.AddRange(new Control[] { lblDireccionCliente, txtDireccionCliente, lblTelefonoCliente, txtTelefonoCliente });

            // Fila 4: Agregar producto
            lblProducto = new Label { Text = "Producto:", Location = new Point(20, 190), AutoSize = true };
            txtProductoSeleccionado = new TextBox { Location = new Point(90, 187), Width = 220, ReadOnly = true, BackColor = Color.FromArgb(226, 224, 222) };
            btnBuscarProducto = new Button
            {
                Text = "Buscar",
                Location = new Point(315, 186),
                Size = new Size(70, 25),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBuscarProducto.Click += BtnBuscarProducto_Click;

            lblCantidad = new Label { Text = "Cant.:", Location = new Point(400, 190), AutoSize = true };
            nudCantidad = new NumericUpDown { Location = new Point(445, 187), Width = 80, Minimum = 1, Maximum = 1000, Value = 1 };

            btnAgregar = new Button
            {
                Text = "+ Agregar",
                Location = new Point(535, 186),
                Size = new Size(100, 25),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnAgregar.Click += BtnAgregar_Click;

            this.Controls.AddRange(new Control[] { lblProducto, txtProductoSeleccionado, btnBuscarProducto, lblCantidad, nudCantidad, btnAgregar });


            Label lblDetalle = new Label { Text = "Detalle de la Factura:", Location = new Point(20, 225), Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            dgvCarrito = new DataGridView
            {
                Location = new Point(20, 250),
                Size = new Size(700, 290),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            btnQuitarItem = new Button
            {
                Text = "Quitar Item",
                Location = new Point(20, 550),
                Size = new Size(100, 30),
                BackColor = Color.Red,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnQuitarItem.Click += BtnQuitarItem_Click;

            this.Controls.AddRange(new Control[] { lblDetalle, dgvCarrito, btnQuitarItem });

            lblTotalPagar = new Label { Text = "TOTAL A PAGAR: ₲", Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(740, 260), AutoSize = true };
            lblTotalPagarMonto = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 57, 36),
                Location = new Point(740, 285),
                AutoSize = true
            };

            lblIVAInfo = new Label { Text = "IVA incluido (10%):", Font = new Font("Segoe UI", 9), Location = new Point(740, 330), AutoSize = true };
            lblIVAMonto = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 90, 90),
                Location = new Point(740, 350),
                AutoSize = true
            };

            btnGuardarVenta = new Button
            {
                Text = "✓ GUARDAR VENTA",
                Location = new Point(740, 420),
                Size = new Size(220, 40),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnGuardarVenta.Click += BtnGuardarVenta_Click;

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(740, 470),
                Size = new Size(220, 40),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblTotalPagar, lblTotalPagarMonto, lblIVAInfo, lblIVAMonto, btnGuardarVenta, btnCancelar });
        }


        private void FormFacturacion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                string ayuda = "AYUDA - Facturación y Ventas\n\n";
                ayuda += "1. Ingresá el N° de Factura y el CI/RUC del cliente, luego presioná 'Buscar'.\n";
                ayuda += "2. Se completan automáticamente el nombre, dirección y teléfono del cliente.\n";
                ayuda += "3. Presioná 'Buscar' para elegir el producto, ingresá la Cantidad y presioná '+ Agregar'.\n";
                ayuda += "4. Repetí para cada producto.\n";
                ayuda += "5. Presioná 'Guardar Venta' para registrar la venta, descontar el stock y abrir la factura.\n";
                MessageBox.Show(ayuda, "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ConfigurarTablaCarrito()
        {
            dtCarrito = new DataTable();
            dtCarrito.Columns.Add("IdProducto", typeof(int));
            dtCarrito.Columns.Add("Codigo", typeof(string));
            dtCarrito.Columns.Add("Producto", typeof(string));
            dtCarrito.Columns.Add("Cantidad", typeof(int));
            dtCarrito.Columns.Add("PrecioUnitario", typeof(decimal));
            dtCarrito.Columns.Add("Subtotal", typeof(decimal));
            dgvCarrito.DataSource = dtCarrito;
            dgvCarrito.Columns["IdProducto"].Visible = false;
        }

        private void BtnBuscarCliente_Click(object sender, EventArgs e)
        {
            FormBuscarCliente dlg = new FormBuscarCliente();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtClienteCI.Text = dlg.ClienteSeleccionadoCI;
                txtNombreCliente.Text = dlg.ClienteSeleccionadoNombre;
                txtDireccionCliente.Text = dlg.ClienteSeleccionadoDireccion;
                txtTelefonoCliente.Text = dlg.ClienteSeleccionadoTelefono;
            }
        }

        private void BtnBuscarProducto_Click(object sender, EventArgs e)
        {
            FormBuscarProducto dlg = new FormBuscarProducto();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                idProductoBuscado = dlg.ProductoSeleccionadoId;
                nombreProductoBuscado = dlg.ProductoSeleccionadoNombre;
                txtProductoSeleccionado.Text = dlg.ProductoSeleccionadoCodigo + " - " + dlg.ProductoSeleccionadoNombre;
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (idProductoBuscado == 0)
            {
                MessageBox.Show("Busque y seleccione un producto primero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Producto p = new Producto();
            DataTable dt = p.SelectAll();
            DataRow[] filas = dt.Select($"ID = {idProductoBuscado}");

            if (filas.Length > 0)
            {
                string codigo = filas[0]["Código"].ToString();
                string nombre = filas[0]["Nombre"].ToString();
                decimal precio = Convert.ToDecimal(filas[0]["PrecioVenta"]);
                int stock = Convert.ToInt32(filas[0]["Stock"]);
                int cantidad = (int)nudCantidad.Value;

                if (cantidad > stock)
                {
                    MessageBox.Show($"Stock insuficiente. Disponible: {stock}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                decimal subtotal = cantidad * precio;
                dtCarrito.Rows.Add(idProductoBuscado, codigo, nombre, cantidad, precio, subtotal);
                CalcularTotal();

                idProductoBuscado = 0;
                nombreProductoBuscado = "";
                txtProductoSeleccionado.Clear();
                nudCantidad.Value = 1;
            }
            else
            {
                MessageBox.Show("Producto no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnQuitarItem_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.SelectedRows.Count > 0)
            {
                dgvCarrito.Rows.RemoveAt(dgvCarrito.SelectedRows[0].Index);
                CalcularTotal();
            }
            else
            {
                MessageBox.Show("Seleccione un producto.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CalcularTotal()
        {
            decimal totalPagar = 0;
            foreach (DataRow row in dtCarrito.Rows)
                totalPagar += Convert.ToDecimal(row["Subtotal"]);

            decimal totalIVA = Math.Round(totalPagar / DIVISOR_IVA_10, 0);

            lblTotalPagarMonto.Text = totalPagar.ToString("N0");
            lblIVAMonto.Text = totalIVA.ToString("N0");
        }

        private string GenerarNumeroFactura()
        {
            Venta v = new Venta();
            DataTable dt = v.Consultar();
            int siguienteId = 1;

            if (dt != null && dt.Rows.Count > 0)
                siguienteId = dt.AsEnumerable().Max(r => Convert.ToInt32(r["ID"])) + 1;

            return "F-" + siguienteId.ToString("D6");
        }

        private void BtnGuardarVenta_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreCliente.Text))
            {
                MessageBox.Show("Busque un cliente válido primero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtCarrito.Rows.Count == 0)
            {
                MessageBox.Show("El carrito está vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalPagar = 0;
            foreach (DataRow row in dtCarrito.Rows)
                totalPagar += Convert.ToDecimal(row["Subtotal"]);
            decimal totalIVA = Math.Round(totalPagar / DIVISOR_IVA_10, 0);

            Venta venta = new Venta
            {
                Fecha = DateTime.Now,
                NroFact = txtNumFactura.Text,
                RucCliente = txtClienteCI.Text,
                TotalIVA = totalIVA,
                TotalPagar = totalPagar,
                MetodoPago = cmbTipoPago.Text
            };

            int idVentaGenerada = venta.Insertar();

            if (idVentaGenerada > 0)
            {
                DataTable itemsFactura = dtCarrito.Copy();

                foreach (DataRow row in dtCarrito.Rows)
                {
                    decimal subtotalItem = Convert.ToDecimal(row["Subtotal"]);
                    decimal ivaItem = Math.Round(subtotalItem / DIVISOR_IVA_10, 0);

                    DetalleVenta dv = new DetalleVenta
                    {
                        IDVenta = idVentaGenerada,
                        IDProducto = Convert.ToInt32(row["IdProducto"]),
                        Cantidad = Convert.ToInt32(row["Cantidad"]),
                        Precio = Convert.ToDecimal(row["PrecioUnitario"]),
                        Subtotal = subtotalItem,
                        IVA = ivaItem
                    };
                    dv.InsertarYDescontarStock();
                }

                FormFacturaImpresion factura = new FormFacturaImpresion(
                    txtNumFactura.Text,
                    DateTime.Now,
                    txtNombreCliente.Text,
                    txtDireccionCliente.Text,
                    txtTelefonoCliente.Text,
                    itemsFactura,
                    totalPagar,
                    totalIVA
                );
                factura.ShowDialog();

                LimpiarFormulario();
            }
        }

        private void LimpiarFormulario()
        {
            dtCarrito.Rows.Clear();
            lblTotalPagarMonto.Text = "0";
            lblIVAMonto.Text = "0";
            txtNumFactura.Text = GenerarNumeroFactura(); txtClienteCI.Clear();
            txtNombreCliente.Clear();
            txtDireccionCliente.Clear();
            txtTelefonoCliente.Clear();
            txtProductoSeleccionado.Clear();
            idProductoBuscado = 0;
            nombreProductoBuscado = "";
            nudCantidad.Value = 1;
            cmbTipoPago.SelectedIndex = 0;
        }
    }
}