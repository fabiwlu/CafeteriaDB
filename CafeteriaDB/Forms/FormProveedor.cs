using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CafeteriaDB.Clases;

namespace CafeteriaDB.Forms
{
    public class FormProveedor : Form
    {
        private Panel pnlHeader;
        private Label lblTitulo;
        private Label lblCIRUC, lblNombre, lblTelefono, lblCorreo, lblDireccion, lblCiudad;
        private TextBox txtCIRUC, txtNombre, txtTelefono, txtCorreo, txtDireccion, txtCiudad;
        private Button btnGuardar, btnEditar, btnEliminar, btnCancelar;
        private DataGridView dgvProveedores;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private Proveedor proveedor = new Proveedor();
        private int idSeleccionado = 0;
        private bool modoEdicion = false;

        public FormProveedor()
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
            this.Text = "CafeteriaDB - Proveedores";
            this.Size = new Size(780, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(226, 224, 222);
            this.KeyPreview = true;
            this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "cafeteria.ico"));
            this.KeyDown += FormProveedor_KeyDown;

            // HEADER
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.FromArgb(30, 57, 36)
            };
            lblTitulo = new Label
            {
                Text = "Gestión de Proveedores",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 12),
                Size = new Size(220, 30)
            };
            lblBuscar = new Label
            {
                Text = "Buscar:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(255, 18),
                Size = new Size(65, 20)
            };
            txtBuscar = new TextBox
            {
                Location = new Point(310, 14),
                Size = new Size(215, 26),
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
                Text = "Datos del Proveedor",
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

            lblCIRUC = CrearLabel("CI / RUC *", 15, 48);
            txtCIRUC = CrearTextBox(15, 68); txtCIRUC.MaxLength = 20;

            lblNombre = CrearLabel("Nombre *", 15, 105);
            txtNombre = CrearTextBox(15, 125); txtNombre.MaxLength = 100;

            lblTelefono = CrearLabel("Teléfono", 15, 162);
            txtTelefono = CrearTextBox(15, 182); txtTelefono.MaxLength = 15;

            lblCorreo = CrearLabel("Correo", 15, 219);
            txtCorreo = CrearTextBox(15, 239); txtCorreo.MaxLength = 100;

            lblDireccion = CrearLabel("Dirección", 15, 276);
            txtDireccion = CrearTextBox(15, 296); txtDireccion.MaxLength = 200;

            lblCiudad = CrearLabel("Ciudad", 15, 333);
            txtCiudad = CrearTextBox(15, 353); txtCiudad.MaxLength = 50;

            btnGuardar = CrearBoton("Guardar", Color.FromArgb(47, 108, 72), 15, 400);
            btnGuardar.Click += BtnGuardar_Click;
            btnGuardar.MouseEnter += (s, e) => btnGuardar.BackColor = Color.FromArgb(30, 57, 36);
            btnGuardar.MouseLeave += (s, e) => btnGuardar.BackColor = Color.FromArgb(47, 108, 72);

            btnCancelar = CrearBoton("Cancelar", Color.FromArgb(154, 114, 101), 140, 400);
            btnCancelar.Visible = false;
            btnCancelar.Click += (s, e) => LimpiarFormulario();
            btnCancelar.MouseEnter += (s, e) => btnCancelar.BackColor = Color.FromArgb(120, 80, 70);
            btnCancelar.MouseLeave += (s, e) => btnCancelar.BackColor = Color.FromArgb(154, 114, 101);

            Label lObl = new Label
            {
                Text = "* Campos obligatorios",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                Location = new Point(15, 445),
                Size = new Size(200, 18)
            };

            pnlForm.Controls.AddRange(new Control[] {
                lSec, sep, lblCIRUC, txtCIRUC, lblNombre, txtNombre,
                lblTelefono, txtTelefono, lblCorreo, txtCorreo,
                lblDireccion, txtDireccion, lblCiudad, txtCiudad,
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
                Text = "Lista de Proveedores",
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

            dgvProveedores = new DataGridView
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
            dgvProveedores.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(47, 108, 72);
            dgvProveedores.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProveedores.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvProveedores.DefaultCellStyle.SelectionBackColor = Color.FromArgb(135, 168, 152);
            dgvProveedores.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvProveedores.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 242);
            dgvProveedores.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnEditar_Click(s, e); };

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
                lLista, sep2, dgvProveedores, btnEditar, btnEliminar, lTip
            });

            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlForm);
            this.Controls.Add(pnlGrid);
        }

        private void CargarDatos()
        {
            dgvProveedores.DataSource = proveedor.SelectAll();
        }

        private void LimpiarFormulario()
        {
            txtCIRUC.Clear(); txtNombre.Clear(); txtTelefono.Clear();
            txtCorreo.Clear(); txtDireccion.Clear(); txtCiudad.Clear();
            idSeleccionado = 0;
            modoEdicion = false;
            btnGuardar.Text = "Guardar";
            btnCancelar.Visible = false;
            txtCIRUC.Focus();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCIRUC.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("CI/RUC y Nombre son obligatorios.", "Campos requeridos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            proveedor.CIoRUC = txtCIRUC.Text.Trim();
            proveedor.Nombre = txtNombre.Text.Trim();
            proveedor.Telefono = txtTelefono.Text.Trim();
            proveedor.Correo = txtCorreo.Text.Trim();
            proveedor.Direccion = txtDireccion.Text.Trim();
            proveedor.Ciudad = txtCiudad.Text.Trim();

            bool exito;
            if (modoEdicion)
            {
                proveedor.Id = idSeleccionado;
                exito = proveedor.Update();
                if (exito) MessageBox.Show("Proveedor actualizado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                exito = proveedor.Insert();
                if (exito) MessageBox.Show("Proveedor registrado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (exito) { CargarDatos(); LimpiarFormulario(); }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProveedores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un proveedor.", "Sin selección",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow fila = dgvProveedores.SelectedRows[0];
            idSeleccionado = Convert.ToInt32(fila.Cells["ID"].Value);
            txtCIRUC.Text = fila.Cells["CIoRUC"].Value?.ToString();
            txtNombre.Text = fila.Cells["Nombre"].Value?.ToString();
            txtTelefono.Text = fila.Cells["Teléfono"].Value?.ToString();
            txtCorreo.Text = fila.Cells["Correo"].Value?.ToString();
            txtDireccion.Text = fila.Cells["Dirección"].Value?.ToString();
            txtCiudad.Text = fila.Cells["Ciudad"].Value?.ToString();

            modoEdicion = true;
            btnGuardar.Text = "Actualizar";
            btnCancelar.Visible = true;
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProveedores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un proveedor.", "Sin selección",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombre = dgvProveedores.SelectedRows[0].Cells["Nombre"].Value?.ToString();
            if (MessageBox.Show($"¿Eliminar al proveedor '{nombre}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                proveedor.Id = Convert.ToInt32(dgvProveedores.SelectedRows[0].Cells["ID"].Value);
                if (proveedor.Delete())
                {
                    MessageBox.Show("Proveedor eliminado.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos(); LimpiarFormulario();
                }
            }
        }

        private void FormProveedor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                MessageBox.Show(
                    "AYUDA - Gestión de Proveedores\n\n" +
                    "• AGREGAR: complete los campos y haga clic en Guardar\n" +
                    "• EDITAR: seleccione un proveedor y haga clic en Editar\n" +
                    "• ELIMINAR: seleccione un proveedor y haga clic en Eliminar\n" +
                    "• CI/RUC y Nombre son obligatorios\n" +
                    "• Solo accesible para Administradores",
                    "Ayuda - Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (dgvProveedores.DataSource is DataTable dt)
            {
                string filtro = txtBuscar.Text.Replace("'", "''");
                dt.DefaultView.RowFilter = string.Format("Nombre LIKE '%{0}%' OR CIoRUC LIKE '%{0}%'", filtro);
            }
        }
    }
}