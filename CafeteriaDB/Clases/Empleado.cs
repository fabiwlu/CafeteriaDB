using System;
using System.Data;
using System.Data.SqlClient;

namespace CafeteriaDB.Clases
{
    public class Empleado
    {
        public int ID { get; set; }
        public string CI { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Puesto { get; set; }
        public decimal Salario { get; set; }
        public DateTime FechaIngreso { get; set; }

        private Conexion conexion;

        public Empleado()
        {
            conexion = new Conexion();
        }

        public bool Insertar()
        {
            try
            {
                conexion.Conectar();
                string query = @"INSERT INTO Empleado (CI, Nombre, [Teléfono], Puesto, Salario, FechaIngreso) 
                               VALUES (@ci, @nombre, @telefono, @puesto, @salario, @fechaIngreso)";
                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@ci", CI);
                cmd.Parameters.AddWithValue("@nombre", Nombre);
                cmd.Parameters.AddWithValue("@telefono", Telefono ?? "");
                cmd.Parameters.AddWithValue("@puesto", Puesto ?? "");
                cmd.Parameters.AddWithValue("@salario", Salario);
                cmd.Parameters.AddWithValue("@fechaIngreso", FechaIngreso);
                cmd.ExecuteNonQuery();
                conexion.Desconectar();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al insertar empleado: " + ex.Message);
                conexion.Desconectar();
                return false;
            }
        }

        public bool Actualizar()
        {
            try
            {
                conexion.Conectar();
                string query = @"UPDATE Empleado SET CI=@ci, Nombre=@nombre, [Teléfono]=@telefono, Puesto=@puesto, Salario=@salario WHERE ID=@id";
                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@id", ID);
                cmd.Parameters.AddWithValue("@ci", CI);
                cmd.Parameters.AddWithValue("@nombre", Nombre);
                cmd.Parameters.AddWithValue("@telefono", Telefono ?? "");
                cmd.Parameters.AddWithValue("@puesto", Puesto ?? "");
                cmd.Parameters.AddWithValue("@salario", Salario);
                cmd.ExecuteNonQuery();
                conexion.Desconectar();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al actualizar empleado: " + ex.Message);
                conexion.Desconectar();
                return false;
            }
        }

        public bool Eliminar()
        {
            try
            {
                conexion.Conectar();
                string query = "DELETE FROM Empleado WHERE ID=@id";
                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@id", ID);
                cmd.ExecuteNonQuery();
                conexion.Desconectar();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al eliminar empleado: " + ex.Message);
                conexion.Desconectar();
                return false;
            }
        }

        public DataTable Consultar()
        {
            try
            {
                conexion.Conectar();
                string query = "SELECT ID, CI, Nombre, [Teléfono] AS Telefono, Puesto, Salario, FechaIngreso FROM Empleado ORDER BY Nombre";
                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conexion.Desconectar();
                return dt;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al consultar empleados: " + ex.Message);
                conexion.Desconectar();
                return null;
            }
        }
    }
}