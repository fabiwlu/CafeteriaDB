using System;
using System.Windows.Forms;

namespace CafeteriaDB
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (Forms.FormSplash splash = new Forms.FormSplash())
            {
                splash.ShowDialog();
            }

            Application.Run(new Forms.FormLogin());
        }
    }
}