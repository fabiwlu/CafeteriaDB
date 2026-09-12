using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CafeteriaDB.Clases;

namespace CafeteriaDB.Forms
{
    public partial class FormBuscarProveedor : Form
    {
        public int ProveedorSeleccionadoId { get; private set; }
        public string ProveedorSeleccionadoNombre { get; private set; }

        private ComboBox cboOpcion;
        private TextBox txtBuscar;
        private DataGridView dgvResultados;
        private DataTable dtProveedores;

        public FormBuscarProveedor()
        {
            InicializarComponentes();
            CargarProveedores();
        }

        private void InicializarComponentes()
        {
            this.Text = "Buscar Proveedor";
            this.Size = new Size(650, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 245, 241);
            this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "cafeteria.ico"));

            Label lblOpcion = new Label { Text = "Buscar por:", Location = new Point(20, 20), AutoSize = true };
            cboOpcion = new ComboBox
            {
                Location = new Point(100, 17),
                Width = 130,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboOpcion.Items.AddRange(new string[] { "Nombre", "CI/RUC" });
            cboOpcion.SelectedIndex = 0;
            cboOpcion.SelectedIndexChanged += (s, e) => AplicarFiltro();

            txtBuscar = new TextBox { Location = new Point(240, 17), Width = 370 };
            txtBuscar.TextChanged += (s, e) => AplicarFiltro();

            dgvResultados = new DataGridView
            {
                Location = new Point(20, 55),
                Size = new Size(590, 295),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvResultados.CellDoubleClick += DgvResultados_CellDoubleClick;

            Button btnSeleccionar = new Button
            {
                Text = "Seleccionar",
                Location = new Point(400, 365),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSeleccionar.Click += (s, e) => ConfirmarSeleccion();

            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(510, 365),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(154, 114, 101),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.AddRange(new Control[] { lblOpcion, cboOpcion, txtBuscar, dgvResultados, btnSeleccionar, btnCancelar });
        }

        private void CargarProveedores()
        {
            Proveedor prov = new Proveedor();
            dtProveedores = prov.SelectAll();
            dgvResultados.DataSource = dtProveedores;
        }

        private void AplicarFiltro()
        {
            if (dtProveedores == null) return;

            string texto = txtBuscar.Text.Replace("'", "''");
            string columna = cboOpcion.Text == "Nombre" ? "Nombre" : "CIoRUC";

            dtProveedores.DefaultView.RowFilter = string.IsNullOrWhiteSpace(texto)
                ? string.Empty
                : $"{columna} LIKE '%{texto}%'";
        }

        private void DgvResultados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) ConfirmarSeleccion();
        }

        private void ConfirmarSeleccion()
        {
            if (dgvResultados.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un proveedor de la lista.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow fila = dgvResultados.SelectedRows[0];
            ProveedorSeleccionadoId = Convert.ToInt32(fila.Cells["ID"].Value);
            ProveedorSeleccionadoNombre = fila.Cells["Nombre"].Value.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}