using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace CafeteriaDB.Forms
{
    public partial class FormFacturaImpresion : Form
    {
        private string nroFactura;
        private DateTime fecha;
        private string clienteNombre, clienteDireccion, clienteTelefono;
        private DataTable items;
        private decimal totalPagar, totalIVA;

        private DataGridView dgvItems;
        private PrintDocument printDocument;

        public FormFacturaImpresion(string nroFactura, DateTime fecha, string clienteNombre,
            string clienteDireccion, string clienteTelefono, DataTable items, decimal totalPagar, decimal totalIVA)
        {
            this.nroFactura = nroFactura;
            this.fecha = fecha;
            this.clienteNombre = clienteNombre;
            this.clienteDireccion = clienteDireccion;
            this.clienteTelefono = clienteTelefono;
            this.items = items;
            this.totalPagar = totalPagar;
            this.totalIVA = totalIVA;

            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text = "Factura N° " + nroFactura;
            this.Size = new Size(650, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "cafeteria.ico"));
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(47, 108, 72) };
            Label lblTitulo = new Label
            {
                Text = "FACTURA",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 12),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitulo);
            this.Controls.Add(pnlHeader);

            AddInfoLabel($"Fecha: {fecha:dd/MM/yyyy}", 20, 75);
            AddInfoLabel($"N° Factura: {nroFactura}", 350, 75);
            AddInfoLabel($"Cliente: {clienteNombre}", 20, 100);
            AddInfoLabel($"Dirección: {clienteDireccion}", 20, 125);
            AddInfoLabel($"Teléfono: {clienteTelefono}", 350, 125);

            dgvItems = new DataGridView
            {
                Location = new Point(20, 160),
                Size = new Size(590, 320),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                DataSource = items
            };
            if (dgvItems.Columns.Contains("IdProducto"))
                dgvItems.Columns["IdProducto"].Visible = false;
            this.Controls.Add(dgvItems);

            Label lblTotalPagar = new Label
            {
                Text = $"TOTAL A PAGAR: ₲ {totalPagar:N0}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 57, 36),
                Location = new Point(20, 495),
                AutoSize = true
            };
            Label lblIVA = new Label
            {
                Text = $"IVA 10% incluido: ₲ {totalIVA:N0}",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(90, 90, 90),
                Location = new Point(20, 525),
                AutoSize = true
            };
            this.Controls.Add(lblTotalPagar);
            this.Controls.Add(lblIVA);

            Button btnImprimir = new Button
            {
                Text = "🖨 Imprimir",
                Location = new Point(360, 570),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(47, 108, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnImprimir.Click += BtnImprimir_Click;

            Button btnCerrar = new Button
            {
                Text = "Cerrar",
                Location = new Point(490, 570),
                Size = new Size(120, 40),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCerrar.Click += (s, e) => this.Close();

            this.Controls.Add(btnImprimir);
            this.Controls.Add(btnCerrar);

            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void AddInfoLabel(string texto, int x, int y)
        {
            Label lbl = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 10),
                Location = new Point(x, y),
                AutoSize = true
            };
            this.Controls.Add(lbl);
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            using (PrintDialog dialog = new PrintDialog())
            {
                dialog.Document = printDocument;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font fontTitulo = new Font("Arial", 16, FontStyle.Bold);
            Font fontNormal = new Font("Arial", 10);
            Font fontNegrita = new Font("Arial", 11, FontStyle.Bold);
            int y = 40;

            g.DrawString("CAFETERIA - FACTURA", fontTitulo, Brushes.Black, 40, y);
            y += 35;
            g.DrawString($"Fecha: {fecha:dd/MM/yyyy}", fontNormal, Brushes.Black, 40, y);
            g.DrawString($"N° Factura: {nroFactura}", fontNormal, Brushes.Black, 300, y);
            y += 22;
            g.DrawString($"Cliente: {clienteNombre}", fontNormal, Brushes.Black, 40, y);
            y += 22;
            g.DrawString($"Dirección: {clienteDireccion}", fontNormal, Brushes.Black, 40, y);
            y += 22;
            g.DrawString($"Teléfono: {clienteTelefono}", fontNormal, Brushes.Black, 40, y);
            y += 35;

            g.DrawString("Código", fontNegrita, Brushes.Black, 40, y);
            g.DrawString("Producto", fontNegrita, Brushes.Black, 130, y);
            g.DrawString("Cant.", fontNegrita, Brushes.Black, 350, y);
            g.DrawString("Precio", fontNegrita, Brushes.Black, 420, y);
            g.DrawString("Subtotal", fontNegrita, Brushes.Black, 500, y);
            y += 20;
            g.DrawLine(Pens.Black, 40, y, 580, y);
            y += 10;

            foreach (DataRow row in items.Rows)
            {
                g.DrawString(row["Codigo"].ToString(), fontNormal, Brushes.Black, 40, y);
                g.DrawString(row["Producto"].ToString(), fontNormal, Brushes.Black, 130, y);
                g.DrawString(row["Cantidad"].ToString(), fontNormal, Brushes.Black, 350, y);
                g.DrawString(Convert.ToDecimal(row["PrecioUnitario"]).ToString("N0"), fontNormal, Brushes.Black, 420, y);
                g.DrawString(Convert.ToDecimal(row["Subtotal"]).ToString("N0"), fontNormal, Brushes.Black, 500, y);
                y += 22;
            }

            y += 15;
            g.DrawLine(Pens.Black, 40, y, 580, y);
            y += 15;
            g.DrawString($"TOTAL A PAGAR: ₲ {totalPagar:N0}", fontNegrita, Brushes.Black, 350, y);
            y += 22;
            g.DrawString($"IVA 10% incluido: ₲ {totalIVA:N0}", fontNormal, Brushes.Black, 350, y);
        }
    }
}