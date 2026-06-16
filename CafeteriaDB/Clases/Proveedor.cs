using System;
using System.Data;
using System.Data.SqlClient;

namespace CafeteriaDB.Clases
{
    public class Proveedor
    {
        private int id;
        private string ciRuc;
        private string nombre;
        private string telefono;
        private string correo;
        private string direccion;
        private string ciudad;

        public int Id { get => id; set => id = value; }
        public string CIoRUC { get => ciRuc; set => ciRuc = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Correo { get => correo; set => correo = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public string Ciudad { get => ciudad; set => ciudad = value; }

        private Conexion conexion;

        public Proveedor()
        {
            conexion = new Conexion();
        }

        public bool Insert()
        {
            try
            {
                conexion.Conectar();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Proveedor (CIoRUC, Nombre, [Teléfono], Correo, [Dirección], Ciudad) VALUES (@ciRuc, @nombre, @telefono, @correo, @direccion, @ciudad)",
                    conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@ciRuc", ciRuc);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@telefono", telefono ?? "");
                cmd.Parameters.AddWithValue("@correo", correo ?? "");
                cmd.Parameters.AddWithValue("@direccion", direccion ?? "");
                cmd.Parameters.AddWithValue("@ciudad", ciudad ?? "");
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
                    "UPDATE Proveedor SET CIoRUC=@ciRuc, Nombre=@nombre, [Teléfono]=@telefono, Correo=@correo, [Dirección]=@direccion, Ciudad=@ciudad WHERE ID=@id",
                    conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@ciRuc", ciRuc);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@telefono", telefono ?? "");
                cmd.Parameters.AddWithValue("@correo", correo ?? "");
                cmd.Parameters.AddWithValue("@direccion", direccion ?? "");
                cmd.Parameters.AddWithValue("@ciudad", ciudad ?? "");
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
                SqlCommand cmd = new SqlCommand("DELETE FROM Proveedor WHERE ID=@id", conexion.ObtenerConexion());
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Proveedor", conexion.ObtenerConexion());
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Proveedor WHERE ID=@id", conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@id", idBuscar);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    id = (int)reader["ID"];
                    ciRuc = reader["CIoRUC"].ToString();
                    nombre = reader["Nombre"].ToString();
                    telefono = reader["Teléfono"].ToString();
                    correo = reader["Correo"].ToString();
                    direccion = reader["Dirección"].ToString();
                    ciudad = reader["Ciudad"].ToString();
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