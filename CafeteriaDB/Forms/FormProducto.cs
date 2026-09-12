using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CafeteriaDB.Clases;

namespace CafeteriaDB.Forms
{
    public class FormProducto : Form
    {
        private Panel pnlHeader;
        private Label lblTitulo;
        private Label lblCodigo, lblNombre, lblDescripcion, lblPrecio, lblStock, lblCategoria;
        private TextBox txtCodigo, txtNombre, txtDescripcion, txtPrecio, txtStock;
        private ComboBox cmbCategoria;
        private Button btnGuardar, btnEditar, btnEliminar, btnCancelar;
        private DataGridView dgvProductos;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private Producto producto = new Producto();
        private int idSeleccionado = 0;
        private bool modoEdicion = false;

        public FormProducto()
        {
            InicializarComponentes();
            CargarDatos();
        }

        private Label CrearLabel(string texto, int x, int y)
        {
            return new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 57, 36),
                Location = new Point(x, y),
                Size = new Size(240, 18)
            };
        }

        private TextBox CrearTextBox(int x, int y, int ancho = 240)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(ancho, 26),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(226, 224, 222)
            };
        }

        private Button CrearBoton(string texto, Color color, int x, int y, int ancho = 115)
        {
            var b = new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(ancho, 34),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private void InicializarComponentes()
        {
            this.Text = "CafeteriaDB - Productos";
            this.Size = new Size(780, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(226, 224, 222);
            this.KeyPreview = true;
            this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "cafeteria.ico"));
            this.KeyDown += FormProducto_KeyDown;

            // HEADER
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.FromArgb(30, 57, 36)
            };
            lblTitulo = new Label
            {
                Text = "Gestión de Productos",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 12),
                Size = new Size(200, 30)
            };
            lblBuscar = new Label
            {
                Text = "Buscar: ",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(255, 18),
                Size = new Size(55, 20)
            };
            txtBuscar = new TextBox
            {
                Location = new Point(310, 14),
                Size = new Size(140, 26),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            Label lblF1h = new Label
            {
                Text = "F1 = Ayuda",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(135, 168, 152),
                Location = new Point(560, 20),
                Size = new Size(90, 18)
            };

            Button btnSalir = new Button
            {
                Text = "Salir",
                Location = new Point(665, 10),
                Size = new Size(90, 34),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(154, 114, 101),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.Click += (s, e) => this.Close();
            btnSalir.MouseEnter += (s, e) => btnSalir.BackColor = Color.FromArgb(120, 80, 70);
            btnSalir.MouseLeave += (s, e) => btnSalir.BackColor = Color.FromArgb(154, 114, 101);

            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(lblF1h);
            pnlHeader.Controls.Add(btnSalir);
            pnlHeader.Controls.Add(lblBuscar);
            pnlHeader.Controls.Add(txtBuscar);

            // PANEL FORMULARIO
            Panel pnlForm = new Panel
            {
                Location = new Point(10, 65),
                Size = new Size(275, 500),
                BackColor = Color.White
            };

            Label lSec = new Label
            {
                Text = "Datos del Producto",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(47, 108, 72),
                Location = new Point(15, 12),
                Size = new Size(245, 20)
            };
            Label sep = new Label
            {
                BackColor = Color.FromArgb(47, 108, 72),
                Size = new Size(245, 2),
                Location = new Point(15, 35)
            };

            lblCodigo = CrearLabel("Código *", 15, 48);
            txtCodigo = CrearTextBox(15, 68); txtCodigo.MaxLength = 20;

            lblNombre = CrearLabel("Nombre *", 15, 105);
            txtNombre = CrearTextBox(15, 125); txtNombre.MaxLength = 100;

            lblDescripcion = CrearLabel("Descripción", 15, 162);
            txtDescripcion = CrearTextBox(15, 182); txtDescripcion.MaxLength = 300;

            lblPrecio = CrearLabel("Precio de Venta *", 15, 219);
            txtPrecio = CrearTextBox(15, 239, 110);

            lblStock = CrearLabel("Stock *", 135, 219);
            txtStock = CrearTextBox(135, 239, 110);

            lblCategoria = CrearLabel("Categoría", 15, 276);
            cmbCategoria = new ComboBox
            {
                Location = new Point(15, 296),
                Size = new Size(240, 26),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(226, 224, 222),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cmbCategoria.Items.AddRange(new string[] {
                "Bebidas calientes", "Bebidas frías", "Alimentos", "Postres", "Snacks", "Otro"
            });

            btnGuardar = CrearBoton("Guardar", Color.FromArgb(47, 108, 72), 15, 350);
            btnGuardar.Click += BtnGuardar_Click;
            btnGuardar.MouseEnter += (s, e) => btnGuardar.BackColor = Color.FromArgb(30, 57, 36);
            btnGuardar.MouseLeave += (s, e) => btnGuardar.BackColor = Color.FromArgb(47, 108, 72);

            btnCancelar = CrearBoton("Cancelar", Color.FromArgb(154, 114, 101), 140, 350);
            btnCancelar.Visible = false;
            btnCancelar.Click += (s, e) => LimpiarFormulario();
            btnCancelar.MouseEnter += (s, e) => btnCancelar.BackColor = Color.FromArgb(120, 80, 70);
            btnCancelar.MouseLeave += (s, e) => btnCancelar.BackColor = Color.FromArgb(154, 114, 101);

            Label lObl = new Label
            {
                Text = "* Campos obligatorios",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                Location = new Point(15, 395),
                Size = new Size(200, 18)
            };

            pnlForm.Controls.AddRange(new Control[] {
                lSec, sep, lblCodigo, txtCodigo, lblNombre, txtNombre,
                lblDescripcion, txtDescripcion, lblPrecio, txtPrecio,
                lblStock, txtStock, lblCategoria, cmbCategoria,
                btnGuardar, btnCancelar, lObl
            });

            // PANEL GRILLA
            Panel pnlGrid = new Panel
            {
                Location = new Point(295, 65),
                Size = new Size(465, 500),
                BackColor = Color.White
            };

            Label lLista = new Label
            {
                Text = "Lista de Productos",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(47, 108, 72),
                Location = new Point(10, 12),
                Size = new Size(200, 20)
            };
            Label sep2 = new Label
            {
                BackColor = Color.FromArgb(47, 108, 72),
                Size = new Size(445, 2),
                Location = new Point(10, 35)
            };

            dgvProductos = new DataGridView
            {
                Location = new Point(10, 45),
                Size = new Size(445, 370),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9)
            };
            dgvProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(47, 108, 72);
            dgvProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvProductos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(135, 168, 152);
            dgvProductos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvProductos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 242);
            dgvProductos.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnEditar_Click(s, e); };

            btnEditar = CrearBoton("Editar", Color.FromArgb(47, 108, 72), 10, 425);
            btnEditar.Click += BtnEditar_Click;
            btnEditar.MouseEnter += (s, e) => btnEditar.BackColor = Color.FromArgb(30, 57, 36);
            btnEditar.MouseLeave += (s, e) => btnEditar.BackColor = Color.FromArgb(47, 108, 72);

            btnEliminar = CrearBoton("Eliminar", Color.FromArgb(154, 114, 101), 135, 425);
            btnEliminar.Click += BtnEliminar_Click;
            btnEliminar.MouseEnter += (s, e) => btnEliminar.BackColor = Color.FromArgb(120, 80, 70);
            btnEliminar.MouseLeave += (s, e) => btnEliminar.BackColor = Color.FromArgb(154, 114, 101);

            Label lTip = new Label
            {
                Text = "Doble clic para editar rápido",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                Location = new Point(265, 435),
                Size = new Size(180, 18)
            };

            pnlGrid.Controls.AddRange(new Control[] {
                lLista, sep2, dgvProductos, btnEditar, btnEliminar, lTip
            });

            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlForm);
            this.Controls.Add(pnlGrid);
        }

        private void CargarDatos()
        {
            dgvProductos.DataSource = producto.SelectAll();
        }

        private void LimpiarFormulario()
        {
            txtCodigo.Clear(); txtNombre.Clear(); txtDescripcion.Clear();
            txtPrecio.Clear(); txtStock.Clear();
            cmbCategoria.SelectedIndex = -1;
            idSeleccionado = 0;
            modoEdicion = false;
            btnGuardar.Text = "Guardar";
            btnCancelar.Visible = false;
            txtCodigo.Focus();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Código y Nombre son obligatorios.", "Campos requeridos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio < 0)
            {
                MessageBox.Show("Ingrese un precio válido.", "Precio inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Ingrese un stock válido.", "Stock inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            producto.Codigo = txtCodigo.Text.Trim();
            producto.Nombre = txtNombre.Text.Trim();
            producto.Descripcion = txtDescripcion.Text.Trim();
            producto.PrecioVenta = precio;
            producto.Stock = stock;
            producto.Categoria = cmbCategoria.Text;

            bool exito;
            if (modoEdicion)
            {
                producto.Id = idSeleccionado;
                exito = producto.Update();
                if (exito) MessageBox.Show("Producto actualizado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                exito = producto.Insert();
                if (exito) MessageBox.Show("Producto registrado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (exito) { CargarDatos(); LimpiarFormulario(); }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto.", "Sin selección",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow fila = dgvProductos.SelectedRows[0];
            idSeleccionado = Convert.ToInt32(fila.Cells["ID"].Value);
            txtCodigo.Text = fila.Cells["Código"].Value?.ToString();
            txtNombre.Text = fila.Cells["Nombre"].Value?.ToString();
            txtDescripcion.Text = fila.Cells["Descripción"].Value?.ToString();
            txtPrecio.Text = fila.Cells["PrecioVenta"].Value?.ToString();
            txtStock.Text = fila.Cells["Stock"].Value?.ToString();

            string cat = fila.Cells["Categoría"].Value?.ToString();
            int idx = cmbCategoria.Items.IndexOf(cat);
            cmbCategoria.SelectedIndex = idx >= 0 ? idx : -1;

            modoEdicion = true;
            btnGuardar.Text = "Actualizar";
            btnCancelar.Visible = true;
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto.", "Sin selección",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombre = dgvProductos.SelectedRows[0].Cells["Nombre"].Value?.ToString();
            if (MessageBox.Show($"¿Eliminar el producto '{nombre}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                producto.Id = Convert.ToInt32(dgvProductos.SelectedRows[0].Cells["ID"].Value);
                if (producto.Delete())
                {
                    MessageBox.Show("Producto eliminado.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos(); LimpiarFormulario();
                }
            }
        }

        private void FormProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                MessageBox.Show(
                    "AYUDA - Gestión de Productos\n\n" +
                    "• AGREGAR: complete los campos y haga clic en Guardar\n" +
                    "• EDITAR: seleccione un producto y haga clic en Editar\n" +
                    "• ELIMINAR: seleccione un producto y haga clic en Eliminar\n" +
                    "• Código y Nombre son obligatorios\n" +
                    "• El precio y stock deben ser números",
                    "Ayuda - Productos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (dgvProductos.DataSource is DataTable dt)
            {
                string filtro = txtBuscar.Text.Replace("'", "''");
                dt.DefaultView.RowFilter = string.Format("Nombre LIKE '%{0}%' OR Código LIKE '%{0}%'", filtro);
            }
        }
    }
}