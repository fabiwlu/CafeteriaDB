using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CafeteriaDB.Clases;

namespace CafeteriaDB.Forms
{
    public class FormCliente : Form
    {
        private Panel pnlHeader;
        private Label lblTitulo;
        private Label lblCI, lblNombre, lblTelefono, lblCorreo, lblDireccion;
        private TextBox txtCI, txtNombre, txtTelefono, txtCorreo, txtDireccion;
        private Button btnGuardar, btnEditar, btnEliminar, btnCancelar;
        private DataGridView dgvClientes;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private Cliente cliente = new Cliente();
        private int idSeleccionado = 0;
        private bool modoEdicion = false;

        public FormCliente()
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
            this.Text = "CafeteriaDB - Clientes";
            this.Size = new Size(780, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(226, 224, 222);
            this.KeyPreview = true;
            this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "cafeteria.ico"));
            this.KeyDown += FormCliente_KeyDown;

            // HEADER
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.FromArgb(30, 57, 36)
            };
            lblTitulo = new Label
            {
                Text = "Gestión de Clientes",
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
                Size = new Size(275, 480),
                BackColor = Color.White
            };

            Label lSec = new Label
            {
                Text = "Datos del Cliente",
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

            lblCI = CrearLabel("CI *", 15, 48);
            txtCI = CrearTextBox(15, 68); txtCI.MaxLength = 15;

            lblNombre = CrearLabel("Nombre *", 15, 105);
            txtNombre = CrearTextBox(15, 125); txtNombre.MaxLength = 100;

            lblTelefono = CrearLabel("Teléfono", 15, 162);
            txtTelefono = CrearTextBox(15, 182); txtTelefono.MaxLength = 15;

            lblCorreo = CrearLabel("Correo", 15, 219);
            txtCorreo = CrearTextBox(15, 239); txtCorreo.MaxLength = 100;

            lblDireccion = CrearLabel("Dirección", 15, 276);
            txtDireccion = CrearTextBox(15, 296); txtDireccion.MaxLength = 200;

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
                lSec, sep, lblCI, txtCI, lblNombre, txtNombre,
                lblTelefono, txtTelefono, lblCorreo, txtCorreo,
                lblDireccion, txtDireccion, btnGuardar, btnCancelar, lObl
            });

            // PANEL GRILLA
            Panel pnlGrid = new Panel
            {
                Location = new Point(295, 65),
                Size = new Size(465, 480),
                BackColor = Color.White
            };

            Label lLista = new Label
            {
                Text = "Lista de Clientes",
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

            dgvClientes = new DataGridView
            {
                Location = new Point(10, 45),
                Size = new Size(445, 350),
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
            dgvClientes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(47, 108, 72);
            dgvClientes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvClientes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvClientes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(135, 168, 152);
            dgvClientes.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvClientes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 242);
            dgvClientes.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnEditar_Click(s, e); };

            btnEditar = CrearBoton("Editar", Color.FromArgb(47, 108, 72), 10, 408);
            btnEditar.Click += BtnEditar_Click;
            btnEditar.MouseEnter += (s, e) => btnEditar.BackColor = Color.FromArgb(30, 57, 36);
            btnEditar.MouseLeave += (s, e) => btnEditar.BackColor = Color.FromArgb(47, 108, 72);

            btnEliminar = CrearBoton("Eliminar", Color.FromArgb(154, 114, 101), 135, 408);
            btnEliminar.Click += BtnEliminar_Click;
            btnEliminar.MouseEnter += (s, e) => btnEliminar.BackColor = Color.FromArgb(120, 80, 70);
            btnEliminar.MouseLeave += (s, e) => btnEliminar.BackColor = Color.FromArgb(154, 114, 101);

            Label lTip = new Label
            {
                Text = "Doble clic para editar rápido",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                Location = new Point(265, 418),
                Size = new Size(180, 18)
            };

            pnlGrid.Controls.AddRange(new Control[] {
                lLista, sep2, dgvClientes, btnEditar, btnEliminar, lTip
            });

            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlForm);
            this.Controls.Add(pnlGrid);
        }

        private void CargarDatos()
        {
            dgvClientes.DataSource = cliente.SelectAll();
        }

        private void LimpiarFormulario()
        {
            txtCI.Clear(); txtNombre.Clear(); txtTelefono.Clear();
            txtCorreo.Clear(); txtDireccion.Clear();
            idSeleccionado = 0;
            modoEdicion = false;
            btnGuardar.Text = "Guardar";
            btnCancelar.Visible = false;
            txtCI.Focus();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCI.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("CI y Nombre son obligatorios.", "Campos requeridos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            cliente.CI = txtCI.Text.Trim();
            cliente.Nombre = txtNombre.Text.Trim();
            cliente.Telefono = txtTelefono.Text.Trim();
            cliente.Correo = txtCorreo.Text.Trim();
            cliente.Direccion = txtDireccion.Text.Trim();

            bool exito;
            if (modoEdicion)
            {
                cliente.Id = idSeleccionado;
                exito = cliente.Update();
                if (exito) MessageBox.Show("Cliente actualizado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                exito = cliente.Insert();
                if (exito) MessageBox.Show("Cliente registrado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (exito) { CargarDatos(); LimpiarFormulario(); }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un cliente.", "Sin selección",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow fila = dgvClientes.SelectedRows[0];
            idSeleccionado = Convert.ToInt32(fila.Cells["ID"].Value);
            txtCI.Text = fila.Cells["CI"].Value?.ToString();
            txtNombre.Text = fila.Cells["Nombre"].Value?.ToString();
            txtTelefono.Text = fila.Cells["Teléfono"].Value?.ToString();
            txtCorreo.Text = fila.Cells["Correo"].Value?.ToString();
            txtDireccion.Text = fila.Cells["Dirección"].Value?.ToString();

            modoEdicion = true;
            btnGuardar.Text = "Actualizar";
            btnCancelar.Visible = true;
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un cliente.", "Sin selección",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombre = dgvClientes.SelectedRows[0].Cells["Nombre"].Value?.ToString();
            if (MessageBox.Show($"¿Eliminar al cliente '{nombre}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cliente.Id = Convert.ToInt32(dgvClientes.SelectedRows[0].Cells["ID"].Value);
                if (cliente.Delete())
                {
                    MessageBox.Show("Cliente eliminado.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos(); LimpiarFormulario();
                }
            }
        }

        private void FormCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                MessageBox.Show(
                    "AYUDA - Gestión de Clientes\n\n" +
                    "• AGREGAR: complete los campos y haga clic en Guardar\n" +
                    "• EDITAR: seleccione un cliente y haga clic en Editar\n" +
                    "• ELIMINAR: seleccione un cliente y haga clic en Eliminar\n" +
                    "• CI y Nombre son obligatorios",
                    "Ayuda - Clientes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (dgvClientes.DataSource is DataTable dt)
            {
                string filtro = txtBuscar.Text.Replace("'", "''");
                dt.DefaultView.RowFilter = string.Format("Nombre LIKE '%{0}%' OR CI LIKE '%{0}%'", filtro);
            }
        }
    }
}