using System;
using System.Data;
using System.Data.SqlClient;

namespace CafeteriaDB.Clases
{
    public class Usuario
    {
        private int id;
        private string ci;
        private string nombre;
        private string activo;
        private string nivel;
        private string contrasena;

        public int Id { get => id; set => id = value; }
        public string CI { get => ci; set => ci = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Activo { get => activo; set => activo = value; }
        public string Nivel { get => nivel; set => nivel = value; }
        public string Contrasena { get => contrasena; set => contrasena = value; }

        private Conexion conexion;

        public Usuario()
        {
            conexion = new Conexion();
        }

        public bool Insert()
        {
            try
            {
                conexion.Conectar();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Usuario (CI, Nombre, Activo, Nivel, [Contraseña]) VALUES (@ci, @nombre, @activo, @nivel, @contrasena)",
                    conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@ci", ci);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@activo", activo);
                cmd.Parameters.AddWithValue("@nivel", nivel);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);
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
                    "UPDATE Usuario SET CI=@ci, Nombre=@nombre, Activo=@activo, Nivel=@nivel, [Contraseña]=@contrasena WHERE ID=@id",
                    conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@ci", ci);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@activo", activo);
                cmd.Parameters.AddWithValue("@nivel", nivel);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);
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
                SqlCommand cmd = new SqlCommand("DELETE FROM Usuario WHERE ID=@id", conexion.ObtenerConexion());
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
                SqlCommand cmd = new SqlCommand("SELECT ID, CI, Nombre, Activo, Nivel FROM Usuario", conexion.ObtenerConexion());
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE ID=@id", conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@id", idBuscar);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    id = (int)reader["ID"];
                    ci = reader["CI"].ToString();
                    nombre = reader["Nombre"].ToString();
                    activo = reader["Activo"].ToString();
                    nivel = reader["Nivel"].ToString();
                    contrasena = reader["Contraseña"].ToString();
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

        public bool ValidarLogin(string ciLogin, string contrasenaLogin)
        {
            try
            {
                conexion.Conectar();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Usuario WHERE CI=@ci AND [Contraseña]=@contrasena AND Activo='S'",
                    conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@ci", ciLogin);
                cmd.Parameters.AddWithValue("@contrasena", contrasenaLogin);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    id = (int)reader["ID"];
                    ci = reader["CI"].ToString();
                    nombre = reader["Nombre"].ToString();
                    activo = reader["Activo"].ToString();
                    nivel = reader["Nivel"].ToString();
                    conexion.Desconectar();
                    return true;
                }
                conexion.Desconectar();
                return false;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error en login: " + ex.Message);
                return false;
            }
        }
    }
}