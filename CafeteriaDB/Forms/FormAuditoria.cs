using System;
using System.Drawing;
using System.Windows.Forms;
using CafeteriaDB.Clases;
using System.Data;
using ClosedXML.Excel;

namespace CafeteriaDB.Forms
{
    public partial class FormAuditoria : Form
    {
        private ComboBox cboFiltro;
        private DateTimePicker dtpDesde, dtpHasta;
        private Button btnBuscar;
        private DataGridView dgvAuditoria;
        private Button btnExportar, btnCerrar;
        private Auditoria auditoria = new Auditoria();

        public FormAuditoria()
        {
            InicializarComponentes();
            CargarDatos();
        }

        private void InicializarComponentes()
        {
            this.Text = "CafeteriaDB - Auditoría del Sistema";
            this.Size = new Size(900, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 245, 241);
            this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "cafeteria.ico"));

            this.KeyPreview = true;
            this.KeyDown += FormAuditoria_KeyDown;

            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.FromArgb(30, 57, 36) };
            Label lblTitulo = new Label
            {
                Text = "AUDITORÍA DEL SISTEMA",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitulo);

            Button btnSalirHeader = new Button
            {
                Text = "Salir",
                Location = new Point(780, 11),
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

            Label lblFiltro = new Label { Text = "Tabla:", Location = new Point(20, 70), AutoSize = true };
            cboFiltro = new ComboBox { Location = new Point(75, 67), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboFiltro.Items.AddRange(new string[] { "Todos", "Usuario", "Producto", "Venta" });
            cboFiltro.SelectedIndex = 0;

            this.Controls.Add(lblFiltro);
            this.Controls.Add(cboFiltro);

            Label lblDesde = new Label { Text = "Desde:", Location = new Point(20, 105), AutoSize = true };
            dtpDesde = new DateTimePicker { Location = new Point(75, 102), Width = 140, Format = DateTimePickerFormat.Short, Value = DateTime.Now.AddMonths(-1) };

            Label lblHasta = new Label { Text = "Hasta:", Location = new Point(230, 105), AutoSize = true };
            dtpHasta = new DateTimePicker { Location = new Point(280, 102), Width = 140, Format = DateTimePickerFormat.Short, Value = DateTime.Now };

            btnBuscar = new Button
            {
                Text = "Buscar",
                Location = new Point(440, 101),
                Size = new Size(90, 27),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Click += (s, e) => CargarDatos();

            this.Controls.AddRange(new Control[] { lblDesde, dtpDesde, lblHasta, dtpHasta, btnBuscar });

            dgvAuditoria = new DataGridView
            {
                Location = new Point(20, 140),
                Size = new Size(840, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            this.Controls.Add(dgvAuditoria);

            btnExportar = new Button
            {
                Text = "Exportar a Excel",
                Location = new Point(20, 505),
                Size = new Size(130, 30),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnExportar.FlatAppearance.BorderSize = 0;
            
            btnExportar.Click += BtnExportar_Click;
            btnCerrar = new Button
            {
                Text = "Cerrar",
                Location = new Point(730, 505),
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
                Location = new Point(300, 513),
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

            dgvAuditoria.DataSource = auditoria.ConsultarFiltrado(cboFiltro.Text, desde, hasta);
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            if (dgvAuditoria.Rows.Count < 1)
            {
                MessageBox.Show("No hay registros para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Tabla");
            dt.Columns.Add("Accion");
            dt.Columns.Add("Referencia");
            dt.Columns.Add("Descripcion");
            dt.Columns.Add("Fecha");

            foreach (DataGridViewRow row in dgvAuditoria.Rows)
            {
                if (row.IsNewRow) continue;

                DataRow dr = dt.NewRow();
                dr[0] = row.Cells[0].Value;
                dr[1] = row.Cells[1].Value;
                dr[2] = row.Cells[2].Value;
                dr[3] = row.Cells[3].Value;
                dr[4] = row.Cells[4].Value;
                dr[5] = row.Cells[5].Value;
                dt.Rows.Add(dr);
            }


            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "Auditoria_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
            savefile.Filter = "Excel Files | *.xlsx";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Auditoria");
                        wb.SaveAs(savefile.FileName);
                    }
                    MessageBox.Show("Reporte generado correctamente.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al generar el reporte.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void FormAuditoria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                string ayuda = "AYUDA - Auditoría del Sistema\n\n";
                ayuda += "1. Elegí una Tabla (Usuario, Producto, Venta, o Todos) para filtrar por tipo de cambio.\n";
                ayuda += "2. Elegí un rango de fechas Desde/Hasta y presioná 'Buscar'.\n";
                ayuda += "3. Usá 'Exportar a Excel' para descargar lo que estás viendo en la grilla.";
                MessageBox.Show(ayuda, "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}