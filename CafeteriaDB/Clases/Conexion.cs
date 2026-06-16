using System;
using System.Data.SqlClient;

namespace CafeteriaDB.Clases
{
    public class Conexion
    {
        private string cadenaConexion = "Data Source=localhost\\SQL2022;Initial Catalog=CafeteriaDB;User ID=sa;Password=1234;";
        private SqlConnection conexion;

        public Conexion()
        {
            conexion = new SqlConnection(cadenaConexion);
        }

        public SqlConnection ObtenerConexion()
        {
            return conexion;
        }

        public bool Conectar()
        {
            try
            {
                if (conexion.State == System.Data.ConnectionState.Closed)
                    conexion.Open();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al conectar: " + ex.Message);
                return false;
            }
        }

        public void Desconectar()
        {
            try
            {
                if (conexion.State == System.Data.ConnectionState.Open)
                    conexion.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al desconectar: " + ex.Message);
            }
        }
    }
}
