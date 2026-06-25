using System;
using System.Drawing;
using System.Windows.Forms;
using CafeteriaDB.Clases;

namespace CafeteriaDB.Forms
{
    public class FormLogin : Form
    {
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblCI;
        private Label lblContrasena;
        private TextBox txtCI;
        private TextBox txtContrasena;
        private Button btnIngresar;
        private Label lblAyuda;
        private Panel pnlCard;

        private Usuario usuario = new Usuario();

        public FormLogin()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            // FORM
            this.Text = "CafeteriaDB - Iniciar Sesión";
            this.Size = new Size(440, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(47, 108, 72);
            this.KeyPreview = true;
            this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "cafeteria.ico"));
            this.KeyDown += FormLogin_KeyDown;
            this.FormClosed += FormLogin_FormClosed;

            // CARD
            pnlCard = new Panel();
            pnlCard.Size = new Size(340, 400);
            pnlCard.Location = new Point(48, 55);
            pnlCard.BackColor = Color.White;

            // TITULO
            lblTitulo = new Label();
            lblTitulo.Text = "Cafeteria";
            lblTitulo.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(30, 57, 36);
            lblTitulo.Location = new Point(20, 25);
            lblTitulo.Size = new Size(300, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

            // SUBTITULO
            lblSubtitulo = new Label();
            lblSubtitulo.Text = "Sistema de Gestión";
            lblSubtitulo.Font = new Font("Segoe UI", 10);
            lblSubtitulo.ForeColor = Color.FromArgb(135, 168, 152);
            lblSubtitulo.Location = new Point(20, 68);
            lblSubtitulo.Size = new Size(300, 20);
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;

            // SEPARADOR
            Label sep = new Label();
            sep.BackColor = Color.FromArgb(226, 224, 222);
            sep.Size = new Size(300, 1);
            sep.Location = new Point(20, 100);

            // CI
            lblCI = new Label();
            lblCI.Text = "Cédula de Identidad";
            lblCI.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblCI.ForeColor = Color.FromArgb(30, 57, 36);
            lblCI.Location = new Point(20, 115);
            lblCI.Size = new Size(200, 18);

            txtCI = new TextBox();
            txtCI.Location = new Point(20, 136);
            txtCI.Size = new Size(300, 28);
            txtCI.Font = new Font("Segoe UI", 11);
            txtCI.BorderStyle = BorderStyle.FixedSingle;
            txtCI.BackColor = Color.FromArgb(226, 224, 222);
            txtCI.MaxLength = 15;

            // CONTRASEÑA
            lblContrasena = new Label();
            lblContrasena.Text = "Contraseña";
            lblContrasena.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblContrasena.ForeColor = Color.FromArgb(30, 57, 36);
            lblContrasena.Location = new Point(20, 185);
            lblContrasena.Size = new Size(200, 18);

            txtContrasena = new TextBox();
            txtContrasena.Location = new Point(20, 206);
            txtContrasena.Size = new Size(300, 28);
            txtContrasena.Font = new Font("Segoe UI", 11);
            txtContrasena.BorderStyle = BorderStyle.FixedSingle;
            txtContrasena.BackColor = Color.FromArgb(226, 224, 222);
            txtContrasena.UseSystemPasswordChar = true;
            txtContrasena.MaxLength = 50;
            txtContrasena.KeyPress += TxtContrasena_KeyPress;

            // BOTON
            btnIngresar = new Button();
            btnIngresar.Text = "Ingresar";
            btnIngresar.Location = new Point(20, 265);
            btnIngresar.Size = new Size(300, 42);
            btnIngresar.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnIngresar.BackColor = Color.FromArgb(47, 108, 72);
            btnIngresar.ForeColor = Color.White;
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.Cursor = Cursors.Hand;
            btnIngresar.Click += BtnIngresar_Click;
            btnIngresar.MouseEnter += (s, e) => btnIngresar.BackColor = Color.FromArgb(30, 57, 36);
            btnIngresar.MouseLeave += (s, e) => btnIngresar.BackColor = Color.FromArgb(47, 108, 72);

            // AYUDA
            lblAyuda = new Label();
            lblAyuda.Text = "Presione F1 para ayuda";
            lblAyuda.Font = new Font("Segoe UI", 8);
            lblAyuda.ForeColor = Color.FromArgb(135, 168, 152);
            lblAyuda.Location = new Point(20, 330);
            lblAyuda.Size = new Size(300, 18);
            lblAyuda.TextAlign = ContentAlignment.MiddleCenter;

            // AGREGAR A CARD
            pnlCard.Controls.Add(lblTitulo);
            pnlCard.Controls.Add(lblSubtitulo);
            pnlCard.Controls.Add(sep);
            pnlCard.Controls.Add(lblCI);
            pnlCard.Controls.Add(txtCI);
            pnlCard.Controls.Add(lblContrasena);
            pnlCard.Controls.Add(txtContrasena);
            pnlCard.Controls.Add(btnIngresar);
            pnlCard.Controls.Add(lblAyuda);

            this.Controls.Add(pnlCard);
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCI.Text) || string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("Por favor ingrese CI y contraseña.", "Campos requeridos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (usuario.ValidarLogin(txtCI.Text.Trim(), txtContrasena.Text))
            {
                FormMenu menu = new FormMenu(usuario.Nivel, usuario.Nombre);
                menu.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("CI o contraseña incorrectos, o usuario inactivo.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContrasena.Clear();
                txtCI.Focus();
            }
        }

        private void TxtContrasena_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                BtnIngresar_Click(sender, e);
        }

        private void FormLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                MessageBox.Show(
                    "AYUDA - Inicio de Sesión\n\n" +
                    "1. Ingrese su CI\n" +
                    "2. Ingrese su contraseña\n" +
                    "3. Haga clic en Ingresar o presione Enter\n\n" +
                    "Niveles de acceso:\n" +
                    "• Admin: acceso completo\n" +
                    "• Usuario: solo Clientes y Productos",
                    "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.SuspendLayout();
            // 
            // FormLogin
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLogin";
            this.ResumeLayout(false);

        }
    }
}