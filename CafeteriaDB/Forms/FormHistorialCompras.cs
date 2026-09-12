using System;
using System.Drawing;
using System.Windows.Forms;
using CafeteriaDB.Clases;

namespace CafeteriaDB.Forms
{
    public partial class FormHistorialCompras : Form
    {
        private DateTimePicker dtpDesde, dtpHasta;
        private Button btnBuscar;
        private DataGridView dgvCompras;
        private Button btnExportar, btnCerrar;
        private Compra compra = new Compra();

        public FormHistorialCompras()
        {
            InicializarComponentes();
            CargarDatos();
        }

        private void InicializarComponentes()
        {
            this.Text = "CafeteriaDB - Historial de Compras";
            this.Size = new Size(950, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 245, 241);
            this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "cafeteria.ico"));

            this.KeyPreview = true;
            this.KeyDown += FormHistorialCompras_KeyDown;

            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.FromArgb(30, 57, 36) };
            Label lblTitulo = new Label
            {
                Text = "HISTORIAL DE COMPRAS",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitulo);

            Button btnSalirHeader = new Button
            {
                Text = "Salir",
                Location = new Point(830, 11),
                Size = new Size(90, 34),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(154, 114, 101),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSalirHeader.FlatAppearance.BorderSize = 0;
            btnSalirHeader.Click += (s, e) => this.Close();
            pnlHeader.Controls.Add(btnSalirHeader);

            this.Controls.Add(pnlHeader);

            Label lblDesde = new Label { Text = "Desde:", Location = new Point(20, 70), AutoSize = true };
            dtpDesde = new DateTimePicker { Location = new Point(75, 67), Width = 140, Format = DateTimePickerFormat.Short, Value = DateTime.Now.AddMonths(-1) };

            Label lblHasta = new Label { Text = "Hasta:", Location = new Point(230, 70), AutoSize = true };
            dtpHasta = new DateTimePicker { Location = new Point(280, 67), Width = 140, Format = DateTimePickerFormat.Short, Value = DateTime.Now };

            btnBuscar = new Button
            {
                Text = "Buscar",
                Location = new Point(440, 66),
                Size = new Size(90, 27),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Click += (s, e) => CargarDatos();

            this.Controls.AddRange(new Control[] { lblDesde, dtpDesde, lblHasta, dtpHasta, btnBuscar });

            dgvCompras = new DataGridView
            {
                Location = new Point(20, 105),
                Size = new Size(890, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            this.Controls.Add(dgvCompras);

            btnExportar = new Button
            {
                Text = "Exportar a Excel",
                Location = new Point(20, 470),
                Size = new Size(130, 30),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnExportar.FlatAppearance.BorderSize = 0;
            btnExportar.Click += (s, e) => ExcelExportHelper.ExportarGrilla(dgvCompras, "HistorialCompras", "Compras");

            btnCerrar = new Button
            {
                Text = "Cerrar",
                Location = new Point(750, 470),
                Size = new Size(130, 30),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += (s, e) => this.Close();

            this.Controls.Add(btnExportar);
            this.Controls.Add(btnCerrar);

            Label lblF1 = new Label
            {
                Text = "Presione F1 para ayuda",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(135, 168, 152),
                Location = new Point(300, 480),
                AutoSize = true
            };
            this.Controls.Add(lblF1);
        }

        private void CargarDatos()
        {
            if (dtpDesde.Value.Date > dtpHasta.Value.Date)
            {
                MessageBox.Show("La fecha 'Desde' no puede ser posterior a 'Hasta'.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime desde = dtpDesde.Value.Date;
            DateTime hasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1);

            dgvCompras.DataSource = compra.ConsultarPorFecha(desde, hasta);
        }

        private void FormHistorialCompras_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                string ayuda = "AYUDA - Historial de Compras\n\n";
                ayuda += "1. Elegí un rango de fechas Desde/Hasta y presioná 'Buscar'.\n";
                ayuda += "2. Usá 'Exportar a Excel' para descargar lo que estás viendo en la grilla.";
                MessageBox.Show(ayuda, "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}