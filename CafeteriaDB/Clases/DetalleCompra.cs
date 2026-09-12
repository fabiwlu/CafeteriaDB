using System;
using System.Data;
using System.Data.SqlClient;

namespace CafeteriaDB.Clases
{
    public class DetalleCompra
    {
        public int ID { get; set; }
        public int IdCompra { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        private Conexion conexion;

        public DetalleCompra()
        {
            conexion = new Conexion();
        }

        public bool InsertarYAumentarStock()
        {
            try
            {
                conexion.Conectar();
                string queryDetalle = @"INSERT INTO Detalle_Compra (IDCompra, IDProducto, Cantidad, PrecioUnitario, Subtotal) 
                               VALUES (@idCompra, @idProducto, @cantidad, @precioUnitario, @subtotal)";
                SqlCommand cmdDetalle = new SqlCommand(queryDetalle, conexion.ObtenerConexion());
                cmdDetalle.Parameters.AddWithValue("@idCompra", IdCompra);
                cmdDetalle.Parameters.AddWithValue("@idProducto", IdProducto);
                cmdDetalle.Parameters.AddWithValue("@cantidad", Cantidad);
                cmdDetalle.Parameters.AddWithValue("@precioUnitario", PrecioUnitario);
                cmdDetalle.Parameters.AddWithValue("@subtotal", Subtotal);
                cmdDetalle.ExecuteNonQuery();

                // El aumento de stock  hace el trigger TR_AumentarStock_Compra
                conexion.Desconectar();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error en detalle de compra: " + ex.Message);
                conexion.Desconectar();
                return false;
            }
        }

        public DataTable ConsultarPorCompra(int idCompra)
        {
            try
            {
                conexion.Conectar();
                string query = @"SELECT dc.ID, p.Nombre AS Producto, dc.Cantidad, dc.PrecioUnitario, dc.Subtotal 
                               FROM Detalle_Compra dc
                               INNER JOIN Producto p ON dc.IDProducto = p.ID
                               WHERE dc.IDCompra = @idCompra";
                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@idCompra", idCompra);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conexion.Desconectar();
                return dt;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al consultar detalle: " + ex.Message);
                conexion.Desconectar();
                return null;
            }
        }
    }
}