using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CafeteriaDB.Clases;

namespace CafeteriaDB.Forms
{
    public class FormUsuario : Form
    {
        private Panel pnlHeader;
        private Label lblTitulo;
        private Label lblCI, lblNombre, lblNivel, lblActivo, lblContrasena;
        private TextBox txtCI, txtNombre, txtContrasena;
        private ComboBox cmbNivel, cmbActivo;
        private Label lblContraseñaInfo;
        private Button btnGuardar, btnEditar, btnEliminar, btnCancelar;
        private DataGridView dgvUsuarios;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private Usuario usuario = new Usuario();
        private int idSeleccionado = 0;
        private bool modoEdicion = false;

        public FormUsuario()
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
            this.Text = "CafeteriaDB - Usuarios";
            this.Size = new Size(780, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(226, 224, 222);
            this.KeyPreview = true;
            this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "cafeteria.ico"));
            this.KeyDown += FormUsuario_KeyDown;
            
            // HEADER
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.FromArgb(30, 57, 36)
            };
            lblTitulo = new Label
            {
                Text = "Gestión de Usuarios",
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

            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(lblF1h);
            pnlHeader.Controls.Add(btnSalir);
            pnlHeader.Controls.Add(lblBuscar);
            pnlHeader.Controls.Add(txtBuscar);
            // PANEL FORMULARIO
            Panel pnlForm = new Panel
            {
                Location = new Point(10, 65),
                Size = new Size(275, 490),
                BackColor = Color.White
            };

            Label lSec = new Label
            {
                Text = "Datos del Usuario",
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

            lblNivel = CrearLabel("Nivel de Acceso *", 15, 162);
            cmbNivel = new ComboBox
            {
                Location = new Point(15, 182),
                Size = new Size(240, 26),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(226, 224, 222),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cmbNivel.Items.AddRange(new string[] { "Admin", "Usuario" });

            lblActivo = CrearLabel("Estado", 15, 219);
            cmbActivo = new ComboBox
            {
                Location = new Point(15, 239),
                Size = new Size(120, 26),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(226, 224, 222),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cmbActivo.Items.AddRange(new string[] { "S", "N" });
            cmbActivo.SelectedIndex = 0;

            Label lActInfo = new Label
            {
                Text = "S = Activo / N = Inactivo",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                Location = new Point(145, 245),
                Size = new Size(110, 18)
            };

            lblContrasena = CrearLabel("Contraseña *", 15, 276);
            txtContrasena = CrearTextBox(15, 296);
            txtContrasena.UseSystemPasswordChar = true;
            txtContrasena.MaxLength = 50;

            lblContraseñaInfo = new Label
            {
                Text = "(Dejar vacío para no cambiar)",
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.FromArgb(154, 114, 101),
                Location = new Point(15, 325),
                Size = new Size(240, 18),
                Visible = false
            };

            btnGuardar = CrearBoton("Guardar", Color.FromArgb(47, 108, 72), 15, 355);
            btnGuardar.Click += BtnGuardar_Click;
            btnGuardar.MouseEnter += (s, e) => btnGuardar.BackColor = Color.FromArgb(30, 57, 36);
            btnGuardar.MouseLeave += (s, e) => btnGuardar.BackColor = Color.FromArgb(47, 108, 72);

            btnCancelar = CrearBoton("Cancelar", Color.FromArgb(154, 114, 101), 140, 355);
            btnCancelar.Visible = false;
            btnCancelar.Click += (s, e) => LimpiarFormulario();
            btnCancelar.MouseEnter += (s, e) => btnCancelar.BackColor = Color.FromArgb(120, 80, 70);
            btnCancelar.MouseLeave += (s, e) => btnCancelar.BackColor = Color.FromArgb(154, 114, 101);

            Label lObl = new Label
            {
                Text = "* Campos obligatorios",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                Location = new Point(15, 400),
                Size = new Size(200, 18)
            };

            pnlForm.Controls.AddRange(new Control[] {
                lSec, sep, lblCI, txtCI, lblNombre, txtNombre,
                lblNivel, cmbNivel, lblActivo, cmbActivo, lActInfo,
                lblContrasena, txtContrasena, lblContraseñaInfo,
                btnGuardar, btnCancelar, lObl
            });

            // PANEL GRILLA
            Panel pnlGrid = new Panel
            {
                Location = new Point(295, 65),
                Size = new Size(465, 490),
                BackColor = Color.White
            };

            Label lLista = new Label
            {
                Text = "Lista de Usuarios",
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

            dgvUsuarios = new DataGridView
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
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(47, 108, 72);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = Color.FromArgb(135, 168, 152);
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 242);
            dgvUsuarios.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnEditar_Click(s, e); };

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

            Label lSeg = new Label
            {
                Text = "Contraseñas ocultas por seguridad",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(154, 114, 101),
                Location = new Point(10, 445),
                Size = new Size(280, 18)
            };

            pnlGrid.Controls.AddRange(new Control[] {
                lLista, sep2, dgvUsuarios, btnEditar, btnEliminar, lTip, lSeg
            });

            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlForm);
            this.Controls.Add(pnlGrid);
        }

        private void CargarDatos()
        {
            dgvUsuarios.DataSource = usuario.SelectAll();
        }

        private void LimpiarFormulario()
        {
            txtCI.Clear(); txtNombre.Clear(); txtContrasena.Clear();
            cmbNivel.SelectedIndex = -1;
            cmbActivo.SelectedIndex = 0;
            idSeleccionado = 0;
            modoEdicion = false;
            btnGuardar.Text = "Guardar";
            btnCancelar.Visible = false;
            lblContraseñaInfo.Visible = false;
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

            if (cmbNivel.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione el nivel de acceso.", "Campo requerido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!modoEdicion && string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("La contraseña es obligatoria para nuevos usuarios.", "Campo requerido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            usuario.CI = txtCI.Text.Trim();
            usuario.Nombre = txtNombre.Text.Trim();
            usuario.Nivel = cmbNivel.SelectedItem.ToString();
            usuario.Activo = cmbActivo.SelectedItem?.ToString() ?? "S";

            bool exito;
            if (modoEdicion)
            {
                usuario.Id = idSeleccionado;
                if (!string.IsNullOrWhiteSpace(txtContrasena.Text))
                    usuario.Contrasena = txtContrasena.Text;
                else
                {
                    Usuario u2 = new Usuario();
                    u2.SelectById(idSeleccionado);
                    usuario.Contrasena = u2.Contrasena;
                }
                exito = usuario.Update();
                if (exito) MessageBox.Show("Usuario actualizado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                usuario.Contrasena = txtContrasena.Text;
                exito = usuario.Insert();
                if (exito) MessageBox.Show("Usuario registrado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (exito) { CargarDatos(); LimpiarFormulario(); }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario.", "Sin selección",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow fila = dgvUsuarios.SelectedRows[0];
            idSeleccionado = Convert.ToInt32(fila.Cells["ID"].Value);
            txtCI.Text = fila.Cells["CI"].Value?.ToString();
            txtNombre.Text = fila.Cells["Nombre"].Value?.ToString();

            string nivel = fila.Cells["Nivel"].Value?.ToString();
            cmbNivel.SelectedIndex = nivel == "Admin" ? 0 : 1;

            string activo = fila.Cells["Activo"].Value?.ToString();
            cmbActivo.SelectedIndex = activo == "S" ? 0 : 1;

            txtContrasena.Clear();
            lblContraseñaInfo.Visible = true;

            modoEdicion = true;
            btnGuardar.Text = "Actualizar";
            btnCancelar.Visible = true;
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario.", "Sin selección",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombre = dgvUsuarios.SelectedRows[0].Cells["Nombre"].Value?.ToString();
            if (MessageBox.Show($"¿Eliminar al usuario '{nombre}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                usuario.Id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["ID"].Value);
                if (usuario.Delete())
                {
                    MessageBox.Show("Usuario eliminado.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos(); LimpiarFormulario();
                }
            }
        }

        private void FormUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                MessageBox.Show(
                    "AYUDA - Gestión de Usuarios\n\n" +
                    "• AGREGAR: complete los campos y haga clic en Guardar\n" +
                    "• EDITAR: seleccione un usuario y haga clic en Editar\n" +
                    "• ELIMINAR: seleccione un usuario y haga clic en Eliminar\n\n" +
                    "Niveles:\n" +
                    "• Admin: acceso completo\n" +
                    "• Usuario: solo Clientes y Productos\n\n" +
                    "Estado:\n" +
                    "• S = puede iniciar sesión\n" +
                    "• N = no puede iniciar sesión\n\n" +
                    "Al editar, deje la contraseña vacía para no cambiarla.",
                    "Ayuda - Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.DataSource is DataTable dt)
            {
                string filtro = txtBuscar.Text.Replace("'", "''");
                dt.DefaultView.RowFilter = string.Format("Nombre LIKE '%{0}%' OR CI LIKE '%{0}%'", filtro);
            }
        }
    }
}