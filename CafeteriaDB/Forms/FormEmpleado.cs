using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CafeteriaDB.Clases;

namespace CafeteriaDB.Forms
{
    public partial class FormEmpleado : Form   // <-- se agregó "partial"
    {
        private DataGridView dgvEmpleados;
        private TextBox txtCI, txtNombre, txtTelefono, txtPuesto;
        private NumericUpDown nudSalario;
        private DateTimePicker dtpFechaIngreso;
        private Button btnAgregar, btnActualizar, btnEliminar, btnCerrar;
        private Empleado empleado = new Empleado();
        private int idSeleccionado = 0;

        public FormEmpleado()
        {
            InicializarComponentes();
            CargarDatos();
        }

        private void InicializarComponentes()
        {
            this.Text = "CafeteriaDB - Empleados";
            this.Size = new Size(900, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 245, 241);
            this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "cafeteria.ico"));
            this.KeyPreview = true;
            this.KeyDown += FormEmpleado_KeyDown;

            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(30, 57, 36)
            };
            Label lblTitulo = new Label
            {
                Text = "GESTIÓN DE EMPLEADOS",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 12),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitulo);
            this.Controls.Add(pnlHeader);


            Button btnSalirHeader = new Button
            {
                Text = "Salir",
                Location = new Point(770, 8),
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



            Panel pnlEntrada = new Panel
            {
                Location = new Point(20, 70),
                Size = new Size(380, 350),
                BorderStyle = BorderStyle.FixedSingle
            };

            int yPos = 10;

            pnlEntrada.Controls.Add(new Label { Text = "CI:", Location = new Point(10, yPos), AutoSize = true });
            txtCI = new TextBox { Location = new Point(80, yPos), Width = 280 };
            pnlEntrada.Controls.Add(txtCI);
            yPos += 30;

            pnlEntrada.Controls.Add(new Label { Text = "Nombre:", Location = new Point(10, yPos), AutoSize = true });
            txtNombre = new TextBox { Location = new Point(80, yPos), Width = 280 };
            pnlEntrada.Controls.Add(txtNombre);
            yPos += 30;

            pnlEntrada.Controls.Add(new Label { Text = "Teléfono:", Location = new Point(10, yPos), AutoSize = true });
            txtTelefono = new TextBox { Location = new Point(80, yPos), Width = 280 };
            pnlEntrada.Controls.Add(txtTelefono);
            yPos += 30;

            pnlEntrada.Controls.Add(new Label { Text = "Puesto:", Location = new Point(10, yPos), AutoSize = true });
            txtPuesto = new TextBox { Location = new Point(80, yPos), Width = 280 };
            pnlEntrada.Controls.Add(txtPuesto);
            yPos += 30;

            pnlEntrada.Controls.Add(new Label { Text = "Salario:", Location = new Point(10, yPos), AutoSize = true });
            nudSalario = new NumericUpDown { Location = new Point(80, yPos), Width = 280, DecimalPlaces = 0, Maximum = 100000000 };
            pnlEntrada.Controls.Add(nudSalario);
            yPos += 30;

            pnlEntrada.Controls.Add(new Label { Text = "Ingreso:", Location = new Point(10, yPos), AutoSize = true });
            dtpFechaIngreso = new DateTimePicker { Location = new Point(80, yPos), Width = 280, Value = DateTime.Now };
            pnlEntrada.Controls.Add(dtpFechaIngreso);
            yPos += 40;

            btnAgregar = new Button { Text = "Agregar", Location = new Point(10, yPos), Width = 85, BackColor = Color.FromArgb(47, 108, 72), ForeColor = Color.White };
            btnAgregar.Click += BtnAgregar_Click;
            btnActualizar = new Button { Text = "Actualizar", Location = new Point(105, yPos), Width = 85, BackColor = Color.Orange, ForeColor = Color.White };
            btnActualizar.Click += BtnActualizar_Click;
            btnEliminar = new Button { Text = "Eliminar", Location = new Point(200, yPos), Width = 85, BackColor = Color.Red, ForeColor = Color.White };
            btnEliminar.Click += BtnEliminar_Click;
            btnCerrar = new Button { Text = "Cerrar", Location = new Point(10, yPos + 40), Width = 275, BackColor = Color.Gray, ForeColor = Color.White};
            btnCerrar.Click += (s, e) => this.Close();


            pnlEntrada.Controls.Add(btnCerrar);
            pnlEntrada.Controls.Add(btnAgregar);
            pnlEntrada.Controls.Add(btnActualizar);
            pnlEntrada.Controls.Add(btnEliminar);

            this.Controls.Add(pnlEntrada);

            dgvEmpleados = new DataGridView
            {
                Location = new Point(420, 70),
                Size = new Size(450, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvEmpleados.CellClick += DgvEmpleados_CellClick;
            this.Controls.Add(dgvEmpleados);
            Label lblF1 = new Label
            {
                Text = "Presione F1 para ayuda",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(135, 168, 152),
                Location = new Point(20, 430),
                AutoSize = true
            };
            this.Controls.Add(lblF1);
        }


        private void FormEmpleado_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                string ayuda = "AYUDA - Gestión de Empleados\n\n";
                ayuda += "1. Completá CI, Nombre, Teléfono, Puesto, Salario e Ingreso, y presioná 'Agregar'.\n";
                ayuda += "2. Para editar, hacé click en un empleado de la lista (se cargan sus datos), modificá lo que necesites y presioná 'Actualizar'.\n";
                ayuda += "3. Para eliminar, seleccioná un empleado de la lista y presioná 'Eliminar'.\n";
                ayuda += "4. Presioná 'Cerrar' para volver al menú principal.";
                MessageBox.Show(ayuda, "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CargarDatos()
        {
            DataTable dt = empleado.Consultar();
            dgvEmpleados.DataSource = dt;
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCI.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Ingrese CI y Nombre.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Empleado emp = new Empleado
            {
                CI = txtCI.Text,
                Nombre = txtNombre.Text,
                Telefono = txtTelefono.Text,
                Puesto = txtPuesto.Text,
                Salario = nudSalario.Value,
                FechaIngreso = dtpFechaIngreso.Value
            };

            if (emp.Insertar())
            {
                MessageBox.Show("Empleado agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarDatos();
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un empleado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Empleado emp = new Empleado
            {
                ID = idSeleccionado,
                CI = txtCI.Text,
                Nombre = txtNombre.Text,
                Telefono = txtTelefono.Text,
                Puesto = txtPuesto.Text,
                Salario = nudSalario.Value
            };

            if (emp.Actualizar())
            {
                MessageBox.Show("Empleado actualizado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarDatos();
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un empleado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Empleado emp = new Empleado { ID = idSeleccionado };
                if (emp.Eliminar())
                {
                    MessageBox.Show("Empleado eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarDatos();
                }
            }
        }

        private void DgvEmpleados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvEmpleados.Rows[e.RowIndex];
                idSeleccionado = Convert.ToInt32(fila.Cells["ID"].Value);
                txtCI.Text = fila.Cells["CI"].Value.ToString();
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtTelefono.Text = fila.Cells["Telefono"].Value?.ToString() ?? "";
                txtPuesto.Text = fila.Cells["Puesto"].Value?.ToString() ?? "";
                nudSalario.Value = Convert.ToDecimal(fila.Cells["Salario"].Value);
                dtpFechaIngreso.Value = Convert.ToDateTime(fila.Cells["FechaIngreso"].Value);
            }
        }

        private void LimpiarCampos()
        {
            txtCI.Clear();
            txtNombre.Clear();
            txtTelefono.Clear();
            txtPuesto.Clear();
            nudSalario.Value = 0;
            dtpFechaIngreso.Value = DateTime.Now;
            idSeleccionado = 0;
        }
    }
}