using System;
using System.Data;
using System.Data.SqlClient;

namespace CafeteriaDB.Clases
{
    public class Auditoria
    {
        private Conexion conexion;

        public Auditoria()
        {
            conexion = new Conexion();
        }

        public DataTable Consultar()
        {
            try
            {
                conexion.Conectar();
                string query = "SELECT ID, Tabla, Accion, Referencia, Descripcion, Fecha FROM Auditoria ORDER BY Fecha DESC";
                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conexion.Desconectar();
                return dt;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al consultar auditoría: " + ex.Message);
                conexion.Desconectar();
                return null;
            }
        }

        public DataTable ConsultarPorTabla(string tabla)
        {
            try
            {
                conexion.Conectar();
                string query = "SELECT ID, Tabla, Accion, Referencia, Descripcion, Fecha FROM Auditoria WHERE Tabla = @tabla ORDER BY Fecha DESC";
                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@tabla", tabla);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conexion.Desconectar();
                return dt;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al consultar auditoría: " + ex.Message);
                conexion.Desconectar();
                return null;
            }
        }

        public DataTable ConsultarFiltrado(string tabla, DateTime desde, DateTime hasta)
        {
            try
            {
                conexion.Conectar();
                string query = @"SELECT ID, Tabla, Accion, Referencia, Descripcion, Fecha FROM Auditoria
                               WHERE Fecha BETWEEN @desde AND @hasta";
                if (tabla != "Todos")
                    query += " AND Tabla = @tabla";
                query += " ORDER BY Fecha DESC";

                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@desde", desde);
                cmd.Parameters.AddWithValue("@hasta", hasta);
                if (tabla != "Todos")
                    cmd.Parameters.AddWithValue("@tabla", tabla);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conexion.Desconectar();
                return dt;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al consultar auditoría: " + ex.Message);
                conexion.Desconectar();
                return null;
            }
        }
    }
}