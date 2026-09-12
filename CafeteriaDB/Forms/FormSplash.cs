using System;
using System.Drawing;
using System.Windows.Forms;

namespace CafeteriaDB.Forms
{
    public partial class FormSplash : Form
    {
        private Timer timerCierre;

        public FormSplash()
        {
            InicializarComponentes();
        }

       
        private void InicializarComponentes()
        {
            this.Size = new Size(550, 380);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(47, 108, 72);

           

            PictureBox picLogo = new PictureBox
            {
                Image = Properties.Resources.logo_cafe,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(180, 180),
                Location = new Point(185, 45),
                BackColor = Color.Transparent
            };

            ProgressBar barraCarga = new ProgressBar
            {
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 25,
                Size = new Size(300, 8),
                Location = new Point(125, 255)
            };

            Label lblCargando = new Label
            {
                Text = "Cargando...",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(135, 168, 152),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(460, 20),
                Location = new Point(45, 275)
            };

            this.Controls.Add(picLogo);
            this.Controls.Add(barraCarga);
            this.Controls.Add(lblCargando);

            timerCierre = new Timer();
            timerCierre.Interval = 3200;
            timerCierre.Tick += (s, e) =>
            {
                timerCierre.Stop();
                this.Close();
            };
            timerCierre.Start();
        }
    }
}