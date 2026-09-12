using System;
using System.Data;
using System.Data.SqlClient;

namespace CafeteriaDB.Clases
{
    public class Compra
    {
        public int ID { get; set; }
        public string NumeroCompra { get; set; }
        public int IDProveedor { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }

        private Conexion conexion;

        public Compra()
        {
            conexion = new Conexion();
        }

        public int Insertar()
        {
            try
            {
                conexion.Conectar();
                string query = @"INSERT INTO Compra (NumeroCompra, IDProveedor, Fecha, Subtotal, IVA, Total, Estado, Observaciones) 
                               VALUES (@numeroCompra, @idProveedor, @fecha, @subtotal, @iva, @total, @estado, @observaciones);
                               SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@numeroCompra", NumeroCompra ?? "");
                cmd.Parameters.AddWithValue("@idProveedor", IDProveedor);
                cmd.Parameters.AddWithValue("@fecha", Fecha);
                cmd.Parameters.AddWithValue("@subtotal", Subtotal);
                cmd.Parameters.AddWithValue("@iva", IVA);
                cmd.Parameters.AddWithValue("@total", Total);
                cmd.Parameters.AddWithValue("@estado", Estado ?? "Completada");
                cmd.Parameters.AddWithValue("@observaciones", Observaciones ?? "");

                object resultado = cmd.ExecuteScalar();
                conexion.Desconectar();

                if (resultado != null)
                    return Convert.ToInt32(resultado);
                return 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al guardar compra: " + ex.Message);
                conexion.Desconectar();
                return 0;
            }
        }

        public DataTable Consultar()
        {
            try
            {
                conexion.Conectar();
                string query = @"SELECT c.ID, c.NumeroCompra, pr.Nombre AS Proveedor, c.Fecha, c.Subtotal, c.IVA, c.Total, c.Estado, c.Observaciones
                               FROM Compra c
                               INNER JOIN Proveedor pr ON c.IDProveedor = pr.ID
                               ORDER BY c.Fecha DESC";
                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conexion.Desconectar();
                return dt;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al consultar compras: " + ex.Message);
                conexion.Desconectar();
                return null;
            }
        }
        public DataTable ConsultarPorFecha(DateTime desde, DateTime hasta)
        {
            try
            {
                conexion.Conectar();
                string query = @"SELECT c.ID, c.NumeroCompra, pr.Nombre AS Proveedor, c.Fecha, c.Subtotal, c.IVA, c.Total, c.Estado
                       FROM Compra c
                       INNER JOIN Proveedor pr ON c.IDProveedor = pr.ID
                       WHERE c.Fecha BETWEEN @desde AND @hasta
                       ORDER BY c.Fecha DESC";
                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@desde", desde);
                cmd.Parameters.AddWithValue("@hasta", hasta);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conexion.Desconectar();
                return dt;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al consultar compras: " + ex.Message);
                conexion.Desconectar();
                return null;
            }
        }
    }
}