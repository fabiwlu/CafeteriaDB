using System;
using System.Data;
using System.Data.SqlClient;

namespace CafeteriaDB.Clases
{
    public class DetalleVenta
    {
        public int ID { get; set; }
        public int IDVenta { get; set; }
        public int IDProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }

        private Conexion conexion;

        public DetalleVenta()
        {
            conexion = new Conexion();
        }

        public bool InsertarYDescontarStock()
        {
            try
            {
                conexion.Conectar();
                string query = @"INSERT INTO Detalle_Venta (IDVenta, IDProducto, Cantidad, Precio, Subtotal, IVA) 
                       VALUES (@idVenta, @idProducto, @cantidad, @precio, @subtotal, @iva)";
                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@idVenta", IDVenta);
                cmd.Parameters.AddWithValue("@idProducto", IDProducto);
                cmd.Parameters.AddWithValue("@cantidad", Cantidad);
                cmd.Parameters.AddWithValue("@precio", Precio);
                cmd.Parameters.AddWithValue("@subtotal", Subtotal);
                cmd.Parameters.AddWithValue("@iva", IVA);
                cmd.ExecuteNonQuery();

                // El descuento de stock hace el trigger TR_DescontarStock_Venta
                conexion.Desconectar();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al guardar detalle de venta: " + ex.Message);
                conexion.Desconectar();
                return false;
            }
        }
    }
}