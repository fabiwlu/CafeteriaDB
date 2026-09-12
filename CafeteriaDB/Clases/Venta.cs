using System;
using System.Data;
using System.Data.SqlClient;

namespace CafeteriaDB.Clases
{
    public class Venta
    {
        public int ID { get; set; }
        public DateTime Fecha { get; set; }
        public string NroFact { get; set; }
        public string RucCliente { get; set; }
        public decimal TotalIVA { get; set; }
        public decimal TotalPagar { get; set; }
        public string MetodoPago { get; set; }

        private Conexion conexion;

        public Venta()
        {
            conexion = new Conexion();
        }

        public int Insertar()
        {
            try
            {
                conexion.Conectar();
                string query = @"INSERT INTO Venta (Fecha, NroFact, RUC_Cliente, Total_IVA, Total_Pagar, MetodoPago) 
                               VALUES (@fecha, @nroFact, @rucCliente, @totalIVA, @totalPagar, @metodoPago);
                               SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@fecha", Fecha);
                cmd.Parameters.AddWithValue("@nroFact", NroFact ?? "");
                cmd.Parameters.AddWithValue("@rucCliente", RucCliente ?? "");
                cmd.Parameters.AddWithValue("@totalIVA", TotalIVA);
                cmd.Parameters.AddWithValue("@totalPagar", TotalPagar);
                cmd.Parameters.AddWithValue("@metodoPago", MetodoPago ?? "Efectivo");

                object resultado = cmd.ExecuteScalar();
                conexion.Desconectar();

                if (resultado != null)
                    return Convert.ToInt32(resultado);
                return 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al guardar venta: " + ex.Message);
                conexion.Desconectar();
                return 0;
            }
        }

        public bool Anular(int id)
        {
            try
            {
                conexion.Conectar();
                SqlCommand cmd = new SqlCommand("UPDATE Venta SET Estado = 'Anulada' WHERE ID=@id", conexion.ObtenerConexion());
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conexion.Desconectar();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al anular la factura: " + ex.Message);
                conexion.Desconectar();
                return false;
            }
        }

        public DataTable Consultar()
        {
            try
            {
                conexion.Conectar();
                string query = "SELECT ID, Fecha, NroFact, RUC_Cliente, Total_IVA, Total_Pagar, Estado, MetodoPago FROM Venta ORDER BY Fecha DESC";
                SqlCommand cmd = new SqlCommand(query, conexion.ObtenerConexion());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conexion.Desconectar();
                return dt;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al consultar ventas: " + ex.Message);
                conexion.Desconectar();
                return null;
            }
        }
        public DataTable ConsultarPorFecha(DateTime desde, DateTime hasta)
        {
            try
            {
                conexion.Conectar();
                string query = @"SELECT ID, Fecha, NroFact, RUC_Cliente, Total_IVA, Total_Pagar, Estado, MetodoPago 
                       FROM Venta 
                       WHERE Fecha BETWEEN @desde AND @hasta 
                       ORDER BY Fecha DESC";
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
                System.Windows.Forms.MessageBox.Show("Error al consultar ventas: " + ex.Message);
                conexion.Desconectar();
                return null;
            }
        }
    }
}