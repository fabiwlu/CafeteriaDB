using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CafeteriaDB.Clases;
using System.Linq;

namespace CafeteriaDB.Forms
{
    public partial class FormCompra : Form
    {
        private const decimal DIVISOR_IVA_10 = 11m;

        private Label lblFecha, lblProveedor, lblNumCompra;
        private DateTimePicker dtpFecha;
        private TextBox txtProveedorSeleccionado;
        private Button btnBuscarProveedor;
        private TextBox txtNumCompra;

        private Label lblProducto, lblCantidad, lblPrecio;
        private TextBox txtProductoSeleccionado;
        private Button btnBuscarProducto;
        private NumericUpDown nudCantidad, nudPrecio;
        private Button btnAgregar;
        private int idProductoBuscado = 0;
        private string nombreProductoBuscado = "";

        private DataGridView dgvCompra;
        private DataTable dtCompra;
        private Button btnQuitarItem;

        private Label lblTotal, lblTotalMonto, lblIVAInfo, lblIVAMonto;
        private Button btnGuardarCompra, btnCancelar;

        private int idProveedorSeleccionado = 0;

        public FormCompra()
        {
            InicializarComponentes();
            ConfigurarTablaCompra();
            txtNumCompra.Text = GenerarNumeroCompra();
        }

        private void InicializarComponentes()
        {
            this.Text = "CafeteriaDB - Compra a Proveedores";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 245, 241);
            this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "cafeteria.ico"));
            this.KeyPreview = true;
            this.KeyDown += FormCompra_KeyDown;

            Panel pnlHeader = new Panel { 
                Dock = DockStyle.Top, 
                Height = 60, 
                BackColor = Color.FromArgb(30, 57, 36) };
            Label lblTitulo = new Label
            {
                Text = "REGISTRO DE COMPRA A PROVEEDORES",
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


            Button btnHistorial = new Button
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
            btnHistorial.Click += (s, e) => new FormHistorialCompras().ShowDialog();
            pnlHeader.Controls.Add(btnHistorial);

            //-----

            lblFecha = new Label { Text = "Fecha:", Location = new Point(20, 80), AutoSize = true };
            dtpFecha = new DateTimePicker { Location = new Point(80, 77), Width = 130, Format = DateTimePickerFormat.Short, Enabled = false, Value = DateTime.Now };

            lblProveedor = new Label { Text = "Proveedor:", Location = new Point(220, 80), AutoSize = true };
            txtProveedorSeleccionado = new TextBox { Location = new Point(310, 77), Width = 220, ReadOnly = true, BackColor = Color.FromArgb(226, 224, 222) };
            btnBuscarProveedor = new Button
            {
                Text = "Buscar",
                Location = new Point(535, 76),
                Size = new Size(75, 25),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBuscarProveedor.Click += BtnBuscarProveedor_Click;

            lblNumCompra = new Label { Text = "N° Compra:", Location = new Point(620, 80), AutoSize = true };
            txtNumCompra = new TextBox { Location = new Point(710, 77), Width = 150, ReadOnly = true, BackColor = Color.FromArgb(226, 224, 222) };
            this.Controls.AddRange(new Control[] { lblFecha, dtpFecha, lblProveedor, txtProveedorSeleccionado, btnBuscarProveedor, lblNumCompra, txtNumCompra });

            lblProducto = new Label { Text = "Producto:", Location = new Point(20, 130), AutoSize = true };
            txtProductoSeleccionado = new TextBox { Location = new Point(90, 127), Width = 180, ReadOnly = true, BackColor = Color.FromArgb(226, 224, 222) };
            btnBuscarProducto = new Button
            {
                Text = "Buscar",
                Location = new Point(275, 126),
                Size = new Size(70, 25),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBuscarProducto.Click += BtnBuscarProducto_Click;

            lblCantidad = new Label { Text = "Cantidad:", Location = new Point(360, 130), AutoSize = true };
            nudCantidad = new NumericUpDown { Location = new Point(450, 127), Width = 80, Minimum = 1, Maximum = 5000, Value = 1 };

            lblPrecio = new Label { Text = "Precio Unit.:", Location = new Point(540, 130), AutoSize = true };
            nudPrecio = new NumericUpDown { Location = new Point(630, 127), Width = 100, DecimalPlaces = 2, Minimum = 0, Maximum = 100000000 };

            btnAgregar = new Button
            {
                Text = "+ Agregar",
                Location = new Point(740, 126),
                Size = new Size(100, 25),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnAgregar.Click += BtnAgregar_Click;

            this.Controls.AddRange(new Control[] { lblProducto, txtProductoSeleccionado, btnBuscarProducto, lblCantidad, nudCantidad, lblPrecio, nudPrecio, btnAgregar });

            Label lblDetalle = new Label { Text = "Detalle de Compra:", Location = new Point(20, 170), Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            dgvCompra = new DataGridView
            {
                Location = new Point(20, 195),
                Size = new Size(700, 300),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            btnQuitarItem = new Button
            {
                Text = "Quitar Item",
                Location = new Point(20, 505),
                Size = new Size(100, 30),
                BackColor = Color.Red,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnQuitarItem.Click += BtnQuitarItem_Click;

            this.Controls.AddRange(new Control[] { lblDetalle, dgvCompra, btnQuitarItem });

            lblTotal = new Label { Text = "TOTAL: ₲", Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(730, 210), AutoSize = true };
            lblTotalMonto = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 57, 36),
                Location = new Point(730, 235),
                AutoSize = true
            };

            lblIVAInfo = new Label { Text = "IVA incluido (10%):", Font = new Font("Segoe UI", 9), Location = new Point(730, 270), AutoSize = true };
            lblIVAMonto = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 90, 90),
                Location = new Point(730, 288),
                AutoSize = true
            };

            btnGuardarCompra = new Button
            {
                Text = "✓ GUARDAR COMPRA",
                Location = new Point(730, 330),
                Size = new Size(230, 40),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnGuardarCompra.Click += BtnGuardarCompra_Click;

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(730, 380),
                Size = new Size(230, 40),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblTotal, lblTotalMonto, lblIVAInfo, lblIVAMonto, btnGuardarCompra, btnCancelar });
        }

        private void ConfigurarTablaCompra()
        {
            dtCompra = new DataTable();
            dtCompra.Columns.Add("IdProducto", typeof(int));
            dtCompra.Columns.Add("Producto", typeof(string));
            dtCompra.Columns.Add("Cantidad", typeof(int));
            dtCompra.Columns.Add("PrecioUnitario", typeof(decimal));
            dtCompra.Columns.Add("Subtotal", typeof(decimal));
            dgvCompra.DataSource = dtCompra;
            dgvCompra.Columns["IdProducto"].Visible = false;
        }


        private void FormCompra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                string ayuda = "AYUDA - Compra a Proveedores\n\n";
                ayuda += "1. Seleccioná el Proveedor y cargá el N° de Compra.\n";
                ayuda += "2. Buscá el producto por Código, ingresá Cantidad y Precio Unitario, y presioná '+ Agregar'.\n";
                ayuda += "3. Repetí para cada producto.\n";
                ayuda += "4. Revisá el Total y el IVA calculado automáticamente.\n";
                ayuda += "5. Presioná 'Guardar Compra' para registrar y aumentar el stock.\n";
                ayuda += "\nUsá 'Quitar Item' para sacar un producto ya agregado, o 'Cancelar' para salir sin guardar.";
                MessageBox.Show(ayuda, "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnBuscarProveedor_Click(object sender, EventArgs e)
        {
            FormBuscarProveedor dlg = new FormBuscarProveedor();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                idProveedorSeleccionado = dlg.ProveedorSeleccionadoId;
                txtProveedorSeleccionado.Text = dlg.ProveedorSeleccionadoNombre;
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

            int cantidad = (int)nudCantidad.Value;
            decimal precioUnitario = nudPrecio.Value;
            decimal subtotal = cantidad * precioUnitario;

            dtCompra.Rows.Add(idProductoBuscado, nombreProductoBuscado, cantidad, precioUnitario, subtotal);
            CalcularTotal();

            idProductoBuscado = 0;
            nombreProductoBuscado = "";
            txtProductoSeleccionado.Clear();
            nudCantidad.Value = 1;
            nudPrecio.Value = 0;
        }

        private void BtnQuitarItem_Click(object sender, EventArgs e)
        {
            if (dgvCompra.SelectedRows.Count > 0)
            {
                dgvCompra.Rows.RemoveAt(dgvCompra.SelectedRows[0].Index);
                CalcularTotal();
            }
        }

        private void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataRow row in dtCompra.Rows)
                total += Convert.ToDecimal(row["Subtotal"]);

            decimal iva = Math.Round(total / DIVISOR_IVA_10, 0);

            lblTotalMonto.Text = total.ToString("N0");
            lblIVAMonto.Text = iva.ToString("N0");
        }

        private string GenerarNumeroCompra()
        {
            Compra c = new Compra();
            DataTable dt = c.Consultar();
            int siguienteId = 1;

            if (dt != null && dt.Rows.Count > 0)
                siguienteId = dt.AsEnumerable().Max(r => Convert.ToInt32(r["ID"])) + 1;

            return "C-" + siguienteId.ToString("D6");
        }

        private void BtnGuardarCompra_Click(object sender, EventArgs e)
        {
            if (idProveedorSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un proveedor.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtCompra.Rows.Count == 0)
            {
                MessageBox.Show("Agregue productos a la compra.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal total = Convert.ToDecimal(lblTotalMonto.Text.Replace(".", ""));
            decimal iva = Math.Round(total / DIVISOR_IVA_10, 0);
            decimal subtotal = total - iva;

            Compra compra = new Compra
            {
                NumeroCompra = txtNumCompra.Text,
                IDProveedor = idProveedorSeleccionado,
                Fecha = DateTime.Now,
                Subtotal = subtotal,
                IVA = iva,
                Total = total,
                Estado = "Recibida",
                Observaciones = ""
            };

            int idCompraGenerada = compra.Insertar();

            if (idCompraGenerada > 0)
            {
                foreach (DataRow row in dtCompra.Rows)
                {
                    DetalleCompra dc = new DetalleCompra
                    {
                        IdCompra = idCompraGenerada,
                        IdProducto = Convert.ToInt32(row["IdProducto"]),
                        Cantidad = Convert.ToInt32(row["Cantidad"]),
                        PrecioUnitario = Convert.ToDecimal(row["PrecioUnitario"]),
                        Subtotal = Convert.ToDecimal(row["Subtotal"])
                    };
                    dc.InsertarYAumentarStock();
                }

                MessageBox.Show($"¡Compra registrada con éxito!\nCompra N°: {idCompraGenerada}", 
                                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
            }
        }

        private void LimpiarFormulario()
        {
            dtCompra.Rows.Clear();
            lblTotalMonto.Text = "0";
            lblIVAMonto.Text = "0";
            txtNumCompra.Text = GenerarNumeroCompra(); txtProductoSeleccionado.Clear();
            idProductoBuscado = 0;
            nombreProductoBuscado = "";
            nudCantidad.Value = 1;
            nudPrecio.Value = 0;
            txtProveedorSeleccionado.Clear();
            idProveedorSeleccionado = 0;
        }
    }
}