using System;
using System.Drawing;
using System.Windows.Forms;

namespace CafeteriaDB.Forms
{
    public class FormMenu : Form
    {
        private string nivelUsuario;
        private string nombreUsuario;

        private Panel pnlHeader;
        private Label lblTitulo;
        private Label lblBienvenida;
        private Label lblNivel;
        private Button btnClientes;
        private Button btnProductos;
        private Button btnProveedores;
        private Button btnUsuarios;
        private Button btnSalir;

        public FormMenu(string nivel, string nombre)
        {
            this.nivelUsuario = nivel;
            this.nombreUsuario = nombre;
            InicializarComponentes();
            ConfigurarSegunNivel();
        }

        private Button CrearBotonMenu(string texto, string icono, int x, int y)
        {
            Button btn = new Button();
            btn.Text = icono + "\n" + texto;
            btn.Location = new Point(x, y);
            btn.Size = new Size(150, 110);
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.BackColor = Color.FromArgb(47, 108, 72);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(30, 57, 36);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(47, 108, 72);
            return btn;
        }

        private void InicializarComponentes()
        {
            // FORM
            this.Text = "CafeteriaDB - Menú Principal";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(226, 224, 222);
            this.KeyPreview = true;
            this.KeyDown += FormMenu_KeyDown;
            this.FormClosed += FormMenu_FormClosed;

            // HEADER
            pnlHeader = new Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 80;
            pnlHeader.BackColor = Color.FromArgb(30, 57, 36);

            lblTitulo = new Label();
            lblTitulo.Text = "Cafeteria";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 8);
            lblTitulo.Size = new Size(300, 35);

            lblBienvenida = new Label();
            lblBienvenida.Text = "Bienvenido/a, " + nombreUsuario;
            lblBienvenida.Font = new Font("Segoe UI", 10);
            lblBienvenida.ForeColor = Color.FromArgb(135, 168, 152);
            lblBienvenida.Location = new Point(20, 48);
            lblBienvenida.Size = new Size(350, 20);

            lblNivel = new Label();
            lblNivel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblNivel.ForeColor = Color.FromArgb(226, 224, 222);
            lblNivel.Location = new Point(390, 48);
            lblNivel.Size = new Size(170, 20);
            lblNivel.TextAlign = ContentAlignment.MiddleRight;

            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(lblBienvenida);
            pnlHeader.Controls.Add(lblNivel);

            // LABEL MODULOS
            Label lblModulos = new Label();
            lblModulos.Text = "Módulos del Sistema";
            lblModulos.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblModulos.ForeColor = Color.FromArgb(30, 57, 36);
            lblModulos.Location = new Point(30, 100);
            lblModulos.Size = new Size(300, 25);

            // BOTONES
            btnClientes = CrearBotonMenu("Clientes", "👥", 30, 135);
            btnClientes.Click += (s, e) => { new FormCliente().ShowDialog(); };

            btnProductos = CrearBotonMenu("Productos", "🛒", 195, 135);
            btnProductos.Click += (s, e) => { new FormProducto().ShowDialog(); };

            btnProveedores = CrearBotonMenu("Proveedores", "🚚", 360, 135);
            btnProveedores.Click += (s, e) => { new FormProveedor().ShowDialog(); };

            btnUsuarios = CrearBotonMenu("Usuarios", "🔑", 30, 260);
            btnUsuarios.Click += (s, e) => { new FormUsuario().ShowDialog(); };

            // BOTON SALIR
            btnSalir = new Button();
            btnSalir.Text = "Cerrar Sesión";
            btnSalir.Location = new Point(360, 260);
            btnSalir.Size = new Size(150, 50);
            btnSalir.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSalir.BackColor = Color.FromArgb(154, 114, 101);
            btnSalir.ForeColor = Color.White;
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.Cursor = Cursors.Hand;
            btnSalir.Click += BtnSalir_Click;
            btnSalir.MouseEnter += (s, e) => btnSalir.BackColor = Color.FromArgb(120, 80, 70);
            btnSalir.MouseLeave += (s, e) => btnSalir.BackColor = Color.FromArgb(154, 114, 101);

            // F1
            Label lblF1 = new Label();
            lblF1.Text = "Presione F1 para ayuda";
            lblF1.Font = new Font("Segoe UI", 8);
            lblF1.ForeColor = Color.FromArgb(135, 168, 152);
            lblF1.Location = new Point(30, 390);
            lblF1.Size = new Size(200, 18);

            this.Controls.Add(pnlHeader);
            this.Controls.Add(lblModulos);
            this.Controls.Add(btnClientes);
            this.Controls.Add(btnProductos);
            this.Controls.Add(btnProveedores);
            this.Controls.Add(btnUsuarios);
            this.Controls.Add(btnSalir);
            this.Controls.Add(lblF1);
        }

        private void ConfigurarSegunNivel()
        {
            if (nivelUsuario == "Admin")
            {
                lblNivel.Text = "🔑 Administrador";
                btnProveedores.Visible = true;
                btnUsuarios.Visible = true;
            }
            else
            {
                lblNivel.Text = "👤 Usuario";
                btnProveedores.Visible = false;
                btnUsuarios.Visible = false;
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar sesión?", "Cerrar sesión",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                new FormLogin().Show();
                this.Close();
            }
        }

        private void FormMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void FormMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                string ayuda = "AYUDA - Menú Principal\n\n";
                ayuda += "• Clientes: gestión de clientes\n";
                ayuda += "• Productos: gestión de productos\n";
                if (nivelUsuario == "Admin")
                {
                    ayuda += "• Proveedores: gestión de proveedores\n";
                    ayuda += "• Usuarios: gestión de usuarios\n";
                }
                MessageBox.Show(ayuda, "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}