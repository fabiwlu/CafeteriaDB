using System;
using System.Data;
using System.Data.SqlClient;

namespace CafeteriaDB.Clases
{
    public class Cliente
    {
        private int id;
        private string ci;
        private string nombre;
        private string telefono;
        private string correo;
        private string direccion;

        public int Id { get => id; set => id = value; }
        public string CI { get => ci; set => ci = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Correo { get => correo; set => correo = value; }
        public string Direccion { get => direccion; set => direccion = value; }

        private Conexion conexion;

        public Cliente()
        {
            conexion = new Conexion();
        }

        public bool Insert()
        {
            try
            {
                conexion.Conectar();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Cliente (CI, Nombre, [Teléfono], Correo, [Dirección]) VALUES (@ci, @nombre, @telefono, @correo, @direccion)",
                    conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@ci", ci);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@telefono", telefono ?? "");
                cmd.Parameters.AddWithValue("@correo", correo ?? "");
                cmd.Parameters.AddWithValue("@direccion", direccion ?? "");
                cmd.ExecuteNonQuery();
                conexion.Desconectar();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al insertar: " + ex.Message);
                return false;
            }
        }

        public bool Update()
        {
            try
            {
                conexion.Conectar();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Cliente SET CI=@ci, Nombre=@nombre, [Teléfono]=@telefono, Correo=@correo, [Dirección]=@direccion WHERE ID=@id",
                    conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@ci", ci);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@telefono", telefono ?? "");
                cmd.Parameters.AddWithValue("@correo", correo ?? "");
                cmd.Parameters.AddWithValue("@direccion", direccion ?? "");
                cmd.ExecuteNonQuery();
                conexion.Desconectar();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al actualizar: " + ex.Message);
                return false;
            }
        }

        public bool Delete()
        {
            try
            {
                conexion.Conectar();
                SqlCommand cmd = new SqlCommand("DELETE FROM Cliente WHERE ID=@id", conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conexion.Desconectar();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al eliminar: " + ex.Message);
                return false;
            }
        }

        public DataTable SelectAll()
        {
            try
            {
                conexion.Conectar();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Cliente", conexion.ObtenerConexion());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conexion.Desconectar();
                return dt;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al consultar: " + ex.Message);
                return null;
            }
        }

        public bool SelectById(int idBuscar)
        {
            try
            {
                conexion.Conectar();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Cliente WHERE ID=@id", conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@id", idBuscar);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    id = (int)reader["ID"];
                    ci = reader["CI"].ToString();
                    nombre = reader["Nombre"].ToString();
                    telefono = reader["Teléfono"].ToString();
                    correo = reader["Correo"].ToString();
                    direccion = reader["Dirección"].ToString();
                    conexion.Desconectar();
                    return true;
                }
                conexion.Desconectar();
                return false;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al buscar: " + ex.Message);
                return false;
            }
        }
    }
}