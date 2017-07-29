using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HPSF;
using NPOI;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Collections;
namespace SemanticTag
{
    //Input / Output functions for Excel
    class IOExcel
    {
        XSSFWorkbook wb; 
        public IOExcel()
        { 
            
        }
        public void excelRead(string path,BasicBase bb)
        {
           
            try
            {
                wb = new XSSFWorkbook();

                    wb.Clear();
                    bb.getList().Clear();
                
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    wb = new XSSFWorkbook(fs);
                    fs.Close();
                }

                foreach (XSSFSheet sh in wb)
                {
                    bb.getList().Add(sh.SheetName);

                }
            wb.Close();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public DataTable genDT(string sheetName,string path)
        {

            XSSFWorkbook workbook = wb;

            
            ISheet sheet = workbook.GetSheet(sheetName);
            
            DataTable dt = new DataTable();
            try
            {
                IRow headerRow = sheet.GetRow(0);
                IEnumerator rows = sheet.GetRowEnumerator();

                int colCount = headerRow.LastCellNum;
                int rowCount = sheet.LastRowNum;

                for (int c = 0; c < colCount; c++)
                    dt.Columns.Add(headerRow.GetCell(c).ToString());

                while (rows.MoveNext())
                {
                    if (rows.Current.Equals(headerRow))
                        continue;

                    IRow row = (XSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();

                    for (int i = 0; i < colCount; i++)
                    {
                        ICell cell = row.GetCell(i);

                        if (cell != null)
                        {
                            dr[i] = cell.ToString();

                        }


                    }
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }

            return dt;
        }

    }
}
