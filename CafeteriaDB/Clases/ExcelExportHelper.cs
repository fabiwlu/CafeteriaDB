using System;
using System.Data;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace CafeteriaDB.Clases
{
    public static class ExcelExportHelper
    {
        public static void ExportarGrilla(DataGridView grilla, string nombreArchivoBase, string nombreHoja = "Informe")
        {
            if (grilla.Rows.Count < 1)
            {
                MessageBox.Show("No hay registros para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable dt = new DataTable();
            foreach (DataGridViewColumn columna in grilla.Columns)
            {
                if (columna.Visible)
                    dt.Columns.Add(columna.HeaderText, typeof(string));
            }

            foreach (DataGridViewRow fila in grilla.Rows)
            {
                if (!fila.IsNewRow)
                {
                    DataRow nuevaFila = dt.NewRow();
                    int col = 0;
                    foreach (DataGridViewColumn columna in grilla.Columns)
                    {
                        if (columna.Visible)
                        {
                            nuevaFila[col] = fila.Cells[columna.Index].Value?.ToString() ?? "";
                            col++;
                        }
                    }
                    dt.Rows.Add(nuevaFila);
                }
            }

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = string.Format("{0}_{1}.xlsx", nombreArchivoBase, DateTime.Now.ToString("ddMMyyyyHHmmss"));
            savefile.Filter = "Excel Files | *.xlsx";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var hoja = wb.Worksheets.Add(dt, nombreHoja);
                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(savefile.FileName);
                    }
                    MessageBox.Show("Reporte generado correctamente.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al generar el reporte.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
    }
}