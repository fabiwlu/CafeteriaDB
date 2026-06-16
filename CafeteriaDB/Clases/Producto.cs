using System;
using System.Data;
using System.Data.SqlClient;

namespace CafeteriaDB.Clases
{
    public class Producto
    {
        private int id;
        private string codigo;
        private string nombre;
        private string descripcion;
        private decimal precioVenta;
        private int stock;
        private string categoria;

        public int Id { get => id; set => id = value; }
        public string Codigo { get => codigo; set => codigo = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public decimal PrecioVenta { get => precioVenta; set => precioVenta = value; }
        public int Stock { get => stock; set => stock = value; }
        public string Categoria { get => categoria; set => categoria = value; }

        private Conexion conexion;

        public Producto()
        {
            conexion = new Conexion();
        }

        public bool Insert()
        {
            try
            {
                conexion.Conectar();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Producto ([Código], Nombre, [Descripción], PrecioVenta, Stock, [Categoría]) VALUES (@codigo, @nombre, @descripcion, @precioVenta, @stock, @categoria)",
                    conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@descripcion", descripcion ?? "");
                cmd.Parameters.AddWithValue("@precioVenta", precioVenta);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@categoria", categoria ?? "");
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
                    "UPDATE Producto SET [Código]=@codigo, Nombre=@nombre, [Descripción]=@descripcion, PrecioVenta=@precioVenta, Stock=@stock, [Categoría]=@categoria WHERE ID=@id",
                    conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@descripcion", descripcion ?? "");
                cmd.Parameters.AddWithValue("@precioVenta", precioVenta);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@categoria", categoria ?? "");
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
                SqlCommand cmd = new SqlCommand("DELETE FROM Producto WHERE ID=@id", conexion.ObtenerConexion());
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Producto", conexion.ObtenerConexion());
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Producto WHERE ID=@id", conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@id", idBuscar);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    id = (int)reader["ID"];
                    codigo = reader["Código"].ToString();
                    nombre = reader["Nombre"].ToString();
                    descripcion = reader["Descripción"].ToString();
                    precioVenta = (decimal)reader["PrecioVenta"];
                    stock = (int)reader["Stock"];
                    categoria = reader["Categoría"].ToString();
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